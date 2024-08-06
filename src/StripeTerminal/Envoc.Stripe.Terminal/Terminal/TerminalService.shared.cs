namespace StripeTerminal;

using StripeTerminal.Internet;

#if ANDROID
using NativeReader = Com.Stripe.Stripeterminal.External.Models.Reader;
#elif IOS
using NativeReader = SCPReader;
#endif

public partial class TerminalService : ITerminalService
{
    private IStripeTerminalLogger logger;
    private IStripeReaderCache readerCache;
    private readonly IInternetReaderStatusService internetReaderStatusService;
    private Action<IList<Reader>> onReadersDiscoveredAction;
    private List<Reader> discoveredReaders = new List<Reader>();
    private StripeDiscoveryConfiguration discoveryConfiguration;
    private Reader currentReader;
    private static DiscoveryType? connectionType = null;
    private static readonly object _locker = new ();
    private CancellationTokenSource internetReaderCancellationToken = null;
    private IConnectionTokenProviderService connectionTokenProviderService { get; set; }

    public event EventHandler<RefreshReaderEventArgs> RefreshReader;
    public event EventHandler<ReaderUpdateEventArgs> ReaderUpdateProgress;
    public event EventHandler<ReaderUpdateLabelEventArgs> ReaderUpdateLabel;
    public event EventHandler<ConnectionStatusEventArgs> ConnectionStatusChanged;
    public event EventHandler<ReaderUpdateLabelEventArgs> ReaderErrorMessage;
    public event EventHandler<ReaderBatteryUpdateEventArgs> ReaderBatteryUpdate;

    public TerminalService(IStripeTerminalLogger logger, IConnectionTokenProviderService connectionTokenProviderService, IStripeReaderCache readerCache, IInternetReaderStatusService internetReaderStatusService)
    {
        this.logger = logger;
        this.readerCache = readerCache;
        this.connectionTokenProviderService = connectionTokenProviderService;
        this.internetReaderStatusService = internetReaderStatusService;
    }

    public Reader GetConnectedReader() => currentReader;

    public async Task GetBluetoothReaders(Action<IList<Reader>> readers, bool simulated = false)
    {
        await CancelPendingInternetReconnect();

        discoveryConfiguration = new StripeDiscoveryConfiguration
        {
            TimeOut = 15, 
            DiscoveryMethod = DiscoveryType.Bluetooth, 
            IsSimulated= simulated
        };

        await GetReaders(discoveryConfiguration, readers);
    }

    public async Task GetWifiReaders(Action<IList<Reader>> readers, string locationId = null, bool simulated = false)
    {
        await CancelPendingInternetReconnect();

        discoveryConfiguration = new StripeDiscoveryConfiguration
        {
            TimeOut = 15,
            DiscoveryMethod = DiscoveryType.Internet,
            IsSimulated = simulated,
            LocationId = locationId
        };

        await GetReaders(discoveryConfiguration, readers);
    }

    public async Task GetHandoffReaders(Action<IList<Reader>> readers, bool simulated = false)
    {
        await CancelPendingInternetReconnect();

        discoveryConfiguration = new StripeDiscoveryConfiguration
        {
            TimeOut = 15,
            DiscoveryMethod = DiscoveryType.Handoff,
            IsSimulated = simulated
        };

        await GetReaders(discoveryConfiguration, readers);
    }

    public async Task GetLocalReaders(Action<IList<Reader>> readers, bool simulated = false)
    {
        await CancelPendingInternetReconnect();

        discoveryConfiguration = new StripeDiscoveryConfiguration
        {
            TimeOut = 15,
            DiscoveryMethod = DiscoveryType.Local,
            IsSimulated = simulated
        };

        await GetReaders(discoveryConfiguration, readers);
    }

    public async Task GetReaders(StripeDiscoveryConfiguration config, Action<IList<Reader>> readers)
    {
        await Initialize();

        onReadersDiscoveredAction = readers;

        await DiscoverReaders(config);
    }

