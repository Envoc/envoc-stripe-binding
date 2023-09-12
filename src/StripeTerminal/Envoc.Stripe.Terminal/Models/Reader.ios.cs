namespace StripeTerminal;

public static class ReaderExtensions
{
    public static Reader FromNative(this SCPReader entity, SCPReader connected)
    {
        var reader = FromNative(entity);

        reader.IsConnected = reader.SerialNumber == connected?.SerialNumber;

        return reader;
    }

    public static Reader FromNative(this SCPReader entity)
    {
        if (entity == null)
        {
            return null;
        }

        var reader = new Reader
        {
            BatteryStatus = (BatteryStatus)entity.BatteryStatus,
            DeviceSoftwareVersion = entity.DeviceSoftwareVersion,
            DeviceType = entity.DeviceType.ToString(),
            DeviceTypeFriendly = SCPTerminal.StringFromDeviceType(entity.DeviceType),
            IpAddress = entity.IpAddress,
            Label = entity.Label,
            LocationId = entity.LocationId,
            LocationStatus = (LocationStatus)entity.LocationStatus,
            SerialNumber = entity.SerialNumber,
            Simulated = entity.Simulated,
            Status = (ReaderNetworkStatus)entity.Status,
            StripeId = entity.StripeId
        };

        if (entity.AvailableUpdate != null)
        {
            reader.AvailableUpdate = new ReaderSoftwareUpdate
            {
                DeviceSoftwareVersion = entity.AvailableUpdate.DeviceSoftwareVersion,
                EstimatedUpdateTime = (UpdateTimeEstimate)entity.AvailableUpdate.EstimatedUpdateTime,
                RequiredAt = (DateTime)entity.AvailableUpdate.RequiredAt
            };
        }

        if (entity.Location != null)
        {
            reader.Location = new ReaderLocation
            {
                DisplayName = entity.Location.DisplayName,
                Livemode = entity.Location.Livemode,
                //Metadata = entity.Location.Metadata.ToDictionary(x => x.Key.ToString(), x => x.Value.ToString()) ?? throw new NullReferenceException("The value of 'entity.Location.Metadata' should not be null"),
                StripeId = entity.Location.StripeId
            };
        }

        if (entity.BatteryLevel != null && double.TryParse(entity.BatteryLevel.ToString(), out var batteryLevel))
        {
            reader.BatteryLevel = batteryLevel;
        }

        if (entity.IsCharging != null && bool.TryParse(entity.IsCharging.ToString(), out var isCharging))
        {
            reader.IsCharging = isCharging;
        }

        return reader;
    }
}
