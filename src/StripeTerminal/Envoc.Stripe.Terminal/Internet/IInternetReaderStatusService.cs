namespace StripeTerminal.Internet;

public interface IInternetReaderStatusService
{
    Task<ReaderNetworkStatus> GetReaderStatus(Reader reader, CancellationToken cancellationToken = default);
}