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
                    new Status(){ Type = "Registered"},
                    new Status(){ Type = "Active"},
                    new Status(){ Type = "Deleted"},
                };
            }
        }
    }
}
