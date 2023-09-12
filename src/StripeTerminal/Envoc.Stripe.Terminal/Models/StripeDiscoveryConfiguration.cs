namespace StripeTerminal;

public class StripeDiscoveryConfiguration
{
    /// <summary>
    /// 15 by default
    /// </summary>
    public int? TimeOut { get; set; } = 15;

    /// <summary>
    /// For iOS Only. BluetoothScan by default
    /// </summary>
    public DiscoveryType DiscoveryMethod { get; set; }

    /// <summary>
    /// False by default
    /// </summary>
    public bool IsSimulated { get; set; }

    public string LocationId { get; set; }
}

public enum DiscoveryType
{
    Bluetooth,
    Internet,
    Local
}