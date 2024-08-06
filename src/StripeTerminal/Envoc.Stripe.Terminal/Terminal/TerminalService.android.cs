using Android;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Com.Stripe.Stripeterminal;
using Com.Stripe.Stripeterminal.External.Callable;
using Com.Stripe.Stripeterminal.External.Models;
using StripeTerminal.Bluetooth;
using StripeTerminal.Connectivity;
using StripeTerminal.Helpers;
using StripeTerminal.Internet;
using StripeTerminal.Payment;
using System.Threading;
using Models = Com.Stripe.Stripeterminal.External.Models;

namespace StripeTerminal;

public partial class TerminalService : Java.Lang.Object, ITerminalListener, IDiscoveryListener, IHandoffReaderListener
{
    private static ICancelable discoveryTask = null;
    private static ICancelable paymentTask = null;
    private static Context context;
    private static Func<Activity> getActivity;
    private static Activity activity;
    private List<Models.Reader> lastRetrievedReaders = new();
    private BluetoothConnector bluetoothConnector = null;
    private ReaderReconnector readerReconnector = null;

    public TerminalService(
        IStripeTerminalLogger logger,
        IConnectionTokenProviderService connectionTokenProviderService,
        BluetoothConnector bluetoothConnector,
        ReaderReconnector readerReconnector,
        IStripeReaderCache readerCache,
        IInternetReaderStatusService internetReaderStatusService) : this(logger, connectionTokenProviderService, readerCache, internetReaderStatusService)
    {
        this.bluetoothConnector = bluetoothConnector ?? new BluetoothConnector(logger, new EmptyBatteryUpdater());
        this.readerReconnector = readerReconnector ?? new ReaderReconnector();

        this.bluetoothConnector.ReaderUpdateAvailable += OnReaderUpdateAvailable;
        this.bluetoothConnector.ReaderUpdateProgress += OnReaderUpdateProgress;
        this.bluetoothConnector.ReaderUpdateLabel += OnReaderUpdateLabel;
        this.bluetoothConnector.ReaderErrorMessage += OnReaderErrorMessage;
        this.bluetoothConnector.ReaderBatteryUpdate += OnReaderBatteryUpdate;
        this.readerReconnector.ConnectionStatusChangedEvent += OnConnectionStatusChanged;
    }

    protected Terminal Instance => Terminal.Instance;

    public bool IsTerminalConnected => IsTerminalInitialized && Instance.ConnectionStatus == ConnectionStatus.Connected;

    public bool IsTerminalInitialized => Terminal.IsInitialized;

    public static void InitializeContext(Context context, Func<Activity> getActivity)
    {
        TerminalService.context = context;
        TerminalService.getActivity = getActivity;
    }

    public static void InitializeContext(Context context, Activity activity)
    {
        TerminalService.context = context;
        TerminalService.activity = activity;
    }

    public static void InitializeContext(Android.App.Application application)
    {
        TerminalApplicationDelegate.OnCreate(application);
    }

    public async Task<bool> Initialize()
    {
        try
        {
            if (!await RequestPermissions())
            {
                return false;
            }

            if (!Terminal.IsInitialized)
            {
                Terminal.InitTerminal(context, new ConnectionTokenProvider(connectionTokenProviderService), this);
            }
        }
        catch (Exception ex)
        {
            ;
            System.Diagnostics.Debug.WriteLine(ex);
            System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            throw;
        }


        return true;
    }

