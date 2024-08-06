namespace StripeTerminal.Offline;

public class OfflineConnector : SCPOfflineDelegate
{
    public override void DidChangeOfflineStatus(SCPTerminal terminal, SCPOfflineStatus offlineStatus)
    {

    }

    public override void DidForwardPaymentIntent(SCPTerminal terminal, SCPPaymentIntent intent, NSError error)
    {

    }

    public override void DidReportForwardingError(SCPTerminal terminal, NSError error)
    {

    }
}