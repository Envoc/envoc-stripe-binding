namespace StripeTerminal;

public class TerminalDelegate : SCPTerminalDelegate
{
    public override void DidChangeConnectionStatus(SCPTerminal terminal, SCPConnectionStatus status)
    {
        Console.Write("Connection Status Changed");
    }

    public override void DidReportUnexpectedReaderDisconnect(SCPTerminal terminal, SCPReader reader)
    {
        Console.Write("Unexpected Disconnected");
    }
}