namespace Devices.Infrastructure.Seed
{
    internal class StatusesSeed
    {
        public static IEnumerable<Status> Statuses
        {
            get
            {
                return new List<Status>()
                {
                    new Status(){ Type = "Offline"},
                    new Status(){ Type = "Online"},
                    new Status(){ Type = "Disabled"},
                };
            }
        }
    }
}
