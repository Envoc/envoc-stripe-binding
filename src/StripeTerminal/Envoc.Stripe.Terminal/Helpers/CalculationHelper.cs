namespace StripeTerminal.Helpers;

public static class CalculationHelper
{
    public static decimal GetPercentageOf(decimal value, decimal percentage)
    {
        return (value / 100m) * percentage;
    }

    //Amounts have to be whole numbers
    //https://stripe.com/docs/currencies#zero-decimal
    public static nuint GetAmount(decimal amount)
    {
        if (amount <= 0)
        {
            return 0;
        }

        var paddedAmount = amount * 100;
        var round = decimal.Round(paddedAmount, MidpointRounding.AwayFromZero);
        var wholeAmount = Convert.ToUInt32(round);
        return wholeAmount;
    }
}