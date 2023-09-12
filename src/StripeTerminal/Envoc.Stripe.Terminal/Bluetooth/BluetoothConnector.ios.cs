namespace StripeTerminal.Bluetooth;

public class BluetoothConnector : SCPBluetoothReaderDelegate
{
    private readonly IStripeTerminalLogger logger;

    public BluetoothConnector(IStripeTerminalLogger logger)
    {
        this.logger = logger;
    }

    public event EventHandler<ReaderUpdateEventArgs> ReaderUpdateProgress;
    public event EventHandler<ReaderSoftwareUpdateEventArgs> ReaderUpdateAvailable;
    public event EventHandler<ReaderUpdateLabelEventArgs> ReaderUpdateLabel;

    public override void Reader(SCPReader reader, float batteryLevel, SCPBatteryStatus status, bool isCharging)
    {
    }

    public override void ReaderDidReportLowBatteryWarning(SCPReader reader)
    {
    }

    public override void Reader(SCPReader reader, float progress)
    {
        ReaderUpdateProgress?.Invoke(reader, new ReaderUpdateEventArgs(progress));
    }

    public override void Reader(SCPReader reader, SCPReaderDisplayMessage displayMessage)
    {
        var message = SCPTerminal.StringFromReaderDisplayMessage(displayMessage);
        logger?.Trace(StripeTerminalConfiguration.LoggerTracePrefix + $"{nameof(SCPReaderDisplayMessage)}: {displayMessage} : {message}");

        ReaderUpdateLabel?.Invoke(reader, new ReaderUpdateLabelEventArgs(message, showCancel: displayMessage != SCPReaderDisplayMessage.RemoveCard));
    }

    public override void Reader(SCPReader reader, SCPReaderEvent @event, NSDictionary info)
    {
        var message = SCPTerminal.StringFromReaderEvent(@event);
        logger?.Trace(StripeTerminalConfiguration.LoggerTracePrefix + $"{nameof(SCPReaderEvent)}: {@event} : {message}");
    }

    public override void Reader(SCPReader reader, SCPReaderInputOptions inputOptions)
    {
        var message = SCPTerminal.StringFromReaderInputOptions(inputOptions);
        logger?.Trace(StripeTerminalConfiguration.LoggerTracePrefix + $"{nameof(SCPReaderInputOptions)}: {inputOptions} : {message}");

        ReaderUpdateLabel?.Invoke(reader, new ReaderUpdateLabelEventArgs(message, showCancel: true));
    }

    public override void Reader(SCPReader reader, SCPReaderSoftwareUpdate update)
    {
        ReaderUpdateAvailable.Invoke(reader, new ReaderSoftwareUpdateEventArgs(update, reader.SerialNumber));
    }

    public override void Reader(SCPReader reader, SCPReaderSoftwareUpdate update, SCPCancelable cancelable)
    {
        ReaderUpdateAvailable.Invoke(reader, new ReaderSoftwareUpdateEventArgs(update, reader.SerialNumber));
    }

    public override void Reader(SCPReader reader, SCPReaderSoftwareUpdate update, NSError error)
    {
        ReaderUpdateProgress?.Invoke(null, new ReaderUpdateEventArgs());
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