namespace Emulator.Devices.DataModels;

internal class TimestampConfigurationModel
{
    public string CRON { get; set; } = default!;
    public bool IsEqual(TimestampConfigurationModel other)
    {
        return CRON.Equals(other.CRON);
    }

    public void Update(TimestampConfigurationModel other)
    {
        CRON = other.CRON;
    }
}
