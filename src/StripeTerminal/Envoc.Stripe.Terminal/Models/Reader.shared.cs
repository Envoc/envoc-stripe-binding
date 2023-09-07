namespace StripeTerminal;

public partial class Reader
{
    public ReaderSoftwareUpdate AvailableUpdate { get; set; }
    public double BatteryLevel { get; set; }
    public BatteryStatus BatteryStatus { get; set; }
    public string DeviceSoftwareVersion { get; set; }
    public string DeviceType { get; set; }
    public string DeviceTypeFriendly { get; set; }
    public string IpAddress { get; set; }
    public bool? IsCharging { get; set; }
    public string Label { get; set; }
    public ReaderLocation Location { get; set; }
    public string LocationId { get; set; }
    public LocationStatus LocationStatus { get; set; }
    public string SerialNumber { get; set; }
    public bool Simulated { get; set; }
    public ReaderNetworkStatus Status { get; set; }
    public string StripeId { get; set; }

    public bool IsConnected { get; set; }

    //public static bool operator ==(Reader reader1, Reader reader2)
    //{
    //    if (reader1 == null || reader2 == null)
    //    {
    //        return false;
    //    }

    //    return reader1.SerialNumber == reader2.SerialNumber;
    //}

    //public static bool operator !=(Reader reader1, Reader reader2)
    //{
    //    if (reader1 == null || reader2 == null)
    //    {
    //        return true;
    //    }

    //    return reader1.SerialNumber != reader2.SerialNumber;
    //}

    //public override int GetHashCode()
    //{
    //    return HashCode.Combine(SerialNumber);
    //}

    //public override bool Equals(object obj)
    //{
    //    if (obj is null)
    //    {
    //        return false;
    //    }
        
    //    if (obj is Reader r)
    //    {
    //        return Equals(r);
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}
}

public class ReaderLocation
{
    public virtual string DisplayName { get; set; }
    public virtual bool Livemode { get; set; }
    public virtual Dictionary<string, string> Metadata { get; set; }
    public virtual string StripeId { get; set; }
}

public class ReaderSoftwareUpdate
{
    public string DeviceSoftwareVersion { get; set; }
    public virtual UpdateTimeEstimate EstimatedUpdateTime { get; set; }
    public DateTime RequiredAt { get; set; }

    public string FriendlyString { get; set; }

    public string GetFriendlyUpdateTimeEstimate() => FriendlyString ?? EstimatedUpdateTime switch
    {
        UpdateTimeEstimate.LessThanOneMinute => "Less Than 1 Minute",
        UpdateTimeEstimate.OneToTwoMinutes => "1 To 2 Minutes",
        UpdateTimeEstimate.TwoToFiveMinutes => "2 To 5 Minutes",
        UpdateTimeEstimate.FiveToFifteenMinutes => "5 To 15 Minutes",
        _ => "Unknown",
    };
}

public enum BatteryStatus
{
    Unknown,
    Critical,
    Low,
    Nominal
}

public enum ReaderNetworkStatus
{
    Offline,
    Online
}

public enum LocationStatus
{
    Unknown,
    Set,
    NotSet
}
public enum UpdateTimeEstimate
{
    LessThanOneMinute,
    OneToTwoMinutes,
    TwoToFiveMinutes,
    FiveToFifteenMinutes
}
