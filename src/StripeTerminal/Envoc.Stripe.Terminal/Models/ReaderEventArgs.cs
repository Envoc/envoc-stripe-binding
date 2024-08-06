using StripeTerminal.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StripeTerminal;

public class ReaderUpdateEventArgs : EventArgs
{
    public ReaderUpdateEventArgs()
    {
        IsCompleted = true;
        Progress = 1;
    }

    public ReaderUpdateEventArgs(float progress)
    {
        Progress = progress;
    }

    public float Progress { get; private set; }

    public bool IsCompleted { get; private set; }
}

public class ReaderEventArgs : EventArgs
{
    public ReaderEventArgs(string serial)
    {
        Serial = serial;
    }

    public string Serial { get; private set; }
}

public class RefreshReaderEventArgs : ReaderEventArgs
{
    public RefreshReaderEventArgs(Reader reader) : base(reader?.SerialNumber)
    {
        Reader = reader;
    }

    public Reader Reader { get; private set; }
}

public class ConnectionStatusEventArgs : EventArgs
{
    public ConnectionStatusEventArgs(ReaderConnectivityStatus status, ConnectionType connectionType)
    {
        Status = status;
        ConnectionType = connectionType;
    }

    public ReaderConnectivityStatus Status { get; }

    public ConnectionType ConnectionType { get; }

}

public class ReaderUpdateLabelEventArgs : EventArgs
{
    public ReaderUpdateLabelEventArgs(string message, bool showCancel)
    {
        Message = message;
        ShowCancel = showCancel;    
    }

    public string Message { get; }

    public bool ShowCancel { get; }
}

public class ReaderUnexpectedDisconnectEventArgs(Reader reader) : EventArgs
{
    public Reader Reader { get; } = reader;
}

public class ReaderBatteryUpdateEventArgs : EventArgs
{
    public float? BatteryLevel { get; set; }
    public bool IsLow { get; set; }
    public bool IsCharging { get; set; }
}

public enum ConnectionType
{
    Internet,
    Bluetooth,
    Local,
    Handoff
}