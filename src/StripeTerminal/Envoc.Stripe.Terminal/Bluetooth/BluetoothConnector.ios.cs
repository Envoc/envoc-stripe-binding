namespace StripeTerminal.Bluetooth;

public class BluetoothConnector : SCPBluetoothReaderDelegate
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

    //didReportBatteryLevel
    public override void BatteryLevel(SCPReader reader, float batteryLevel, SCPBatteryStatus status, bool isCharging)
    {
        float batteryPct = batteryLevel * 100;

        batteryUpdater?.UpdateBatteryStatus(new ReaderBatteryUpdateEventArgs { BatteryLevel = batteryPct, IsCharging = isCharging });
        ReaderBatteryUpdate?.Invoke(null, new ReaderBatteryUpdateEventArgs { BatteryLevel = batteryPct, IsCharging = isCharging });
    }

    public override void LowBatteryWarning(SCPReader reader)
    {
        batteryUpdater?.UpdateBatteryStatus(new ReaderBatteryUpdateEventArgs { IsLow = true });
        ReaderBatteryUpdate?.Invoke(null, new ReaderBatteryUpdateEventArgs { IsLow = true });
    }

    //didReportReaderSoftwareUpdateProgress
    public override void SoftwareUpdateProgress(SCPReader reader, float progress)
    {
        ReaderUpdateProgress?.Invoke(reader, new ReaderUpdateEventArgs(progress));
    }

    //didRequestReaderDisplayMessage
    public override void ReaderDisplayMessage(SCPReader reader, SCPReaderDisplayMessage displayMessage)
    {
        var message = SCPTerminal.StringFromReaderDisplayMessage(displayMessage);
        logger?.Trace(StripeTerminalConfiguration.LoggerTracePrefix + $"{nameof(SCPReaderDisplayMessage)}: {displayMessage} : {message}");

        ReaderUpdateLabel?.Invoke(reader, new ReaderUpdateLabelEventArgs(message, showCancel: displayMessage != SCPReaderDisplayMessage.RemoveCard));
    }

    //didReportReaderEvent
    public override void ReaderEvent(SCPReader reader, SCPReaderEvent @event, NSDictionary info)
    {
        var message = SCPTerminal.StringFromReaderEvent(@event);
        logger?.Trace(StripeTerminalConfiguration.LoggerTracePrefix + $"{nameof(SCPReaderEvent)}: {@event} : {message}");
    }

    //didRequestReaderInput
    public override void ReaderInput(SCPReader reader, SCPReaderInputOptions inputOptions)
    {
        var message = SCPTerminal.StringFromReaderInputOptions(inputOptions);
        logger?.Trace(StripeTerminalConfiguration.LoggerTracePrefix + $"{nameof(SCPReaderInputOptions)}: {inputOptions} : {message}");

        ReaderUpdateLabel?.Invoke(reader, new ReaderUpdateLabelEventArgs(message, showCancel: true));
    }

    //didReportAvailableUpdate
    public override void AvailableUpdate(SCPReader reader, SCPReaderSoftwareUpdate update)
    {
        ReaderUpdateAvailable.Invoke(reader, new ReaderSoftwareUpdateEventArgs(update, reader.SerialNumber));
    }

    //didStartInstallingUpdate
    public override void StartInstallingUpdate(SCPReader reader, SCPReaderSoftwareUpdate update, SCPCancelable cancelable)
    {
        ReaderUpdateAvailable.Invoke(reader, new ReaderSoftwareUpdateEventArgs(update, reader.SerialNumber));
    }

    //didFinishInstallingUpdate
    public override void FinishInstallingUpdate(SCPReader reader, SCPReaderSoftwareUpdate update, NSError error)
    {
        if (error is null)
        {
            ReaderUpdateProgress?.Invoke(null, new ReaderUpdateEventArgs());
        }
        else
        {
            ReaderErrorMessage?.Invoke(null, new ReaderUpdateLabelEventArgs(error.LocalizedDescription, showCancel: false));
        }
    }
}

public class ReaderSoftwareUpdateEventArgs : ReaderEventArgs
{
    public ReaderSoftwareUpdateEventArgs(SCPReaderSoftwareUpdate softwareUpdate, string serial) : base(serial)
    {
        SoftwareUpdate = softwareUpdate;
    }

    public SCPReaderSoftwareUpdate SoftwareUpdate { get; private set; }
}