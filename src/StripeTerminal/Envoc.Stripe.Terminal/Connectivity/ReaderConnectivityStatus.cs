using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StripeTerminal.Connectivity;

[Flags]
public enum ReaderConnectivityStatus
{
    Unknown = 0,
    Disconnected = 1 << 0,
    Connecting = 1 << 1,
    Bluetooth = 1 << 2,
    Internet = 1 << 3,
    Local = 1 << 4,
}