namespace StripeTerminal.Payment;

public class PaymentResponse
{
    public string Id { get; set; }
    public string Error { get; set; }
    public bool HasError => !string.IsNullOrEmpty(Error);

    public static PaymentResponse AsError(Exception exception)
    {
        return new PaymentResponse
        {
            Error = exception.Message
        };
    }
    public static PaymentResponse AsError(string errorMessage)
    {
        return new PaymentResponse
        {
            Error = errorMessage
        };
    }
}