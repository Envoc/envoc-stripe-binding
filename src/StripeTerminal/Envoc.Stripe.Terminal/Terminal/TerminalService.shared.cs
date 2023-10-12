#if IOS || ANDROID
#define SUPPORTED_PLATFORM
#endif

namespace StripeTerminal;

#if ANDROID
using NativeReader = Com.Stripe.Stripeterminal.External.Models.Reader;
#elif IOS
using NativeReader = SCPReader;
#endif

public partial class TerminalService : ITerminalService
{
    private IStripeTerminalLogger logger;
    private IStripeReaderCache readerCache;
    private Action<IList<Reader>> onReadersDiscoveredAction;
    private List<Reader> discoveredReaders = new List<Reader>();
    private StripeDiscoveryConfiguration discoveryConfiguration;
    private Reader currentReader;
    private static DiscoveryType? connectionType = null;
    private IConnectionTokenProviderService connectionTokenProviderService { get; set; }

    public event EventHandler<RefreshReaderEventArgs> RefreshReader;
    public event EventHandler<ReaderUpdateEventArgs> ReaderUpdateProgress;
    public event EventHandler<ReaderUpdateLabelEventArgs> ReaderUpdateLabel;
    public event EventHandler<ConnectionStatusEventArgs> ConnectionStatusChanged;

    public TerminalService(IStripeTerminalLogger logger, IConnectionTokenProviderService connectionTokenProviderService, IStripeReaderCache readerCache)
    {
        this.logger = logger;
        this.readerCache = readerCache;
        this.connectionTokenProviderService = connectionTokenProviderService;
    }

    public Task GetBluetoothReaders(Action<IList<Reader>> readers, bool simulated = false)
    {
        discoveryConfiguration = new StripeDiscoveryConfiguration
        {
            TimeOut = 15, 
            DiscoveryMethod = DiscoveryType.Bluetooth, 
            IsSimulated= simulated
        };

        return GetReaders(discoveryConfiguration, readers);
    }

    public Task GetWifiReaders(Action<IList<Reader>> readers, bool simulated = false)
    {
        discoveryConfiguration = new StripeDiscoveryConfiguration
        {
            TimeOut = 15,
            DiscoveryMethod = DiscoveryType.Internet,
            IsSimulated = simulated
        };

        return GetReaders(discoveryConfiguration, readers);
    }

    public async Task GetReaders(StripeDiscoveryConfiguration config, Action<IList<Reader>> readers)
    {
        await Initialize();

        onReadersDiscoveredAction = readers;

        await DiscoverReaders(config);
    }

    public async Task<Reader> ReconnectReader(Reader previousReader, DiscoveryType? discoveryType, bool waitForResponse = false)
    {
        if (previousReader == null)
        {
            return null;
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

            // connect reader
            reader = await ConnectReader(new ReaderConnectionRequest
            {
                Reader = reader,
                CurrentStripeLocationId = reader.LocationId
            });

            ConnectionStatusChanged?.Invoke(null, new ConnectionStatusEventArgs(GetConnectivityStatus(discoveryMethod), connectionType));

            tcs.TrySetResult(reader);
        });

        if (!waitForResponse)
        {
            tcs.TrySetResult(reader);
        }

        return await tcs.Task;
    }

    public Task<Reader> ReconnectReader() => ReconnectReader(currentReader, connectionType, false);

    public virtual async Task<Reader> ReconnectToCachedReader()
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

        return await ReconnectReader(lastReader, discoveryType, true);
    }

    public Connectivity.ReaderConnectivityStatus GetConnectivityStatus() => GetConnectivityStatus(connectionType);

    #region Events

    private void OnReaderUpdateProgress(object sender, ReaderUpdateEventArgs e)
    {
        ReaderUpdateProgress?.Invoke(sender, e);
    }

    private void OnReaderUpdateLabel(object sender, ReaderUpdateLabelEventArgs e)
    {
        ReaderUpdateLabel?.Invoke(sender, e);
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
            _ => ConnectionType.Internet
        };
    }

    private ConnectionType GetConnectionType()
    {
        if (connectionType is DiscoveryType.Bluetooth)// or DiscoveryMethod.BluetoothProximity)
        {
            return ConnectionType.Bluetooth;
        }

        return ConnectionType.Internet;
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
    }

#endif

    #endregion
}
