using Com.Stripe.Stripeterminal.External.Callable;
using Com.Stripe.Stripeterminal.External.Models;
using Java.Lang;

namespace StripeTerminal;

public class ConnectionTokenProvider : Java.Lang.Object, IConnectionTokenProvider
{
    private readonly IConnectionTokenProviderService connectionTokenProviderService;

    private int _retries = 1;
    private const int MaxRetries = 3;

    public ConnectionTokenProvider(IConnectionTokenProviderService connectionTokenProviderService)
    {
        this.connectionTokenProviderService = connectionTokenProviderService;
    }

    public async void FetchConnectionToken(IConnectionTokenCallback callback)
    {
        var isSuccess = false;

        do
        {
            try
            {
                var token = await connectionTokenProviderService.FetchConnectionToken();
                isSuccess = true;
                callback.OnSuccess(token);
            }
            catch (Throwable ex)
            {
                if (_retries >= MaxRetries)
                {
                    callback.OnFailure(new ConnectionTokenException("Failed to fetch connection token", ex));
                }
            }
            catch (System.Exception ex)
            {
                if (_retries >= MaxRetries)
                {
                    callback.OnFailure(new ConnectionTokenException("Failed to fetch connection token", new Throwable(ex.Message)));
                }
            }

            _retries++;

        } while (_retries < MaxRetries && isSuccess == false);
    }
}
