namespace StripeTerminal.Connectivity;

public class ReaderReconnector : SCPReconnectionDelegate
{
    public event EventHandler<ConnectionStatusEventArgs> ConnectionStatusChangedEvent;

    public override void Terminal(SCPTerminal terminal, SCPCancelable cancelable)
    {
        var timer = new System.Timers.Timer(StripeConstants.ReconnectInterval)
        {
            AutoReset = false
        };

        timer.Elapsed += (s, a) =>
        {
            if (cancelable.Completed)
            {
                return;
            }

            cancelable.Cancel(error =>
            {
                return;
            });
        };

        timer.Start();

        ConnectionStatusChangedEvent?.Invoke(this, new ConnectionStatusEventArgs(ReaderConnectivityStatus.Connecting, ConnectionType.Bluetooth));
    }

    public override void TerminalDidFailReaderReconnect(SCPTerminal terminal)
    {
        ConnectionStatusChangedEvent?.Invoke(this, new ConnectionStatusEventArgs(ReaderConnectivityStatus.Disconnected, ConnectionType.Bluetooth));
    }

    public override void TerminalDidSucceedReaderReconnect(SCPTerminal terminal)
    {
        ConnectionStatusChangedEvent?.Invoke(this, new ConnectionStatusEventArgs(ReaderConnectivityStatus.Bluetooth, ConnectionType.Bluetooth));
    }
}