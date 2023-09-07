using Com.Stripe.Stripeterminal.External.Callable;
using Com.Stripe.Stripeterminal.External.Models;
using Models = Com.Stripe.Stripeterminal.External.Models;

namespace StripeTerminal;

public class ConnectionCallback : Java.Lang.Object, IReaderCallback
{
    private readonly Action<Models.Reader, TerminalException> _callback;

    public ConnectionCallback(Action<Models.Reader, TerminalException> callback)
    {
        _callback = callback;
    }

    public void OnSuccess(Models.Reader reader)
    {
        _callback(reader, null);
    }

    public void OnFailure(TerminalException e)
    {
        _callback(null, e);
    }
}

public class PaymentIntentCallback : Java.Lang.Object, IPaymentIntentCallback
{
    private readonly Action<PaymentIntent, TerminalException> _callback;

    public PaymentIntentCallback(Action<PaymentIntent, TerminalException> callback)
    {
        _callback = callback;
    }

    public void OnSuccess(PaymentIntent paymentIntent)
    {
        _callback(paymentIntent, null);
    }

    public void OnFailure(TerminalException e)
    {
        _callback(null, e);
    }
}

public class ReaderSoftwareUpdateCallback : Java.Lang.Object, IReaderSoftwareUpdateCallback
{
    private readonly Action<Models.ReaderSoftwareUpdate, TerminalException> _callback;

    public ReaderSoftwareUpdateCallback(Action<Models.ReaderSoftwareUpdate, TerminalException> callback)
    {
        _callback = callback;
    }

    public void OnSuccess(Models.ReaderSoftwareUpdate readerUpdate)
    {
        _callback(readerUpdate, null);
    }

    public void OnFailure(TerminalException e)
    {
        _callback(null, e);
    }
}

public class ReaderSoftwareUpdateListener : Java.Lang.Object//, IReaderSoftwareUpdateListener
{
    private readonly Action<float> _callback;

    public ReaderSoftwareUpdateListener(Action<float> callback)
    {
        _callback = callback;
    }

    void OnReportReaderSoftwareUpdateProgress(float p0)
    {
        _callback(p0);
    }
}

public class GenericCallback : Java.Lang.Object, ICallback
{
    private readonly Action<TerminalException> _callback;

    public GenericCallback(Action<TerminalException> callback)
    {
        _callback = callback;
    }

    public void OnSuccess()
    {
        _callback(null);
    }

    public void OnFailure(TerminalException e)
    {
        _callback(e);
    }
}