    public async Task<Reader> ReconnectReader(Reader previousReader, DiscoveryType? discoveryType, bool waitForResponse = false, CancellationToken cancellationToken = default)
    {
        if (previousReader == null)
        {
            return null;
        }

        if (IsTerminalConnected && currentReader != null)
        {
            return currentReader;
        }

        // determine discovery method
        var discoveryMethod = discoveryType.GetValueOrDefault();
        var connectionType = GetConnectionType(discoveryMethod);

        ConnectionStatusChanged?.Invoke(null, new ConnectionStatusEventArgs(Connectivity.ReaderConnectivityStatus.Connecting, connectionType));
        
        // location must be null for bluetooth
        var locationId = discoveryMethod is DiscoveryType.Bluetooth
            ? null
            : previousReader.LocationId;

        var tcs = new TaskCompletionSource<Reader>();

        Reader reader = null;
        // discover readers
        await GetReaders(discoveryConfiguration = new StripeDiscoveryConfiguration
        {
            DiscoveryMethod = discoveryMethod,
            IsSimulated = previousReader.Simulated,
            LocationId = locationId
        },
        async readers =>
        {
            if (readers == null)
            {
                ConnectionStatusChanged?.Invoke(null, new ConnectionStatusEventArgs(Connectivity.ReaderConnectivityStatus.Disconnected, connectionType));

                tcs.TrySetResult(null);
                return;
            }

            // find reader
            reader = readers.FirstOrDefault(x => x.SerialNumber == previousReader.SerialNumber);
            if (reader == null)
            {
                ConnectionStatusChanged?.Invoke(null, new ConnectionStatusEventArgs(Connectivity.ReaderConnectivityStatus.Disconnected, connectionType));

                tcs.TrySetResult(null);
                return;
            }
            try
            {
                // connect reader
                reader = await ConnectReader(new ReaderConnectionRequest
                {
                    Reader = reader,
                    CurrentStripeLocationId = reader.LocationId
                });

                ConnectionStatusChanged?.Invoke(null, new ConnectionStatusEventArgs(GetConnectivityStatus(discoveryMethod), connectionType));

                tcs.TrySetResult(reader);
            }
            catch (Exception ex)
            {
                Exception($"reader_reconnection_error", ex);
                tcs.TrySetResult(null);
                return;
            }
        });

        if (!waitForResponse)
        {
            tcs.TrySetResult(reader);
        }

        return await tcs.Task.WaitOrCancel(cancellationToken, async () => await CancelDiscovery());
    }

    public Task<Reader> ReconnectReader() => ReconnectReader(currentReader, connectionType, false);

    public virtual async Task<Reader> ReconnectToCachedReader(CancellationToken cancellationToken = default)
    {
        if (readerCache == null)
        {
            return null;
        }

        var (lastReader, discoveryType) = await readerCache.GetLastConnectedReader();

        if (lastReader == null || discoveryType == null)
        {
            return null;
        }

        var reader = await ReconnectReader(lastReader, discoveryType, true, cancellationToken);

        //if reconnect fails, remove the reader so next startup doesn't keep failing
        if (reader == null)
        {
            await readerCache.SetLastConnectedReader(null, null);
        }

        return reader;
    }

    public Connectivity.ReaderConnectivityStatus GetConnectivityStatus() => GetConnectivityStatus(connectionType);

    public async Task<bool> DisconnectReader()
    {
        await readerCache.SetLastConnectedReader(null, null);

        //There's no reader connected if the SDK isn't initialized
        if (!IsTerminalInitialized)
        {
            return true;
        }

        return await DisconnectReaderImplementation();
    }

    private async Task CancelPendingInternetReconnect()
    {
        if (internetReaderCancellationToken != null && !internetReaderCancellationToken.IsCancellationRequested)
        {
            internetReaderCancellationToken.Cancel();
            await Task.Delay(100);

            await CancelDiscovery();

            await Task.Delay(100);
        }
    }

    #region Events

    private void OnReaderErrorMessage(object sender, ReaderUpdateLabelEventArgs e)
    {
        ReaderErrorMessage?.Invoke(sender, e);
    }

    private void OnReaderUpdateProgress(object sender, ReaderUpdateEventArgs e)
    {
        ReaderUpdateProgress?.Invoke(sender, e);
    }

    private void OnReaderUpdateLabel(object sender, ReaderUpdateLabelEventArgs e)
    {
        ReaderUpdateLabel?.Invoke(sender, e);
    }

    private void OnReaderBatteryUpdate(object sender, ReaderBatteryUpdateEventArgs e)
    {
        ReaderBatteryUpdate?.Invoke(sender, e);
    }

    private void OnConnectionStatusChanged(object sender, ConnectionStatusEventArgs e)
    {
        ConnectionStatusChanged?.Invoke(null, new ConnectionStatusEventArgs(e.Status, GetConnectionType()));
    }

