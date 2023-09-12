namespace StripeTerminal;

public static class StripeTerminalConfiguration
{
    public static bool DeclineTransaction { get; set; } = false;
    public static bool UseSimulatedReaders { get; set;} = false;

    /// <summary>
    /// Allows Unit Testing or the library to run on not implemented platforms without crashing
    /// </summary>
    public static bool AllowNotImplementedMethodsToSucceed { get; set; } = false;

    public static string LoggerTracePrefix { get; set; } = "[STRIPE TERMINAL] ";
}