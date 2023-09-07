namespace StripeTerminal;

//https://stripe.com/docs/terminal/references/testing
public class StripeTestCards
{
    public const string VISA = "4242424242424242";
    public const string Success_OnlinePin = "4001000360000005";
    public const string Error_Declined = "4000000000000002";
    public const string Error_InsufficientFunds = "4000000000009995";
    public const string Error_StolenCard = "4000000000009979";
    public const string Error_ExpiredCard = "4000000000000069";
}

public class StripeTestAmount
{
    public const nuint Approved = 00;
    public const nuint Declined = 01;

    public static nuint GetApprovedAmount(nuint amount) => GetAmount(amount, Approved);

    public static nuint GetDeclinedAmount(nuint amount) => GetAmount(amount, Declined);

    private static nuint GetAmount(nuint amount, nuint offset)
    {
        amount /= 100;
        amount *= 100;
        amount += offset;
        return amount;
    }
}