    private ConnectionType GetConnectionType(DiscoveryType discoveryType)
    {
        return discoveryType switch
        {
            DiscoveryType.Bluetooth => ConnectionType.Bluetooth,
            DiscoveryType.Local => ConnectionType.Local,
            DiscoveryType.Internet => ConnectionType.Internet,
            DiscoveryType.Handoff => ConnectionType.Handoff,
            _ => ConnectionType.Internet
        };
    }

    private ConnectionType GetConnectionType()
    {
        if (connectionType == null)
        {
            return ConnectionType.Internet;
        }

        return GetConnectionType(connectionType.Value);
    }

    #endregion

    #region Logging

    private void Trace(string message)
    {
        logger?.Trace(StripeTerminalConfiguration.LoggerTracePrefix + message);
    }

    private void Info(string message, Dictionary<string, object> parameters = null)
    {
        logger?.Info(message, parameters);
    }

    private void Exception(string message, Exception exception)
    {
        logger?.Exception(message, exception);
    }

    #endregion

    #region Platform

#if SUPPORTED_PLATFORM

    public async virtual Task ReconnectToDisconnectedInternetReader(Reader reader)
    {
        if (connectionType != DiscoveryType.Internet)
        {
            return;
        }

        if (internetReaderCancellationToken != null)
        {
            //this shouldn't happen
            //why would this get called again before it finished the first time?
            internetReaderCancellationToken.Cancel();
            await Task.Delay(250);
            internetReaderCancellationToken.Dispose();
        }

        internetReaderCancellationToken = new CancellationTokenSource();

        //immediately try a reconnect without polling the endpoint since the Stripe status is delayed
        var successfullyReconnectedReader = await ReconnectReader(reader, DiscoveryType.Internet, true, internetReaderCancellationToken.Token);
        if (successfullyReconnectedReader != null)
        {
            internetReaderCancellationToken = null;

            return;
        }

        //wait X seconds because reconnect failed
        await Task.Delay(TimeSpan.FromSeconds(30), internetReaderCancellationToken.Token);
        if (internetReaderCancellationToken.IsCancellationRequested)
        {
            internetReaderCancellationToken = null;

            return;
        }

        //poll the endpoint with progressive delay
        await AutoRetry(retryCount: 5, delayInSeconds: 60, async () =>
        {
            //should we pass a Timeout?
            var readerStatus = await internetReaderStatusService.GetReaderStatus(reader, internetReaderCancellationToken.Token);
            if (readerStatus != ReaderNetworkStatus.Online)
            {
                //if offline, break out and wait for the retry
                return false;
            }

            //Stripe reports it is online, try to reconnect
            var successfullyReconnectedReader = await ReconnectReader(reader, DiscoveryType.Internet, true, internetReaderCancellationToken.Token);
            return successfullyReconnectedReader != null;

        }, token: internetReaderCancellationToken.Token, progressiveDelay: true);

        if (internetReaderCancellationToken.IsCancellationRequested)
        {
            internetReaderCancellationToken = null;

            return;
        }

        internetReaderCancellationToken?.Dispose();
        internetReaderCancellationToken = null;

        async Task AutoRetry(int retryCount, int delayInSeconds, Func<Task<bool>> actionToSucceed, CancellationToken token = default, bool progressiveDelay = false)
        {
            int retries = 0;
            while (retries < retryCount && !token.IsCancellationRequested)
            {
                Trace("Retrying");

                try
                {
                    if (await actionToSucceed())
                    {
                        break;
                    }

                    retries++;

                    var delay = progressiveDelay ? retries * delayInSeconds : delayInSeconds;
                    Trace($"Attempt {retries} failed. Delaying {delay} seconds.");
                    await Task.Delay(TimeSpan.FromSeconds(delay), token);
                }
                catch (System.Threading.Tasks.TaskCanceledException)
                {
                    Trace($"Retry cancelled");
                    break;
                }
            }
        }
    }

    private void OnDiscoveredReaders(int? count, IList<NativeReader> readers, NativeReader connectedReader = null)
    {
        Trace($"Discovered {readers?.Count ?? -1} readers");
        
        if (count > 0)
        {
            var stripeReaders = readers.Select(x => x.FromNative(connectedReader)).ToList();

            //Replace instead of add?
            lastRetrievedReaders.Clear();
            lastRetrievedReaders.AddRange(readers);

            // Report Back
            Trace($"Invoking callback with {stripeReaders.Count} readers");
            onReadersDiscoveredAction?.Invoke(stripeReaders);

            discoveryTask = null;
        }
#if APPONDEVICE
        else
        {
            onReadersDiscoveredAction?.Invoke(new List<Reader>());
        }
#endif
    }

#endif

    #endregion
}
