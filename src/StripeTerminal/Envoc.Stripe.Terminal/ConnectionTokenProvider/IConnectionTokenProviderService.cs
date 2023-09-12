namespace StripeTerminal;

public interface IConnectionTokenProviderService
{
    Task<string> FetchConnectionToken();
}