    public ReaderConnectivityStatus GetConnectivityStatus(DiscoveryType? discoveryType)
    {
        if (!IsTerminalInitialized)
        {
            return ReaderConnectivityStatus.Disconnected;
        }

        var _ = Terminal.Instance.ConnectionStatus.ToString();

        if (Instance == null || Instance.ConnectionStatus == ConnectionStatus.NotConnected)
        {
            return ReaderConnectivityStatus.Disconnected;
        }

        if (Instance.ConnectionStatus == ConnectionStatus.Connecting)
        {
            return ReaderConnectivityStatus.Connecting;
        }

        if (Instance.ConnectionStatus == ConnectionStatus.Connected)
        {
            if (discoveryType == null)
            {
                return ReaderConnectivityStatus.Unknown;
            }

            return discoveryType switch
            {
                DiscoveryType.Bluetooth => ReaderConnectivityStatus.Bluetooth,
                DiscoveryType.Internet => ReaderConnectivityStatus.Internet,
                DiscoveryType.Local => ReaderConnectivityStatus.Local,
                DiscoveryType.Handoff => ReaderConnectivityStatus.Handoff,
                _ => ReaderConnectivityStatus.Unknown
            };
        }

        return ReaderConnectivityStatus.Unknown;
    }

    #region Discovery

    private async Task DiscoverReaders(StripeDiscoveryConfiguration config)
    {
        var timeout = config.TimeOut ?? 15;

        IDiscoveryConfiguration configuration = config.DiscoveryMethod switch
        {
            DiscoveryType.Bluetooth => new IDiscoveryConfiguration.BluetoothDiscoveryConfiguration(timeout, config.IsSimulated),
            DiscoveryType.Internet => new IDiscoveryConfiguration.InternetDiscoveryConfiguration(config.LocationId, config.IsSimulated),
            DiscoveryType.Local => new IDiscoveryConfiguration.LocalMobileDiscoveryConfiguration(config.IsSimulated),
            DiscoveryType.Handoff => new IDiscoveryConfiguration.HandoffDiscoveryConfiguration(),
            _ => throw new NotImplementedException(),
        };

        await CancelDiscoveryImplementation();

        if (Instance.ConnectionStatus != ConnectionStatus.NotConnected && config.DiscoveryMethod == connectionType)
        {
            //Need to make a copy with .ToList() because the underlying method modifies the collection
            OnUpdateDiscoveredReaders(lastRetrievedReaders.ToList());

            return;
        }

        discoveryTask = Instance.DiscoverReaders(configuration, this, new GenericCallback((ex) =>
        {
            Trace($"{config.DiscoveryMethod} Discovery timeout");
            string[] ignoreErrors =
            {
                TerminalException.TerminalErrorCode.BluetoothScanTimedOut.ToString(),
                TerminalException.TerminalErrorCode.Canceled.ToString(),
            };

            if (ex != null && !ignoreErrors.Contains(ex.ErrorCode?.ToString()))
            {
                Trace($"Discovery ErrorCode: {ex.ErrorCode?.Name()}");
                Trace($"Discovery ErrorMessage: {ex.ErrorMessage}");
                Exception("reader_discover_error", ex);

                //If it errors out, we want the loading to finish
                onReadersDiscoveredAction?.Invoke([]);

                ReaderErrorMessage?.Invoke(null, new ReaderUpdateLabelEventArgs(ex.ErrorMessage, showCancel: false));
            }
        }));
    }

    public async Task CancelDiscovery()
    {
        if (!await Initialize())
        {
            return;
        }

        await CancelDiscoveryImplementation();
    }

    private async Task CancelDiscoveryImplementation()
    {
        if (discoveryTask != null && !discoveryTask.IsCompleted)
        {
            var tcs = new TaskCompletionSource();

            discoveryTask.Cancel(new GenericCallback((error) =>
            {
                if (error != null)
                {
                    Exception("reader_discover_cancel_error", error);
                }

                tcs.TrySetResult();
            }));

            await tcs.Task;
        }
    }

    #endregion

    #region Connection

