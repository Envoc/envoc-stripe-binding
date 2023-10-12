namespace StripeTerminal;

public interface IStripeReaderCache
{
    Task<(Reader, DiscoveryType?)> GetLastConnectedReader();
    Task SetLastConnectedReader(Reader reader, DiscoveryType? discoveryType);
    Task<bool> HasLastConnectedReader();
}

public class MemoryReaderCache : IStripeReaderCache
{
    private Reader lastConnectedReader;
    private DiscoveryType? lastDiscoveryType;

    public Task<bool> HasLastConnectedReader()
    {
        return Task.FromResult(lastConnectedReader != null && lastDiscoveryType != null);
    }

    public Task<(Reader, DiscoveryType?)> GetLastConnectedReader()
    {
        return Task.FromResult((lastConnectedReader, lastDiscoveryType));
    }

    public Task SetLastConnectedReader(Reader reader, DiscoveryType? discoveryType)
    {
        lastConnectedReader = reader;
        lastDiscoveryType = discoveryType;
        return Task.CompletedTask;
    }
}