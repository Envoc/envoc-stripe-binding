namespace StripeTerminal;

public class TerminalDelegate : SCPTerminalDelegate
{
    private readonly IStripeTerminalLogger logger;

    public TerminalDelegate(IStripeTerminalLogger logger)
    {
        this.logger = logger;
    }

    public event EventHandler<ReaderUnexpectedDisconnectEventArgs> UnexpectedDisconnect;

    public override void DidChangeConnectionStatus(SCPTerminal terminal, SCPConnectionStatus status)
    {
        logger?.Trace($"{StripeTerminalConfiguration.LoggerTracePrefix}Connection Status Changed to: {status}");
    }

    public override void DidChangePaymentStatus(SCPTerminal terminal, SCPPaymentStatus status)
    {
        logger?.Trace($"{StripeTerminalConfiguration.LoggerTracePrefix}Payment Status Changed to: {status}");
    }

    public async override void DidReportUnexpectedReaderDisconnect(SCPTerminal terminal, SCPReader reader)
    {   
        logger?.Trace($"{StripeTerminalConfiguration.LoggerTracePrefix}Unexpected Disconnected");

        //HACK: Event fires before the SDK changes the status to Disconnected. This causes problems in later status checks.
        // Give the SDK time to catch up before processing
        await Task.Delay(50);

        UnexpectedDisconnect?.Invoke(terminal, new(reader.FromNative()));
    }
}