    public async Task<Reader> ConnectReader(ReaderConnectionRequest request)
    {
        if (!await Initialize())
        {
            return null;
        }

        var reader = request.Reader;
        if (reader == null || lastRetrievedReaders == null || !lastRetrievedReaders.Any())
        {
            Info("reader_connection_failure", new()
            {
                ["reader_label"] = reader?.Label ?? StripeTerminalLoggerConfiguration.NullValueReplacement,
                ["readers_count"] = lastRetrievedReaders?.Count ?? 0
            });

            return null;
        }

        var selectedReader = lastRetrievedReaders.FirstOrDefault(x => x.SerialNumber == reader.SerialNumber);
        if (selectedReader == null)
        {
            Info("reader_connection_mismatch", new()
            {
                ["reader_label"] = reader.Label,
                ["readers_count"] = lastRetrievedReaders.Count
            });

            return null;
        }

        //TODO: Do we always overwrite the location?
        //locationId = selectedReader.LocationId ?? request.CurrentStripeLocationId
        var locationId = request.CurrentStripeLocationId ?? selectedReader.Location?.Id;
        if (locationId == null && (discoveryConfiguration.DiscoveryMethod == DiscoveryType.Bluetooth || discoveryConfiguration.DiscoveryMethod == DiscoveryType.Local))
        {
            Info("reader_location_empty");
            return null;
        }

        if (Instance.ConnectionStatus != ConnectionStatus.NotConnected)
        {
            var disconnected = await DisconnectReader();
            if (!disconnected)
            {
                return null;
            }
            else
            {
                await GetReaders(discoveryConfiguration, null);
                //Miniscule race condition
                //HACK:Needed to ensure SDK finishes current operation 
                await Task.Delay(100);
            }
        }

        if (discoveryConfiguration == null)
        {
            return null;
        }

        var tcs = new TaskCompletionSource<Reader>();

        if (discoveryConfiguration.DiscoveryMethod == DiscoveryType.Bluetooth)
        {
            var connectionConfig = new ConnectionConfiguration.BluetoothConnectionConfiguration(
              // When connecting to a physical reader, your integration should specify either the
              // same location as the last connection (selectedReader.locationId) or a new location
              // of your user's choosing.
              //
              locationId: locationId,
              autoReconnectOnUnexpectedDisconnect: true,
              readerReconnector
            );

            Instance.ConnectBluetoothReader(selectedReader, config: connectionConfig, listener: bluetoothConnector, new ConnectionCallback(SetConnectionDelegate));
        }
        else if (discoveryConfiguration.DiscoveryMethod == DiscoveryType.Internet)
        {
            var connectionConfig = new ConnectionConfiguration.InternetConnectionConfiguration();

            try
            {
                Instance.ConnectInternetReader(selectedReader, config: connectionConfig, new ConnectionCallback(SetConnectionDelegate));
            }
            catch (Exception ex)
            {
                tcs.SetResult(null);
            }
        }
        else if (discoveryConfiguration.DiscoveryMethod == DiscoveryType.Handoff)
        {
            var connectionConfig = new ConnectionConfiguration.HandoffConnectionConfiguration();

            try
            {
                Instance.ConnectHandoffReader(selectedReader, config: connectionConfig, listener: this, new ConnectionCallback(SetConnectionDelegate));
            }
            catch (Exception ex)
            {
                tcs.SetResult(null);
            }
        }
        else if (discoveryConfiguration.DiscoveryMethod == DiscoveryType.Local)
        {
            var connectionConfig = new ConnectionConfiguration.LocalMobileConnectionConfiguration(locationId: locationId);

            try
            {
                Instance.ConnectLocalMobileReader(selectedReader, config: connectionConfig, new ConnectionCallback(SetConnectionDelegate));
            }
            catch (Exception ex)
            {
                tcs.SetResult(null);
            }
        }
        else
        {
            return null;
        }

        return await tcs.Task;

        async void SetConnectionDelegate(Models.Reader connectedReader, TerminalException error)
        {
            var readerType = discoveryConfiguration.DiscoveryMethod.ToString().ToLower();

            if (error != null)
            {
                Exception($"reader_{readerType}_connection_error", error);
            }

            if (connectedReader != null)
            {
                currentReader = request.Reader;
                connectionType = discoveryConfiguration.DiscoveryMethod;
                await readerCache.SetLastConnectedReader(currentReader, connectionType);

                tcs.SetResult(connectedReader.FromNative());
            }
            else
            {
                var errorString = error?.LocalizedMessage ?? ConstantStrings.GenericReaderConnectionError;

                ReaderErrorMessage?.Invoke(null, new ReaderUpdateLabelEventArgs(errorString, false));

                tcs.SetResult(null);
            }
        }
    }

