namespace Devices.Infrastructure.Seed
{
    internal class LocationsSeed
    {
        public static IEnumerable<Location> Locations
        {
            get
            {
                return new List<Location>()
                {
                    new Location(){ Name = "Living room"},
                    new Location(){ Name = "Kitchen"},
                };
            }
        }
    }
}
