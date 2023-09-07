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
    private IConnectionTokenProviderService connectionTokenProviderService { get; set; }
    private Action<IList<Reader>> onReadersDiscoveredAction;
    private List<Reader> discoveredReaders = new List<Reader>();
    private StripeDiscoveryConfiguration discoveryConfiguration;
    private Reader currentReader;
    private static DiscoveryType? connectionType = null;

    public event EventHandler<RefreshReaderEventArgs> RefreshReader;
    public event EventHandler<ReaderUpdateEventArgs> ReaderUpdateProgress;
    public event EventHandler<ConnectionStatusEventArgs> ConnectionStatusChanged;

    public TerminalService(IStripeTerminalLogger logger, IConnectionTokenProviderService connectionTokenProviderService)
    {
        this.logger = logger;
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

    public async Task<Reader> ReconnectReader()
    {
        // determine discovery method
        var discoveryMethod = connectionType.GetValueOrDefault();

        // location must be null for bluetooth
        var locationId = discoveryMethod is DiscoveryType.Bluetooth
            ? null
            : currentReader.LocationId;

        Reader reader = null;
        // discover readers
        await GetReaders(new StripeDiscoveryConfiguration
        {
            DiscoveryMethod = discoveryMethod, 
            IsSimulated = currentReader.Simulated,
            LocationId = locationId
        }, 
        async readers =>
        {
            if (readers == null)
            {
                return;
            }

            // find reader
            reader = readers.FirstOrDefault(x => x.SerialNumber == currentReader.SerialNumber);
            if (reader == null)
            {
                return;
            }

            // connect reader
            reader = await ConnectReader(new ReaderConnectionRequest
            {
                Reader = reader,
                CurrentStripeLocationId = reader.LocationId
            });
        });

        return reader;
    }

    #region Events

    private void OnReaderUpdateProgress(object sender, ReaderUpdateEventArgs e)
    {
        ReaderUpdateProgress?.Invoke(sender, e);
    }

    private void OnConnectionStatusChanged(object sender, ConnectionStatusEventArgs e)
    {
        ConnectionStatusChanged?.Invoke(null, new ConnectionStatusEventArgs(e.Status, GetConnectionType()));
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
        logger?.Trace(message);
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

    private void OnDiscoveredReaders(int? count, IList<NativeReader> readers)
    {
        Trace($"Discovered {readers?.Count ?? -1} readers");

        if (count > 0)
        {
            var stripeReaders = readers.Select(x => x.FromNative()).ToList();

            //Replace instead of add?
            lastRetrievedReaders.AddRange(readers);

            // Report Back
            Trace($"Invoking callback with {stripeReaders.Count} readers");
            onReadersDiscoveredAction?.Invoke(stripeReaders);

            discoveryTask = null;
        }
        else
        {
            //Clear lastRetrievedReaders?
        }
    }

#endif

    #endregion
}
