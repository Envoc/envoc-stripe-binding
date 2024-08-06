using StripeTerminal.Connectivity;
using StripeTerminal.Payment;

namespace StripeTerminal;

public partial class TerminalService
{
    public bool IsTerminalConnected { get; private set; }

    public bool IsTerminalInitialized { get; private set; }

    private void NotImplementedCheck()
    {
        if (StripeTerminalConfiguration.AllowNotImplementedMethodsToSucceed)
        {
            Trace($"This method has not been implemented for this platform, but {nameof(StripeTerminalConfiguration.AllowNotImplementedMethodsToSucceed)} is set to true");
        }
        else
        {
            throw new NotImplementedException();
        }
    }

    public ReaderConnectivityStatus GetConnectivityStatus(DiscoveryType? discoveryType)
    {
        NotImplementedCheck();
        return ReaderConnectivityStatus.Unknown;
    }

    public Task<bool> Initialize()
    {
        NotImplementedCheck();
        return Task.FromResult(true);
    }

    #region Discovery

    private Task DiscoverReaders(StripeDiscoveryConfiguration config)
    {
        NotImplementedCheck();
        return Task.CompletedTask;
    }

    public Task CancelDiscovery()
    {
        NotImplementedCheck();
        return Task.CompletedTask;
    }

    #endregion

    #region Connection

    public Task<Reader> ConnectReader(ReaderConnectionRequest request)
    {
        NotImplementedCheck();
        return Task.FromResult<Reader>(null);
    }

    private Task<bool> DisconnectReaderImplementation()
    {
        NotImplementedCheck();
        return Task.FromResult(true);
    }

    #endregion

    #region Payment

    public void CancelPayment()
    {
        NotImplementedCheck();
    }

    public Task<PaymentResponse> CollectPayment(PaymentRequest payment, CancellationToken token = default)
    {
        NotImplementedCheck();
        return Task.FromResult<PaymentResponse>(null);
    }

    #endregion

    #region Permissions

    public Task<bool> RequestPermissions()
    {
        NotImplementedCheck();
        return Task.FromResult(true);
    }

    #endregion

    #region Update

    public Task SetSimulatedUpdate(bool updateRequired)
    {
        NotImplementedCheck();
        return Task.CompletedTask;
    }

    public Task UpdateConnectedReader()
    {
        NotImplementedCheck();
        return Task.CompletedTask;
    }

    #endregion
}