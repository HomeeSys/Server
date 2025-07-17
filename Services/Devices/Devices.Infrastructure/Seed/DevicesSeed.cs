namespace Devices.Infrastructure.Seed
{
    internal class DevicesSeed
    {
        public static IEnumerable<Device> Devices
        {
            get
            {
                return new List<Device>()
                {
                    new Device(){ DeviceNumber = new Guid("4cd40176-4872-44a5-b355-ec51f60ca3cd"), LocationId = 1, TimestampConfigurationId = 1, StatusId = 1, Description = "Description", RegisterDate = DateTime.Now },
                    new Device(){ DeviceNumber = new Guid("83f17437-0048-416e-a57a-a8a13a06a1df"), LocationId = 1, TimestampConfigurationId = 1, StatusId = 1, Description = "Description", RegisterDate = DateTime.Now },
                    new Device(){ DeviceNumber = new Guid("945bb002-587e-4ffa-8b99-fa65523a1489"), LocationId = 1, TimestampConfigurationId = 1, StatusId = 1, Description = "Description", RegisterDate = DateTime.Now },
                    new Device(){ DeviceNumber = new Guid("f35d4d49-95ac-4909-960b-45e96bb93f67"), LocationId = 1, TimestampConfigurationId = 1, StatusId = 1, Description = "Description", RegisterDate = DateTime.Now },
                    new Device(){ DeviceNumber = new Guid("13cd5515-9978-4ab6-bbd4-940c91bcb5b5"), LocationId = 2, TimestampConfigurationId = 1, StatusId = 1, Description = "Description", RegisterDate = DateTime.Now },
                    new Device(){ DeviceNumber = new Guid("9759f5f4-f642-4660-9247-a33bc8e1aba8"), LocationId = 2, TimestampConfigurationId = 1, StatusId = 1, Description = "Description", RegisterDate = DateTime.Now },
                    new Device(){ DeviceNumber = new Guid("2ce4a7cc-c8c0-47b2-bc2a-79629a3b491b"), LocationId = 2, TimestampConfigurationId = 1, StatusId = 1, Description = "Description", RegisterDate = DateTime.Now },
                    new Device(){ DeviceNumber = new Guid("05d895a2-047c-4c21-b23c-dee53bc246e9"), LocationId = 2, TimestampConfigurationId = 1, StatusId = 1, Description = "Description", RegisterDate = DateTime.Now },
                    new Device(){ DeviceNumber = new Guid("353f0875-c1d9-4bfa-8cec-8c35e76eb80c"), LocationId = 2, TimestampConfigurationId = 1, StatusId = 1, Description = "Description", RegisterDate = DateTime.Now },
                    new Device(){ DeviceNumber = new Guid("4ce8c17a-87d7-43f8-af4b-dc7dfbe09dff"), LocationId = 1, TimestampConfigurationId = 1, StatusId = 1, Description = "Description", RegisterDate = DateTime.Now },
                };
            }
        }
    }
}