    private async Task<bool> DisconnectReaderImplementation()
    {
        if (Instance.ConnectionStatus == ConnectionStatus.NotConnected)
        {
            return true;
        }

        var tcs = new TaskCompletionSource<bool>();

        try
        {
            Instance.DisconnectReader(new GenericCallback(error =>
            {
                if (error != null)
                {
                    Exception("reader_disconnection_error", error);
                }

                connectionType = null;
                tcs.SetResult(error == null);
            }));
        }
        catch (Exception)
        {
            tcs.SetResult(false);
        }

        return await tcs.Task;
    }

    #endregion

    #region Permissions

    public async Task<bool> RequestPermissions()
    {
        if (context == null || (activity == null && getActivity == null))
        {
            throw new Exception($"You must call {nameof(InitializeContext)} and supply context and activity before we can request check permissions");
        }

        var result = RequestBluetoothPermission();
        result &= RequestLocationPermission();
        return result;
    }

    private bool RequestBluetoothPermission()
    {
        int permissionRequestCode = 12347;
        var currentActvity = activity ?? getActivity?.Invoke();

        if ((uint)Build.VERSION.SdkInt >= 31)
        {
            if (ContextCompat.CheckSelfPermission(context, Manifest.Permission.BluetoothScan) != Permission.Granted ||
                ContextCompat.CheckSelfPermission(context, Manifest.Permission.BluetoothConnect) != Permission.Granted)
            {
                ActivityCompat.RequestPermissions(currentActvity, new[] { Manifest.Permission.BluetoothScan, Manifest.Permission.BluetoothConnect }, permissionRequestCode);
            }
            else
            {
                return true;
            }
        }
        else
        {
            if (ContextCompat.CheckSelfPermission(context, Manifest.Permission.Bluetooth) != Permission.Granted)
            {
                ActivityCompat.RequestPermissions(currentActvity, new[] { Manifest.Permission.Bluetooth }, permissionRequestCode);
            }
            else
            {
                return true;
            }
        }

        return false;
    }

    private bool RequestLocationPermission()
    {
        int permissionRequestCode = 12348;
        var currentActvity = activity ?? getActivity?.Invoke();

        if ((uint)Build.VERSION.SdkInt <= 28)
        {
            if (ContextCompat.CheckSelfPermission(context, Manifest.Permission.AccessCoarseLocation) != Permission.Granted)
            {
                ActivityCompat.RequestPermissions(currentActvity, new[] { Manifest.Permission.Bluetooth, Manifest.Permission.BluetoothAdmin, Manifest.Permission.AccessCoarseLocation }, permissionRequestCode);
            }
            else
            {
                return true;
            }
        }
        else
        {
            if (ContextCompat.CheckSelfPermission(context, Manifest.Permission.AccessFineLocation) != Permission.Granted)
            {
                ActivityCompat.RequestPermissions(currentActvity, new[] { Manifest.Permission.AccessFineLocation }, permissionRequestCode);
            }
            else
            {
                return true;
            }
        }

        return false;
    }

    #endregion

    #region Payment

