namespace StripeTerminal;

public interface IStripeTerminalLogger
{
    void Trace(string message);
    void Info(string message, Dictionary<string, object> parameters = null);
    void Exception(string message, Exception exception);
}

public static class StripeTerminalLoggerConfiguration
{
    public static string NullValueReplacement { get; set; } = "NULL";
}