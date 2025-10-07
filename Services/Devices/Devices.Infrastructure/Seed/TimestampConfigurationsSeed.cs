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
                    new TimestampConfiguration(){ Cron = "0/30 * * * * ?" },    //  Every 30 seconds
                    new TimestampConfiguration(){ Cron = "0 0/1 * * * ?" },     //  ... 1 minute
                    new TimestampConfiguration(){ Cron = "0 0/5 * * * ?" },     //  ... 5 minutes
                    new TimestampConfiguration(){ Cron = "0 0/10 * * * ?" },    //  ... 10 minutes
                    new TimestampConfiguration(){ Cron = "0 0/15 * * * ?" },    //  ... 15 minutes
                    new TimestampConfiguration(){ Cron = "0 0/30 * * * ?" },    //  ... 30 minutes
                    new TimestampConfiguration(){ Cron = "0 0 0/1 * * ?" },     //  ... 1 hour
                };
            }
        }
    }
}