    public async Task<PaymentResponse> CollectPayment(PaymentRequest payment, CancellationToken token = default)
    {
        if (!await Initialize())
        {
            return PaymentResponse.AsError("Payments not initialized.");
        }

        if (Instance.ConnectionStatus != ConnectionStatus.Connected)
        {
            return PaymentResponse.AsError("Not connected to a reader. Please connect a reader to collect payment.");
        }

        var amount = payment.GetAmount();

        if (StripeTerminalConfiguration.DeclineTransaction)
        {
            amount = StripeTestAmount.GetDeclinedAmount(amount);
        }

        if (token.IsCancellationRequested)
        {
            return new PaymentResponse();
        }

        var intentTask = new TaskCompletionSource<PaymentIntent>();
        try
        {
            var parameters =
                new PaymentIntentParameters
                    .Builder((long)amount, "usd", CaptureMethod.Automatic, new List<PaymentMethodType> { PaymentMethodType.CardPresent })
                    .SetMetadata(payment.Metadata)
                    .Build() as PaymentIntentParameters;

            var collectionConfig =
                new CollectConfiguration
                    .Builder()
                    .Build() as CollectConfiguration;

            if (string.IsNullOrEmpty(payment.PaymentIntentSecret))
            {
                Instance.CreatePaymentIntent(parameters, GetPaymentIntentCallback());
            }
            else
            {
                Instance.RetrievePaymentIntent(payment.PaymentIntentSecret, GetPaymentIntentCallback());
            }

            var intent = await intentTask.Task;
            if (intent == null || intentTask.Task.Exception != null)
            {
                return PaymentResponse.AsError(intentTask.Task.Exception);
            }

            LogStatusStrings(Instance, intent);

            intentTask = new TaskCompletionSource<PaymentIntent>();

            //ensure that collect payment & cancel payment aren't executing at the same time
            lock (_locker)
            {
                if (token.IsCancellationRequested)
                {
                    return new PaymentResponse();
                }

                paymentTask = Instance.CollectPaymentMethod(intent, GetPaymentIntentCallback(), collectionConfig);
            }

            intent = await intentTask.Task;
            if (intent == null || intentTask.Task.Exception != null)
            {
                return PaymentResponse.AsError(intentTask.Task.Exception);
            }

            LogStatusStrings(Instance, intent);

            intentTask = new TaskCompletionSource<PaymentIntent>();
            Instance.ConfirmPaymentIntent(intent, GetPaymentIntentCallback());

            intent = await intentTask.Task;
            if (intent == null || intentTask.Task.Exception != null)
            {
                return PaymentResponse.AsError("Failed to process payment");
            }

            LogStatusStrings(Instance, intent);

            ////Necessary if payment is successful?
            //if (intent.Status == SCPPaymentIntentStatus.Succeeded)
            //{

            //}            

            return new PaymentResponse
            {
                Id = intent.Id
            };
        }
        catch (Exception ex) when (ex.Message.ToUpper().Contains("CANCELED"))
        {
            return new PaymentResponse();
        }
        catch (Exception ex)
        {
            return PaymentResponse.AsError(ex.Message);
        }

        PaymentIntentCallback GetPaymentIntentCallback() => new PaymentIntentCallback((intent, error) =>
        {
            if (intent == null || error != null)
            {
                intentTask.SetException(error);
            }
            else
            {
                intentTask.SetResult(intent);
            }
        });
    }

    private void LogStatusStrings(Terminal terminal, PaymentIntent intent)
    {
        var intentStatus = intent.Status.Name(); //requires_payment_method, requires_confirmation, succeeded
        var paymentStatus = terminal.PaymentStatus.Name(); //always Ready (doesn't change)
        var captureMethod = intent.CaptureMethod; //always Automatic (doesn't change)

        Trace($"{nameof(intentStatus)}: {intent.Status} : {intentStatus}");
        Trace($"{nameof(paymentStatus)}: {terminal.PaymentStatus} : {paymentStatus}");
        Trace($"{nameof(captureMethod)}: {intent.CaptureMethod} : {captureMethod}");
    }

