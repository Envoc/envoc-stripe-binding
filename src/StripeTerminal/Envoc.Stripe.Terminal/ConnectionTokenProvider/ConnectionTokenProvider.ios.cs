namespace StripeTerminal;

public class ConnectionTokenProvider : SCPConnectionTokenProvider
{
    private readonly IConnectionTokenProviderService connectionTokenProviderService;

    private int _retries = 1;
    private const int MaxRetries = 3;

    public ConnectionTokenProvider(IConnectionTokenProviderService connectionTokenProviderService)
    {
        this.connectionTokenProviderService = connectionTokenProviderService;
    }

    public override async void FetchConnectionToken(SCPConnectionTokenCompletionBlock completion)
    {
        var isSuccess = false;

        do
        {
            try
            {
                var token = await connectionTokenProviderService.FetchConnectionToken();
                isSuccess = true;
                completion(token, null);
            }
            catch (Exception ex)
            {
                if (_retries >= MaxRetries)
                {
                    completion(null, new NSError(new NSString(ex.Message), 0));
                }
            }

            _retries++;

        } while (_retries < MaxRetries && isSuccess == false);
    }
}