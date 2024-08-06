using Com.Stripe.Stripeterminal.External.Callable;
using Models = Com.Stripe.Stripeterminal.External.Models;

namespace StripeTerminal.Bluetooth;

public class BluetoothConnector : Java.Lang.Object, IReaderListener
{
    private readonly IStripeTerminalLogger logger;
    private readonly IStripeBatteryUpdater batteryUpdater;

    public BluetoothConnector(IStripeTerminalLogger logger, IStripeBatteryUpdater batteryUpdater)
    {
        this.logger = logger;
        this.batteryUpdater = batteryUpdater;
    }

    public event EventHandler<ReaderUpdateEventArgs> ReaderUpdateProgress;
    public event EventHandler<ReaderSoftwareUpdateEventArgs> ReaderUpdateAvailable;
    public event EventHandler<ReaderUpdateLabelEventArgs> ReaderUpdateLabel;
    public event EventHandler<ReaderUpdateLabelEventArgs> ReaderErrorMessage;
    public event EventHandler<ReaderBatteryUpdateEventArgs> ReaderBatteryUpdate;


    public void OnBatteryLevelUpdate(float batteryLevel, Models.BatteryStatus status, bool isCharging)
    {
        float batteryPct = batteryLevel * 100;

        batteryUpdater?.UpdateBatteryStatus(new ReaderBatteryUpdateEventArgs { BatteryLevel = batteryPct, IsCharging = isCharging });
        ReaderBatteryUpdate?.Invoke(null, new ReaderBatteryUpdateEventArgs { BatteryLevel = batteryPct, IsCharging = isCharging });
    }

    public void OnReportLowBatteryWarning()
    {
        batteryUpdater?.UpdateBatteryStatus(new ReaderBatteryUpdateEventArgs { IsLow = true });
        ReaderBatteryUpdate?.Invoke(null, new ReaderBatteryUpdateEventArgs { IsLow = true });
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

    public void OnFinishInstallingUpdate(Models.ReaderSoftwareUpdate update, Models.TerminalException error)
    {
        if (error is null)
        {
            ReaderUpdateProgress?.Invoke(null, new ReaderUpdateEventArgs());
        }
        else
        {
            ReaderErrorMessage?.Invoke(null, new ReaderUpdateLabelEventArgs(error.ErrorMessage, showCancel: false));
        }
    }

    //TODO: Figure out Android event for error alert
}

public class ReaderSoftwareUpdateEventArgs : ReaderEventArgs
{
    public ReaderSoftwareUpdateEventArgs(Models.ReaderSoftwareUpdate softwareUpdate, string serial) : base(serial)
    {
        SoftwareUpdate = softwareUpdate;
    }

    public Models.ReaderSoftwareUpdate SoftwareUpdate { get; private set; }
}
