namespace StripeTerminal;

public static partial class Extensions
{
    public static bool IsUpdateAvailable(this Reader reader)
    {
        return reader?.AvailableUpdate != null;
    }

    public static bool IsUpdateRequired(this Reader reader)
    {
        if (!reader.IsUpdateAvailable())
        {
            return false;
        }

        return DateTime.UtcNow > reader.AvailableUpdate.RequiredAt;
    }

    public static string GetUpdateEstimate(this Reader reader)
    {
        if (!reader.IsUpdateAvailable())
        {
            return string.Empty;
        }

        return reader.AvailableUpdate.GetFriendlyUpdateTimeEstimate();
    }
}