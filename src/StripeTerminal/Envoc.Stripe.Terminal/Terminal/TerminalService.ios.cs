using StripeTerminal.Bluetooth;
using StripeTerminal.Connectivity;
using StripeTerminal.Internet;
using StripeTerminal.LocalMobile;
using StripeTerminal.Offline;
using StripeTerminal.Payment;

namespace StripeTerminal;

public partial class TerminalService : SCPDiscoveryDelegate
{
    private readonly TerminalDelegate terminalDelegate; 

    private SCPCancelable paymentTask = null;
    private SCPCancelable discoveryTask = null;
    private List<SCPReader> lastRetrievedReaders = new();
    private readonly BluetoothConnector bluetoothConnector = null;
    private readonly ReaderReconnector readerReconnector = null;
    private readonly LocalMobileConnector localMobileConnector = null;
    private readonly OfflineConnector offlineConnector = null;

    public TerminalService(
        IStripeTerminalLogger logger,
        IConnectionTokenProviderService connectionTokenProviderService,
        TerminalDelegate terminalDelegate,
        BluetoothConnector bluetoothConnector,
        ReaderReconnector readerReconnector,
        IStripeReaderCache readerCache,
        IInternetReaderStatusService internetReaderStatusService) : this(logger, connectionTokenProviderService, readerCache, internetReaderStatusService)
    {
        this.terminalDelegate = terminalDelegate; 
        this.bluetoothConnector = bluetoothConnector ?? new BluetoothConnector(logger, new EmptyBatteryUpdater());
        this.readerReconnector = readerReconnector ?? new ReaderReconnector();

        //TODO: Add DI when implementing
        this.localMobileConnector = new LocalMobileConnector(logger);
        this.offlineConnector = new OfflineConnector();

        this.bluetoothConnector.ReaderUpdateAvailable += OnReaderUpdateAvailable;
        this.bluetoothConnector.ReaderUpdateProgress += OnReaderUpdateProgress;
        this.bluetoothConnector.ReaderUpdateLabel += OnReaderUpdateLabel;
        this.bluetoothConnector.ReaderErrorMessage += OnReaderErrorMessage;
        this.bluetoothConnector.ReaderBatteryUpdate += OnReaderBatteryUpdate;
        this.readerReconnector.ConnectionStatusChangedEvent += OnConnectionStatusChanged;
        this.terminalDelegate.UnexpectedDisconnect += OnUnexpectedDisconnect;
    }

    protected SCPTerminal Instance => SCPTerminal.Shared;

    public bool IsTerminalConnected => IsTerminalInitialized && Instance.ConnectionStatus == SCPConnectionStatus.Connected;

    public bool IsTerminalInitialized => SCPTerminal.HasTokenProvider;

