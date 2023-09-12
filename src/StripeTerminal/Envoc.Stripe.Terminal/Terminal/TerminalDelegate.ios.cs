namespace StripeTerminal;

public class TerminalDelegate : SCPTerminalDelegate
{
    private readonly IStripeTerminalLogger logger;

    public TerminalDelegate(IStripeTerminalLogger logger)
    {
        this.logger = logger;
    }

    public override void DidChangeConnectionStatus(SCPTerminal terminal, SCPConnectionStatus status)
    {
        logger?.Trace($"{StripeTerminalConfiguration.LoggerTracePrefix}Connection Status Changed to: {status}");
    }

    public override void DidReportUnexpectedReaderDisconnect(SCPTerminal terminal, SCPReader reader)
    {
        logger?.Trace($"{StripeTerminalConfiguration.LoggerTracePrefix}Unexpected Disconnected");
    }
}