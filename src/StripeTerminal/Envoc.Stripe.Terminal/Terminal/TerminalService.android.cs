﻿using Android;
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
using StripeTerminal.Payment;
using Models = Com.Stripe.Stripeterminal.External.Models;

namespace StripeTerminal;

public partial interface ITerminalService
{
    Task GetHandoffReaders(Action<IList<Reader>> readers, bool simulated = false);
}

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
        IStripeReaderCache readerCache) : this(logger, connectionTokenProviderService, readerCache)
    {
        this.bluetoothConnector = bluetoothConnector ?? new BluetoothConnector(logger);
        this.readerReconnector = readerReconnector ?? new ReaderReconnector();

        this.bluetoothConnector.ReaderUpdateAvailable += OnReaderUpdateAvailable;
        this.bluetoothConnector.ReaderUpdateProgress += OnReaderUpdateProgress;
        this.bluetoothConnector.ReaderUpdateLabel += OnReaderUpdateLabel;
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

    public async Task<bool> Initialize()
    {
        if (!await RequestPermissions())
        {
            return false;
        }

        if (!Terminal.IsInitialized)
        {
            Terminal.InitTerminal(context, new ConnectionTokenProvider(connectionTokenProviderService), this);
        }

        return true;
    }

    public ReaderConnectivityStatus GetConnectivityStatus(DiscoveryType? discoveryType)
    {
        if (!IsTerminalConnected)
        {
            return ReaderConnectivityStatus.Disconnected;
        }

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

    public Task GetHandoffReaders(Action<IList<Reader>> readers, bool simulated = false)
    {
        discoveryConfiguration = new StripeDiscoveryConfiguration
        {
            TimeOut = 15,
            DiscoveryMethod = DiscoveryType.Handoff,
            IsSimulated = simulated
        };

        return GetReaders(discoveryConfiguration, readers);
    }

    private Task DiscoverReaders(StripeDiscoveryConfiguration config)
    {
        var discoveryMethod = config.DiscoveryMethod switch
        {
            DiscoveryType.Bluetooth => DiscoveryMethod.BluetoothScan,
            DiscoveryType.Internet => DiscoveryMethod.Internet,
            DiscoveryType.Local => DiscoveryMethod.LocalMobile,
            DiscoveryType.Handoff => DiscoveryMethod.Handoff,
            _ => throw new NotImplementedException(),
        };

        var configuration = new DiscoveryConfiguration(config.TimeOut ?? 15, discoveryMethod, config.IsSimulated);

        if (discoveryTask != null && !discoveryTask.IsCompleted)
        {
            discoveryTask.Cancel(new GenericCallback((ex) =>
            {
                if (ex != null)
                {
                    Exception($"Discovery cancel failed", ex);
                }
            }));
        }

        if (Instance.ConnectionStatus != ConnectionStatus.NotConnected && config.DiscoveryMethod == connectionType)
        {
            OnUpdateDiscoveredReaders(lastRetrievedReaders);

            return Task.CompletedTask;
        }

        discoveryTask = Instance.DiscoverReaders(configuration, this, new GenericCallback((ex) =>
        {
            Trace($"Discovery timeout");
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
            }
        }));

        return Task.CompletedTask;
    }

    public async Task CancelDiscovery()
    {
        if (!await Initialize())
        {
            return;
        }

        if (discoveryTask != null && !discoveryTask.IsCompleted)
        {
            discoveryTask.Cancel(new GenericCallback((error) =>
            {
                if (error != null)
                {
                    Exception("reader_discover_cancel_error", error);
                }
            }));
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
        var locationId = request.CurrentStripeLocationId ?? selectedReader.Location.Id;
        if (locationId == null)
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

            Instance.ConnectBluetoothReader(selectedReader, config: connectionConfig, listener: bluetoothConnector, new ConnectionCallback(async (connectedReader, error) =>
            {
                if (error != null)
                {
                    Exception("reader_bluetooth_connection_error", error);
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
                    tcs.SetResult(null);
                }
            }));
        }
        else if (discoveryConfiguration.DiscoveryMethod == DiscoveryType.Internet)
        {
            var connectionConfig = new ConnectionConfiguration.InternetConnectionConfiguration();

            try
            {
                Instance.ConnectInternetReader(selectedReader, config: connectionConfig, new ConnectionCallback(async (connectedReader, error) =>
                {
                    if (error != null)
                    {
                        Exception("reader_internet_connection_error", error);
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
                        tcs.SetResult(null);
                    }
                }));
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
                Instance.ConnectHandoffReader(selectedReader, config: connectionConfig, listener: this, new ConnectionCallback(async (connectedReader, error) =>
                {
                    if (error != null)
                    {
                        Exception("reader_handoff_connection_error", error);
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
                        tcs.SetResult(null);
                    }
                }));
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
    }

    public async Task<bool> DisconnectReader()
    {
        if (!await Initialize())
        {
            return false;
        }

        await readerCache.SetLastConnectedReader(null, null);

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

    public async Task<PaymentResponse> CollectPayment(PaymentRequest payment)
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

        try
        {
            var parameters =
                new PaymentIntentParameters
                    .Builder((long)amount, "usd", CaptureMethod.Automatic, new List<PaymentMethodType> { PaymentMethodType.CardPresent })
                    .SetMetadata(payment.Metadata)
                    .Build() as PaymentIntentParameters;

            var tcs = new TaskCompletionSource<PaymentIntent>();

            var intentTask = new TaskCompletionSource<PaymentIntent>();

            Instance.CreatePaymentIntent(parameters, new PaymentIntentCallback((intent, error) =>
            {
                if (intent == null || error != null)
                {
                    intentTask.SetException(error);
                }
                else
                {
                    intentTask.SetResult(intent);
                }
            }));
            var intent = await intentTask.Task;

            if (intent == null || intentTask.Task.Exception != null)
            {
                return PaymentResponse.AsError(intentTask.Task.Exception);
            }

            LogStatusStrings(Instance, intent);

            intentTask = new TaskCompletionSource<PaymentIntent>();
            paymentTask = Instance.CollectPaymentMethod(intent, new PaymentIntentCallback((intent, error) =>
            {
                if (intent == null || error != null)
                {
                    intentTask.SetException(error);
                }
                else
                {
                    intentTask.SetResult(intent);
                }
            }));

            intent = await intentTask.Task;

            if (intent == null || intentTask.Task.Exception != null)
            {
                return PaymentResponse.AsError(intentTask.Task.Exception);
            }

            LogStatusStrings(Instance, intent);

            intentTask = new TaskCompletionSource<PaymentIntent>();
            Instance.ProcessPayment(intent, new PaymentIntentCallback((intent, error) =>
            {
                if (intent == null || error != null)
                {
                    intentTask.SetException(error);
                }
                else
                {
                    intentTask.SetResult(intent);
                }
            }));

            intent = await intentTask.Task;

            if (intent == null || intentTask.Task.Exception != null)
            {
                return PaymentResponse.AsError("Failed to process payment");
            }

            LogStatusStrings(Instance, intent);

            //if (!string.IsNullOrEmpty(intent.Error?.RequestError?.LocalizedDescription))
            //{
            //    return PaymentResponse.AsError(paymentResult.Error.RequestError.LocalizedDescription);
            //}

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

        paymentTask.Cancel(new GenericCallback(error =>
        {
            if (error == null)
            {
                return;
            }

            Exception("reader_payment_cancel_error", error);
        }));
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

    public void OnUnexpectedReaderDisconnect(Com.Stripe.Stripeterminal.External.Models.Reader reader)
    {
        //TODO:
        //throw new NotImplementedException();
        Trace("Unexpected Disconnected");
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