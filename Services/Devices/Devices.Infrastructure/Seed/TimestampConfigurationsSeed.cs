namespace Devices.Infrastructure.Seed
{
    public class TimestampConfigurationsSeed
    {
        public static IEnumerable<TimestampConfiguration> Timestamps
        {
            get
            {
                return new List<TimestampConfiguration>()
                {
                    new TimestampConfiguration(){ Cron = "* * * * *" },
                    new TimestampConfiguration(){ Cron = "*/5 * * * *" },
                };
            }
        }
    }
}
