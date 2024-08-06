namespace StripeTerminal.Connectivity;

public class ReaderReconnector : SCPReconnectionDelegate
{
    public event EventHandler<ConnectionStatusEventArgs> ConnectionStatusChangedEvent;

    public override void ReaderDidStartReconnect(SCPReader reader, SCPCancelable cancelable)
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

    public override void ReaderDidFailReconnect(SCPReader reader)
    {
        ConnectionStatusChangedEvent?.Invoke(this, new ConnectionStatusEventArgs(ReaderConnectivityStatus.Disconnected, ConnectionType.Bluetooth));
    }

    public override void ReaderDidSucceedReconnect(SCPReader reader)
    {
        ConnectionStatusChangedEvent?.Invoke(this, new ConnectionStatusEventArgs(ReaderConnectivityStatus.Bluetooth, ConnectionType.Bluetooth));
    }
}