    public ReaderConnectivityStatus GetConnectivityStatus(DiscoveryType? discoveryType)
    {
        if (!IsTerminalConnected)
        {
            return ReaderConnectivityStatus.Disconnected;
        }

        if (Instance == null || Instance.ConnectionStatus == SCPConnectionStatus.NotConnected)
        {
            return ReaderConnectivityStatus.Disconnected;
        }

        if (Instance.ConnectionStatus == SCPConnectionStatus.Connecting)
        {
            return ReaderConnectivityStatus.Connecting;
        }

        if (Instance.ConnectionStatus == SCPConnectionStatus.Connected)
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
                _ => ReaderConnectivityStatus.Unknown
            };
        }

        return ReaderConnectivityStatus.Unknown;
    }

    public Task<bool> Initialize()
    {
        if (!SCPTerminal.HasTokenProvider)
        {
            SCPTerminal.SetTokenProvider(new ConnectionTokenProvider(connectionTokenProviderService));
        }

        if (terminalDelegate != null && 
            Instance != null && 
            Instance.Delegate == null)
        {
            Instance.Delegate = terminalDelegate;
        }

        //TODO: When implementing offline
        /*
        if (offlineConnector != null &&
            Instance != null &&
            Instance.OfflineDelegate == null)
        {
            Instance.OfflineDelegate = offlineConnector;
        }
        */

        return Task.FromResult(true);
    }

    private void Error(string message, NSError error)
    {
        logger?.Exception(message, error);
    }

    #region Discovery

    private async Task DiscoverReaders(StripeDiscoveryConfiguration config)
    {
        var discoveryMethod = config.DiscoveryMethod switch
        {
            DiscoveryType.Bluetooth => SCPDiscoveryMethod.BluetoothScan,
            DiscoveryType.Internet => SCPDiscoveryMethod.Internet,
            DiscoveryType.Local => SCPDiscoveryMethod.LocalMobile,
            _ => throw new NotImplementedException(),
        };

        var timeout = (nuint)(config.TimeOut ?? 15);

        NSError error;
        var configuration = config.DiscoveryMethod switch
        {
            /*
            //https://stripe.dev/stripe-terminal-ios/docs/Reader%20Discovery%20%26%20Connection.html#/c:objc(cs)SCPBluetoothProximityDiscoveryConfiguration
            //If your app will be used in a busy environment with multiple iOS devices pairing to multiple available readers at the same time, we recommend using this discovery method.
            DiscoveryType.Bluetooth => new SCPBluetoothProximityDiscoveryConfigurationBuilder()
                .SetSimulated(config.IsSimulated)
                .Build(out error),
            */

            DiscoveryType.Bluetooth => new SCPBluetoothScanDiscoveryConfigurationBuilder()
                .SetTimeout(timeout)
                .SetSimulated(config.IsSimulated)
                .Build(out error),

            DiscoveryType.Internet => new SCPInternetDiscoveryConfigurationBuilder()
                .SetLocationId(config.LocationId)
                .SetSimulated(config.IsSimulated)
                .Build(out error),

            DiscoveryType.Local => new SCPLocalMobileDiscoveryConfigurationBuilder()
                .SetSimulated(config.IsSimulated)
                .Build(out error),

            _ => throw new NotImplementedException(),
        };

        await CancelDiscoveryImplementation();

        if (Instance.ConnectionStatus != SCPConnectionStatus.NotConnected && config.DiscoveryMethod == connectionType)
        {
            DidUpdateDiscoveredReaders(Instance, lastRetrievedReaders.ToArray());

            return;
        }

        discoveryTask = Instance.DiscoverReaders(configuration, this, error =>
        {
            nint[] ignoreErrors =
            {
                (nint)SCPError.Canceled,
                (nint)SCPError.BluetoothScanTimedOut
            };

            if (error != null && !ignoreErrors.Contains(error.Code))
            {
                Trace($"Discovery ErrorCode: {error.Code}");
                Trace($"Discovery ErrorMessage: {error.LocalizedDescription}");
                Error("reader_discover_error", error);

                //If it errors out, we want the loading to finish
                onReadersDiscoveredAction?.Invoke([]);

                ReaderErrorMessage?.Invoke(this, new ReaderUpdateLabelEventArgs(error.LocalizedDescription, showCancel: false));
            }
        });
    }

    public async Task CancelDiscovery()
    {
        await Initialize();

        await CancelDiscoveryImplementation();
    }

    private async Task CancelDiscoveryImplementation()
    {
        if (discoveryTask != null && !discoveryTask.Completed)
        {
            var tcs = new TaskCompletionSource();

            discoveryTask.Cancel(error =>
            {
                if (error != null)
                {
                    Error("reader_discover_cancel_error", error);
                }

                tcs.TrySetResult();
            });

            await tcs.Task;
        }
    }

    #endregion

    #region Connection

    public async Task<Reader> ConnectReader(ReaderConnectionRequest request)
    {
        await Initialize();

        var reader = request.Reader;
        if (reader == null || lastRetrievedReaders == null || !lastRetrievedReaders.Any())
        {
            Info("reader_connection_failure", new ()
            {
                ["reader_label"] = reader?.Label ?? StripeTerminalLoggerConfiguration.NullValueReplacement,
                ["readers_count"] = lastRetrievedReaders?.Count ?? 0
            });

            return null;
        }

        var selectedReader = lastRetrievedReaders.FirstOrDefault(x => x.SerialNumber == reader.SerialNumber);
        if (selectedReader == null)
        {
            Info("reader_connection_mismatch", new ()
            {
                ["reader_label"] = reader.Label,
                ["readers_count"] = lastRetrievedReaders.Count
            });

            return null;
        }

        //TODO: Do we always overwrite the location?
        //locationId = selectedReader.LocationId ?? request.CurrentStripeLocationId
        var locationId = request.CurrentStripeLocationId ?? selectedReader.LocationId;
        if (locationId == null)
        {
            Info("reader_location_empty");
            return null;
        }

        if (Instance.ConnectionStatus != SCPConnectionStatus.NotConnected)
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

        NSError builderError;
        if (discoveryConfiguration.DiscoveryMethod == DiscoveryType.Bluetooth)
        {
            // When connecting to a physical reader, your integration should specify either the
            // same location as the last connection (selectedReader.locationId) or a new location
            // of your user's choosing.
            var connectionConfig = new SCPBluetoothConnectionConfigurationBuilder()
                .SetLocationId(locationId)
                .SetAutoReconnectOnUnexpectedDisconnect(true)
                .SetAutoReconnectionDelegate(readerReconnector)
                .Build(out builderError);

            Instance.ConnectBluetoothReader(selectedReader, bluetoothConnector, connectionConfig, SetConnectionDelegate);
        }
        else if (discoveryConfiguration.DiscoveryMethod == DiscoveryType.Internet)
        {
            var connectionConfig = new SCPInternetConnectionConfigurationBuilder()
                .SetAllowCustomerCancel(true)
                .SetFailIfInUse(true)
                .Build(out builderError);

            Instance.ConnectInternetReader(selectedReader, connectionConfig, SetConnectionDelegate);
        }
        else if (discoveryConfiguration.DiscoveryMethod == DiscoveryType.Local)
        {
            var connectionConfig = new SCPLocalMobileConnectionConfigurationBuilder(locationId)
                .SetLocationId(locationId)
                .Build(out builderError);

            Instance.ConnectLocalMobileReader(selectedReader, localMobileConnector, connectionConfig, SetConnectionDelegate);
        }
        else
        {
            return null;
        }

        return await tcs.Task;

        /*
        // Unused but corresponding method for the binding to Async method if we change to that in the future
        async Task<Reader> SetConnectionTask(Task<SCPReader> readerTask, string readerType = null)
        {
            readerType ??= discoveryConfiguration.DiscoveryMethod.ToString().ToLower();

            var reader = await readerTask;
            if (readerTask.Exception != null && readerTask.Exception.GetBaseException() is NSErrorException ex)
            {
                Error($"reader_{readerType}_connection_error", ex.Error);
            }

            if (reader != null)
            {
                currentReader = request.Reader;
                connectionType = discoveryConfiguration.DiscoveryMethod;
                await readerCache.SetLastConnectedReader(currentReader, connectionType);

                return reader.FromNative();
            }
            else
            {
                return null;
            }
        }
        */

        async void SetConnectionDelegate(SCPReader connectedReader, NSError error)
        {
            var readerType = discoveryConfiguration.DiscoveryMethod.ToString().ToLower();

            if (error != null)
            {
                Error($"reader_{readerType}_connection_error", error);
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
                var errorString = error?.LocalizedDescription ?? ConstantStrings.GenericReaderConnectionError;

                ReaderErrorMessage?.Invoke(null, new ReaderUpdateLabelEventArgs(errorString, false));

                tcs.SetResult(null);
            }
        }
    }

    private async Task<bool> DisconnectReaderImplementation()
    {        
        if (Instance.ConnectionStatus == SCPConnectionStatus.NotConnected)
        {
            return true;
        }

        var tcs = new TaskCompletionSource<bool>();

        try
        {
            Instance.DisconnectReader(async error =>
            {
                if (error != null)
                {
                    Error("reader_disconnection_error", error);
                }

                //BUG: https://github.com/stripe/stripe-terminal-ios/issues/229#issuecomment-1568649017
                if (StripeTerminalConfiguration.UseSimulatedReaders)
                {
                    int counter = 0;
                    while (Instance.ConnectionStatus == SCPConnectionStatus.Connected && counter < 10)
                    {
                        await Task.Delay(1000);
                        counter++;
                    }
                }

                connectionType = null;
                tcs.SetResult(error == null);
            });
        }
        catch (Exception)
        {
            tcs.SetResult(false);
        }

        return await tcs.Task;
    }

    #endregion

    #region Payment

    public void CancelPayment()
    {
        if (paymentTask == null)
        {
            return;
        }

        if (paymentTask.Completed)
        {
            return;
        }

        lock (_locker)
        {
            paymentTask.Cancel(error =>
            {
                if (error == null)
                {
                    return;
                }

                Error("reader_payment_cancel_error", error);
            });
        }
    }

    public async Task<PaymentResponse> CollectPayment(PaymentRequest payment, CancellationToken token = default)
    {
        await Initialize();

        if (Instance.ConnectionStatus != SCPConnectionStatus.Connected)
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

        try
        {
            var parameters =
                new SCPPaymentIntentParametersBuilder(amount, "usd")
                    //.SetAmount(amount)
                    //.SetCurrency("usd")
                    .SetPaymentMethodTypes(new[] { "card_present" })
                    .SetCaptureMethod(SCPCaptureMethod.Automatic)
                    .SetMetadata(payment.Metadata.ToNSDictionary())
                    .Build(out var builderError);

            var tcs = new TaskCompletionSource<SCPPaymentIntent>();

            Task<SCPPaymentIntent> intentTask = null;
            if (string.IsNullOrEmpty(payment.PaymentIntentSecret))
            {
                intentTask = Instance.CreatePaymentIntentAsync(parameters);
            }
            else
            {
                intentTask = Instance.RetrievePaymentIntentAsync(payment.PaymentIntentSecret);
            }

            var intent = await intentTask;

            if (intent == null)
            {
                return PaymentResponse.AsError(intentTask.Exception);
            }

            LogStatusStrings(Instance, intent);

            //ensure that collect payment & cancel payment aren't executing at the same time
            lock (_locker)
            {
                if (token.IsCancellationRequested)
                {
                    return new PaymentResponse();
                }

                intentTask = Instance.CollectPaymentMethodAsync(intent, out SCPCancelable cancelable);
                paymentTask = cancelable;
            }

            intent = await intentTask;

            if (intent == null)
            {
                return PaymentResponse.AsError(intentTask.Exception);
            }

            LogStatusStrings(Instance, intent);

            var paymentResult = await Instance.ConfirmPaymentIntentAsync(intent);
            if (paymentResult == null)
            {
                return PaymentResponse.AsError("Failed to process payment");
            }

            LogStatusStrings(Instance, paymentResult?.PaymentIntent ?? intent);

            if (!string.IsNullOrEmpty(paymentResult.Error?.RequestError?.LocalizedDescription))
            {
                return PaymentResponse.AsError(paymentResult.Error.RequestError.LocalizedDescription);
            }

            /*
            var paymentResult = await Instance.ProcessPaymentAsync(intent);
            if (paymentResult == null)
            {
                return PaymentResponse.AsError("Failed to process payment");
            }

            LogStatusStrings(Instance, paymentResult?.PaymentIntent ?? intent);

            if (!string.IsNullOrEmpty(paymentResult.Error?.RequestError?.LocalizedDescription))
            {
                return PaymentResponse.AsError(paymentResult.Error.RequestError.LocalizedDescription);
            }
            */

            ////Necessary if payment is successful?
            //if (intent.Status == SCPPaymentIntentStatus.Succeeded)
            //{

            //}            

            return new PaymentResponse
            {
                Id = paymentResult.PaymentIntent.StripeId
            };
        }
        catch (NSErrorException ex) when (ex.Error.LocalizedDescription.ToUpper().Contains("CANCELED"))
        {
            return new PaymentResponse();
        }
        catch (Exception ex)
        {
            return PaymentResponse.AsError(ex.Message);
        }
    }

    private void LogStatusStrings(SCPTerminal terminal, SCPPaymentIntent intent)
    {
        var intentStatus = SCPTerminal.StringFromPaymentIntentStatus(intent.Status); //requires_payment_method, requires_confirmation, succeeded
        var paymentStatus = SCPTerminal.StringFromPaymentStatus(terminal.PaymentStatus); //always Ready (doesn't change)
        var captureMethod = SCPTerminal.StringFromCaptureMethod(intent.CaptureMethod); //always Automatic (doesn't change)

        Trace($"{nameof(intentStatus)}: {intent.Status} : {intentStatus}");
        Trace($"{nameof(paymentStatus)}: {terminal.PaymentStatus} : {paymentStatus}");
        Trace($"{nameof(captureMethod)}: {intent.CaptureMethod} : {captureMethod}");
    }

    #endregion

    #region Permissions

    public async Task<bool> RequestPermissions()
    {
        var result = CheckBluetoothPermission();
        result &= CheckLocationPermission();
        return result;
    }

    private bool CheckBluetoothPermission()
    {
        return CoreBluetooth.CBManager.Authorization == CoreBluetooth.CBManagerAuthorization.AllowedAlways;
    }

    private bool CheckLocationPermission()
    {
        if (!CoreLocation.CLLocationManager.LocationServicesEnabled)
        {
            return false;
        }

        var status = CoreLocation.CLLocationManager.Status;

        return status switch
        {
            CoreLocation.CLAuthorizationStatus.AuthorizedAlways => true,
            CoreLocation.CLAuthorizationStatus.AuthorizedWhenInUse => true,
            _ => false
        };
    }

    #endregion

    #region Update

    public async Task SetSimulatedUpdate(bool updateRequired)
    {
        await Initialize();

        if (Instance.SimulatorConfiguration == null)
        {
            return;
        }

        Instance.SimulatorConfiguration.AvailableReaderUpdate = updateRequired
            ? SCPSimulateReaderUpdate.Available
            : SCPSimulateReaderUpdate.None;
    }

    public async Task UpdateConnectedReader()
    {
        //BUG: In case loading dialog was showing and just closed
        //https://github.com/CommunityToolkit/Maui/issues/1213#issuecomment-1580300733
        //https://github.com/CommunityToolkit/Maui/issues/1111
        //https://github.com/CommunityToolkit/Maui/pull/1223
        await Task.Delay(500);

        Instance.InstallAvailableUpdate();
    }

    #endregion

    #region ISCPTerminalDelegate

    private async void OnUnexpectedDisconnect(object sender, ReaderUnexpectedDisconnectEventArgs e)
    {
        try
        {
            await ReconnectToDisconnectedInternetReader(e.Reader);
        }
        catch (TaskCanceledException)
        {
            internetReaderCancellationToken = null;

            Trace($"Reconnection cancelled");
        }
    }

    #endregion

    #region IDiscoveryListener

    public override void DidUpdateDiscoveredReaders(SCPTerminal terminal, SCPReader[] readers)
    {
        OnDiscoveredReaders(readers?.Length, readers, terminal.ConnectedReader);
    }

    #endregion

    #region Events

    private void OnReaderUpdateAvailable(object sender, Bluetooth.ReaderSoftwareUpdateEventArgs e)
    {
        var selectedReader = lastRetrievedReaders.FirstOrDefault(x => x.SerialNumber == e.Serial);
        if (selectedReader != null && sender is SCPReader reader)
        {
            lastRetrievedReaders.Remove(selectedReader);
            lastRetrievedReaders.Add(reader);
            RefreshReader?.Invoke(null, new RefreshReaderEventArgs(reader.FromNative()));
        }
    }

    #endregion
}