    public void CancelPayment()
    {
        if (!IsTerminalInitialized)
        {
            return;
        }

        if (paymentTask == null)
        {
            return;
        }

        if (paymentTask.IsCompleted)
        {
            return;
        }

        lock (_locker)
        {
            paymentTask.Cancel(new GenericCallback(error =>
            {
                if (error == null)
                {
                    return;
                }

                Exception("reader_payment_cancel_error", error);
            }));
        }
    }

    #endregion

    #region Update

    public async Task UpdateConnectedReader()
    {
        if (!await Initialize())
        {
            return;
        }

        //BUG: In case loading dialog was showing and just closed
        //https://github.com/CommunityToolkit/Maui/issues/1213#issuecomment-1580300733
        //https://github.com/CommunityToolkit/Maui/issues/1111
        //https://github.com/CommunityToolkit/Maui/pull/1223
        await Task.Delay(500);

        Instance.InstallAvailableUpdate();
        //return Task.CompletedTask;
    }

    public async Task SetSimulatedUpdate(bool updateRequired)
    {
        if (!await Initialize())
        {
            return;
        }

        if (Instance.SimulatorConfiguration == null)
        {
            return;
        }

        Instance.SimulatorConfiguration = new SimulatorConfiguration(
            updateRequired
                ? SimulateReaderUpdate.UpdateAvailable
                : SimulateReaderUpdate.None,
        new SimulatedCard(StripeTestCards.VISA), null);
    }

    #endregion

    #region ITerminalListener

    public async void OnUnexpectedReaderDisconnect(Com.Stripe.Stripeterminal.External.Models.Reader reader)
    {
        Trace("Unexpected Disconnected");

        try
        {
            await ReconnectToDisconnectedInternetReader(reader.FromNative());
        }
        catch (TaskCanceledException)
        {
            internetReaderCancellationToken = null;

            Trace($"Reconnection cancelled");
        }
    }

    public void OnConnectionStatusChange(ConnectionStatus status)
    {
        Trace($"Connection Status Changed to: {status?.Name()}");
    }

    public void OnPaymentStatusChange(PaymentStatus status)
    {
        Trace($"Payment Status Changed to: {status?.Name()}");
    }

    #endregion

    #region IDiscoveryListener
    public void OnUpdateDiscoveredReaders(IList<Com.Stripe.Stripeterminal.External.Models.Reader> readers)
    {
        OnDiscoveredReaders(readers?.Count, readers, Instance.ConnectedReader);
    }

    #endregion

    #region IHandoffReaderListener

    public void OnReportReaderEvent(global::Com.Stripe.Stripeterminal.External.Models.ReaderEvent @event)
    {
        var message = @event.Name();
        logger?.Trace(StripeTerminalConfiguration.LoggerTracePrefix + $"{nameof(Models.ReaderEvent)}: {@event} : {message}");
    }

    #endregion

    #region Events

    private void OnReaderUpdateAvailable(object sender, Bluetooth.ReaderSoftwareUpdateEventArgs e)
    {
        //var selectedReader = lastRetrievedReaders.FirstOrDefault(x => x.SerialNumber == e.Serial);
        //if (selectedReader != null && sender is Models.Reader reader)
        //{
        //    lastRetrievedReaders.Remove(selectedReader);
        //    lastRetrievedReaders.Add(reader);
        //    RefreshReader?.Invoke(null, new RefreshReaderEventArgs(Reader.Map(reader)));
        //}
        if (currentReader != null)
        {
            var selectedReader = lastRetrievedReaders.FirstOrDefault(x => x.SerialNumber == currentReader.SerialNumber);
            //lastRetrievedReaders.Remove(selectedReader);
            //lastRetrievedReaders.Add(reader);

            selectedReader.AvailableUpdate = e.SoftwareUpdate;

            RefreshReader?.Invoke(null, new RefreshReaderEventArgs(selectedReader.FromNative()));
        }
    }

    #endregion
}