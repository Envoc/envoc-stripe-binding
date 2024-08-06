using StripeTerminal.Connectivity;
using StripeTerminal.Payment;

namespace StripeTerminal;

public partial interface ITerminalService
{
    event EventHandler<RefreshReaderEventArgs> RefreshReader;
    event EventHandler<ReaderUpdateEventArgs> ReaderUpdateProgress;
    event EventHandler<ReaderUpdateLabelEventArgs> ReaderUpdateLabel;
    event EventHandler<ConnectionStatusEventArgs> ConnectionStatusChanged;
    event EventHandler<ReaderUpdateLabelEventArgs> ReaderErrorMessage;
    event EventHandler<ReaderBatteryUpdateEventArgs> ReaderBatteryUpdate;

    bool IsTerminalConnected { get; }
    bool IsTerminalInitialized { get; }
    ReaderConnectivityStatus GetConnectivityStatus();

    //Task Initialize(IConnectionTokenProviderService connectionTokenProviderService);
    Task<bool> Initialize();

    Reader GetConnectedReader();
    Task GetWifiReaders(Action<IList<Reader>> readers, string locationId = null, bool simulated = false);
    Task GetBluetoothReaders(Action<IList<Reader>> readers, bool simulated = false);
    Task GetHandoffReaders(Action<IList<Reader>> readers, bool simulated = false);
    Task GetLocalReaders(Action<IList<Reader>> readers, bool simulated = false);
    Task GetReaders(StripeDiscoveryConfiguration config, Action<IList<Reader>> readers);
    Task CancelDiscovery();

    Task<Reader> ConnectReader(ReaderConnectionRequest request);
    Task<bool> DisconnectReader();
    Task<Reader> ReconnectReader();
    Task<Reader> ReconnectReader(Reader previousReader, DiscoveryType? discoveryType, bool waitForResponse = false, CancellationToken cancellationToken = default);
    Task<Reader> ReconnectToCachedReader(CancellationToken cancellationToken = default);

    Task<bool> RequestPermissions();

    Task<PaymentResponse> CollectPayment(PaymentRequest payment, CancellationToken token = default);
    void CancelPayment();

    Task UpdateConnectedReader();
    Task SetSimulatedUpdate(bool updateRequired);
}