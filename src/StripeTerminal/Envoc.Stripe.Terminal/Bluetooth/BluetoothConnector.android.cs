using Com.Stripe.Stripeterminal.External.Callable;
using Models = Com.Stripe.Stripeterminal.External.Models;

namespace StripeTerminal.Bluetooth;

public class BluetoothConnector : Java.Lang.Object, IBluetoothReaderListener
{
    private readonly IStripeTerminalLogger logger;

    public BluetoothConnector(IStripeTerminalLogger logger)
    {
        this.logger = logger;
    }

    public event EventHandler<ReaderUpdateEventArgs> ReaderUpdateProgress;
    public event EventHandler<ReaderSoftwareUpdateEventArgs> ReaderUpdateAvailable;
    public event EventHandler<ReaderUpdateLabelEventArgs> ReaderUpdateLabel;

    public void OnBatteryLevelUpdate(float batteryLevel, Models.BatteryStatus status, bool isCharging)
    {
    }

    public void OnReportLowBatteryWarning()
    {
    }

    public void OnReportReaderSoftwareUpdateProgress(float progress)
    {
        ReaderUpdateProgress?.Invoke(null, new ReaderUpdateEventArgs(progress));
    }

    public void OnRequestReaderDisplayMessage(Models.ReaderDisplayMessage displayMessage)
    {
        var message = displayMessage.ToString();
        logger?.Trace(StripeTerminalConfiguration.LoggerTracePrefix + $"{nameof(Models.ReaderDisplayMessage)}: {displayMessage} : {message}");

        ReaderUpdateLabel?.Invoke(null, new ReaderUpdateLabelEventArgs(message, showCancel: displayMessage != Models.ReaderDisplayMessage.RemoveCard));
    }

    public void OnReportReaderEvent(global::Com.Stripe.Stripeterminal.External.Models.ReaderEvent @event)
    {
        var message = @event.Name();
        logger?.Trace(StripeTerminalConfiguration.LoggerTracePrefix + $"{nameof(Models.ReaderEvent)}: {@event} : {message}");
    }

    public void OnRequestReaderInput(Models.ReaderInputOptions inputOptions)
    {
        var message = inputOptions.ToString();
        logger?.Trace(StripeTerminalConfiguration.LoggerTracePrefix + $"{nameof(Models.ReaderInputOptions)}: {inputOptions} : {message}");

        ReaderUpdateLabel?.Invoke(null, new ReaderUpdateLabelEventArgs(message, showCancel: true));
    }

    public void OnReportAvailableUpdate(Models.ReaderSoftwareUpdate update)
    {
        //ReaderUpdateAvailable.Invoke(null, new ReaderSoftwareUpdateEventArgs(update, reader.SerialNumber));
    }

    public void OnStartInstallingUpdate(Models.ReaderSoftwareUpdate update, ICancelable cancelable)
    {
        //ReaderUpdateAvailable.Invoke(null, new ReaderSoftwareUpdateEventArgs(update, reader.SerialNumber));
    }

    public async void OnFinishInstallingUpdate(Models.ReaderSoftwareUpdate update, Models.TerminalException error)
    {
        ReaderUpdateProgress?.Invoke(null, new ReaderUpdateEventArgs());
        //await dialogService.HideProgress();
    }
}

public class ReaderSoftwareUpdateEventArgs : ReaderEventArgs
{
    public ReaderSoftwareUpdateEventArgs(Models.ReaderSoftwareUpdate softwareUpdate, string serial) : base(serial)
    {
        SoftwareUpdate = softwareUpdate;
    }

    public Models.ReaderSoftwareUpdate SoftwareUpdate { get; private set; }
}
