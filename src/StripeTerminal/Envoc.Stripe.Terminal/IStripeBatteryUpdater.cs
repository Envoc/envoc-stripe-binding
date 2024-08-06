namespace StripeTerminal;

public interface IStripeBatteryUpdater
{
    Task UpdateBatteryStatus(ReaderBatteryUpdateEventArgs e);
}

internal class EmptyBatteryUpdater : IStripeBatteryUpdater
{
    public Task UpdateBatteryStatus(ReaderBatteryUpdateEventArgs e)
    {
        return Task.CompletedTask;
    }
}