using StripeTerminal.Bluetooth;

namespace StripeTerminal.LocalMobile;

public class LocalMobileConnector : SCPLocalMobileReaderDelegate
{
    private readonly IStripeTerminalLogger logger;

    public LocalMobileConnector(IStripeTerminalLogger logger)
    {
        this.logger = logger;
    }

    public event EventHandler<ReaderUpdateEventArgs> ReaderUpdateProgress;
    public event EventHandler<ReaderSoftwareUpdateEventArgs> ReaderUpdateAvailable;
    public event EventHandler<ReaderUpdateLabelEventArgs> ReaderUpdateLabel;

    public override void LocalMobileReader(SCPReader reader, float progress)
    {
        ReaderUpdateProgress?.Invoke(reader, new ReaderUpdateEventArgs(progress));
    }

    public override void LocalMobileReader(SCPReader reader, SCPReaderDisplayMessage displayMessage)
    {
        var message = SCPTerminal.StringFromReaderDisplayMessage(displayMessage);
        logger?.Trace(StripeTerminalConfiguration.LoggerTracePrefix + $"{nameof(SCPReaderDisplayMessage)}: {displayMessage} : {message}");

        ReaderUpdateLabel?.Invoke(reader, new ReaderUpdateLabelEventArgs(message, showCancel: displayMessage != SCPReaderDisplayMessage.RemoveCard));
    }

    public override void LocalMobileReader(SCPReader reader, SCPReaderInputOptions inputOptions)
    {
        var message = SCPTerminal.StringFromReaderInputOptions(inputOptions);
        logger?.Trace(StripeTerminalConfiguration.LoggerTracePrefix + $"{nameof(SCPReaderInputOptions)}: {inputOptions} : {message}");

        ReaderUpdateLabel?.Invoke(reader, new ReaderUpdateLabelEventArgs(message, showCancel: true));
    }

    public override void LocalMobileReader(SCPReader reader, SCPReaderSoftwareUpdate update, SCPCancelable cancelable)
    {
        ReaderUpdateAvailable?.Invoke(reader, new ReaderSoftwareUpdateEventArgs(update, reader?.SerialNumber));
    }

    public override void LocalMobileReader(SCPReader reader, SCPReaderSoftwareUpdate update, NSError error)
    {
        ReaderUpdateProgress?.Invoke(null, new ReaderUpdateEventArgs());
    }

    public override void LocalMobileReaderDidAcceptTermsOfService(SCPReader reader)
    {
        //TODO: ?
    }
}