using StripeTerminal.Bluetooth;
using StripeTerminal.Connectivity;
using StripeTerminal.Payment;

namespace StripeTerminal;

public partial class TerminalService : SCPDiscoveryDelegate
{
    private readonly SCPTerminalDelegate terminalDelegate; 

    private SCPCancelable paymentTask = null;
    private SCPCancelable discoveryTask = null;
    private List<SCPReader> lastRetrievedReaders = new();
    private BluetoothConnector bluetoothConnector = null;
    private ReaderReconnector readerReconnector = null;

    public TerminalService(
        IStripeTerminalLogger logger,
        IConnectionTokenProviderService connectionTokenProviderService,
        SCPTerminalDelegate terminalDelegate,
        BluetoothConnector bluetoothConnector,
        ReaderReconnector readerReconnector) : this(logger, connectionTokenProviderService)
    {
        this.terminalDelegate = terminalDelegate; 
        this.bluetoothConnector = bluetoothConnector ?? new BluetoothConnector(logger);
        this.readerReconnector = readerReconnector ?? new ReaderReconnector();

        this.bluetoothConnector.ReaderUpdateAvailable += OnReaderUpdateAvailable;
        this.bluetoothConnector.ReaderUpdateProgress += OnReaderUpdateProgress;
        this.bluetoothConnector.ReaderUpdateLabel += OnReaderUpdateLabel;
        this.readerReconnector.ConnectionStatusChangedEvent += OnConnectionStatusChanged;
    }

    protected SCPTerminal Instance => SCPTerminal.Shared;

    public bool IsTerminalConnected => IsTerminalInitialized && Instance.ConnectionStatus == SCPConnectionStatus.Connected;

    public bool IsTerminalInitialized => SCPTerminal.HasTokenProvider;

    public ReaderConnectivityStatus GetConnectivityStatus()
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
            if (connectionType == null)
            {
                return ReaderConnectivityStatus.Unknown;
            }

            return connectionType switch
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

        return Task.FromResult(true);
    }

    private void Error(string message, NSError error)
    {
        logger?.Info(message, error);
    }

    #region Discovery

    private Task DiscoverReaders(StripeDiscoveryConfiguration config)
    {
        var discoveryMethod = config.DiscoveryMethod switch
        {
            DiscoveryType.Bluetooth => SCPDiscoveryMethod.BluetoothScan,
            DiscoveryType.Internet => SCPDiscoveryMethod.Internet,
            DiscoveryType.Local => SCPDiscoveryMethod.LocalMobile,
            _ => throw new NotImplementedException(),
        };

        var configuration = new SCPDiscoveryConfiguration(discoveryMethod, config.IsSimulated)
        {
            Timeout = (nuint)(config.TimeOut ?? 15),
        };

        if (discoveryTask != null && !discoveryTask.Completed)
        {
            discoveryTask.Cancel(error =>
            {
                if (error != null)
                {
                    Error("reader_discover_cancel_error", error);
                }
            });
        }

        if (Instance.ConnectionStatus != SCPConnectionStatus.NotConnected && config.DiscoveryMethod == connectionType)
        {
            DidUpdateDiscoveredReaders(Instance, lastRetrievedReaders.ToArray());

            return Task.CompletedTask;
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
                Error("reader_discover_error", error);
            }
        });

        return Task.CompletedTask;
    }

    public async Task CancelDiscovery()
    {
        await Initialize();

        if (discoveryTask != null)
        {
            if (!discoveryTask.Completed)
            {
                discoveryTask.Cancel(error =>
                {
                    if (error != null)
                    {
                        Error("reader_discover_cancel_error", error);
                    }
                });
            }
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

        if (discoveryConfiguration.DiscoveryMethod == DiscoveryType.Bluetooth)
        {
            var connectionConfig = new SCPBluetoothConnectionConfiguration(
              // When connecting to a physical reader, your integration should specify either the
              // same location as the last connection (selectedReader.locationId) or a new location
              // of your user's choosing.
              //
              locationId: locationId,
              autoReconnectOnUnexpectedDisconnect: true,
              readerReconnector
            );

            Instance.ConnectBluetoothReader(selectedReader, @delegate: bluetoothConnector, connectionConfig: connectionConfig, (connectedReader, error) =>
            {
                if (error != null)
                {
                    Error("reader_bluetooth_connection_error", error);
                }

                if (connectedReader != null)
                {
                    currentReader = request.Reader;
                    connectionType = discoveryConfiguration.DiscoveryMethod;
                    tcs.SetResult(connectedReader.FromNative());
                }
                else
                {
                    tcs.SetResult(null);
                }
            });
        }
        else if (discoveryConfiguration.DiscoveryMethod == DiscoveryType.Internet)
        {
            try
            {
                Instance.ConnectInternetReader(selectedReader, connectionConfig: null, (connectedReader, error) =>
                {
                    if (error != null)
                    {
                        Error("reader_internet_connection_error", error);
                    }

                    if (connectedReader != null)
                    {
                        currentReader = request.Reader;
                        connectionType = discoveryConfiguration.DiscoveryMethod;
                        tcs.SetResult(connectedReader.FromNative());
                    }
                    else
                    {
                        tcs.SetResult(null);
                    }
                });
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
        await Initialize();

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

        paymentTask.Cancel(error =>
        {
            if (error == null)
            {
                return;
            }

            Error("reader_payment_cancel_error", error);
        });
    }

    public async Task<PaymentResponse> CollectPayment(PaymentRequest payment)
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

        try
        {
            var parameters =
                new SCPPaymentIntentParameters(amount, "usd", new[] { "card_present" }, SCPCaptureMethod.Automatic)
                {
                    Metadata = payment.Metadata.ToNSDictionary()
                };

            var tcs = new TaskCompletionSource<SCPPaymentIntent>();

            var intentTask = Instance.CreatePaymentIntentAsync(parameters);
            var intent = await intentTask;

            if (intent == null)
            {
                return PaymentResponse.AsError(intentTask.Exception);
            }

            LogStatusStrings(Instance, intent);

            intentTask = Instance.CollectPaymentMethodAsync(intent, out SCPCancelable cancelable);
            paymentTask = cancelable;

            intent = await intentTask;

            if (intent == null)
            {
                return PaymentResponse.AsError(intentTask.Exception);
            }

            LogStatusStrings(Instance, intent);

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
        //TODO: Implement

        return true;
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