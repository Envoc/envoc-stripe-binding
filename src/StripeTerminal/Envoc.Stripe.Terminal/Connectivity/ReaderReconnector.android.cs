using Com.Stripe.Stripeterminal.External.Callable;

namespace StripeTerminal.Connectivity;

public class ReaderReconnector : Java.Lang.Object, IReaderReconnectionListener
{
    public event EventHandler<ConnectionStatusEventArgs> ConnectionStatusChangedEvent;

    public void OnReaderReconnectFailed(Com.Stripe.Stripeterminal.External.Models.Reader reader)
    {
        ConnectionStatusChangedEvent?.Invoke(this, new ConnectionStatusEventArgs(ReaderConnectivityStatus.Disconnected, ConnectionType.Bluetooth));
    }

    public void OnReaderReconnectStarted(Com.Stripe.Stripeterminal.External.Models.Reader reader, ICancelable cancelable)
    {
        var timer = new System.Timers.Timer(StripeConstants.ReconnectInterval)
        {
            AutoReset = false
        };

        timer.Elapsed += (s, a) =>
        {
            if (cancelable.IsCompleted)
            {
                return;
            }

            cancelable.Cancel(new GenericCallback(error =>
            {
                return;
            }));
        };

        timer.Start();

        ConnectionStatusChangedEvent?.Invoke(this, new ConnectionStatusEventArgs(ReaderConnectivityStatus.Connecting, ConnectionType.Bluetooth));
    }

    public void OnReaderReconnectSucceeded(Com.Stripe.Stripeterminal.External.Models.Reader reader)
    {
        ConnectionStatusChangedEvent?.Invoke(this, new ConnectionStatusEventArgs(ReaderConnectivityStatus.Bluetooth, ConnectionType.Bluetooth));
    }
}