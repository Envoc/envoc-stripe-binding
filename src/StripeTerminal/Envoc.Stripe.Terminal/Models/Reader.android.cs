using Models = Com.Stripe.Stripeterminal.External.Models;

namespace StripeTerminal;

public static class ReaderExtensions
{
    public static Reader FromNative(this Models.Reader entity, Models.Reader connected)
    {
        var reader = entity.FromNative();

        reader.IsConnected = reader.SerialNumber == connected?.SerialNumber;

        return reader;
    }

    public static Reader FromNative(this Models.Reader entity)
    {
        if (entity == null)
        {
            return null;
        }

        var reader = new Reader
        {
            //No equivalent
            //BatteryStatus = (Models.BatteryStatus)entity.BatteryLevel,
            DeviceSoftwareVersion = entity.FirmwareVersion,
            DeviceType = entity.DeviceType.ToString(),
            DeviceTypeFriendly = entity.DeviceType.Name(),
            IpAddress = entity.IpAddress,
            Label = entity.Label,
            LocationId = entity.Location?.Id,
            SerialNumber = entity.SerialNumber,
            Simulated = entity.IsSimulated,
            StripeId = entity.Id
        };

        if (entity.BatteryLevel != null)
        {
            reader.BatteryLevel = entity.BatteryLevel.DoubleValue();
        }

        if (entity.LocationStatus != null)
        {
            var locationStatusString = entity.LocationStatus.Name();

            if (locationStatusString.Equals(Models.LocationStatus.NotSet.Name(), StringComparison.InvariantCultureIgnoreCase))
            {
                reader.LocationStatus = LocationStatus.NotSet;
            }
            else if (locationStatusString.Equals(Models.LocationStatus.Set.Name(), StringComparison.InvariantCultureIgnoreCase))
            {
                reader.LocationStatus = LocationStatus.Set;
            }
            else //if (locationStatusString.Equals(Models.LocationStatus.Unknown.Name(), StringComparison.InvariantCultureIgnoreCase))
            {
                reader.LocationStatus = LocationStatus.Unknown;
            }
        }

        var networkStatus = entity.GetNetworkStatus();
        if (networkStatus != null)
        {
            var networkStatusString = networkStatus.Name();

            if (networkStatusString.Equals(Models.Reader.NetworkStatus.Online.Name(), StringComparison.InvariantCultureIgnoreCase))
            {
                reader.Status = ReaderNetworkStatus.Online;
            }
            else //if (networkStatusString.Equals(Models.Reader.NetworkStatus.Offline.Name(), StringComparison.InvariantCultureIgnoreCase))
            {
                reader.Status = ReaderNetworkStatus.Offline;
            }

            //TODO: Default?
        }

        if (entity.AvailableUpdate is Models.ReaderSoftwareUpdate update)
        {
            reader.AvailableUpdate = new ReaderSoftwareUpdate
            {
                DeviceSoftwareVersion = update.Version,
            };

            //Maybe?
            //reader.AvailableUpdate.FriendlyString = update.TimeEstimate.Description;

            if (update.RequiredAt != null)
            {
                var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                reader.AvailableUpdate.RequiredAt = epoch.AddMilliseconds(update.RequiredAt.Time);
            }

            if (update.TimeEstimate != null)
            {
                var timeEstimateString = update.TimeEstimate.Name();

                if (timeEstimateString.Equals(Models.ReaderSoftwareUpdate.UpdateTimeEstimate.LessThanOneMinute.Name(), StringComparison.InvariantCultureIgnoreCase))
                {
                    reader.AvailableUpdate.EstimatedUpdateTime = UpdateTimeEstimate.LessThanOneMinute;
                }
                else if (timeEstimateString.Equals(Models.ReaderSoftwareUpdate.UpdateTimeEstimate.OneToTwoMinutes.Name(), StringComparison.InvariantCultureIgnoreCase))
                {
                    reader.AvailableUpdate.EstimatedUpdateTime = UpdateTimeEstimate.OneToTwoMinutes;
                }
                else if (timeEstimateString.Equals(Models.ReaderSoftwareUpdate.UpdateTimeEstimate.TwoToFiveMinutes.Name(), StringComparison.InvariantCultureIgnoreCase))
                {
                    reader.AvailableUpdate.EstimatedUpdateTime = UpdateTimeEstimate.TwoToFiveMinutes;
                }
                else //if (timeEstimateString.Equals(Models.ReaderSoftwareUpdate.UpdateTimeEstimate.FiveToFifteenMinutes.Name(), StringComparison.InvariantCultureIgnoreCase))
                {
                    reader.AvailableUpdate.EstimatedUpdateTime = UpdateTimeEstimate.FiveToFifteenMinutes;
                }
            }
        }

        if (entity.Location != null)
        {
            reader.Location = new ReaderLocation
            {
                DisplayName = entity.Location.DisplayName,
                Livemode = (bool?)entity.Location.Livemode ?? false,
                //Metadata = entity.Location.Metadata.ToDictionary(x => x.Key.ToString(), x => x.Value.ToString()) ?? throw new NullReferenceException("The value of 'entity.Location.Metadata' should not be null"),
                StripeId = entity.Location.Id
            };
        }

        if (entity.BatteryLevel != null && double.TryParse(entity.BatteryLevel.ToString(), out var batteryLevel))
        {
            reader.BatteryLevel = batteryLevel;
        }

        //No equivalent
        //if (entity.IsCharging != null && bool.TryParse(entity.IsCharging.ToString(), out var isCharging))
        //{
        //    reader.IsCharging = isCharging;
        //}

        return reader;
    }
}