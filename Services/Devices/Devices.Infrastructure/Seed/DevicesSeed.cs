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
                    new Device(){ DeviceNumber = new Guid("B9558A38-8F92-48B0-8530-42D258B710C8"), LocationId = 1, TimestampConfigurationId = 1, StatusId = 1, Description = "Description", RegisterDate = DateTime.Now },
                    new Device(){ DeviceNumber = new Guid("7B95D91A-5D16-4A5C-BB8F-BCD829A489E3"), LocationId = 1, TimestampConfigurationId = 1, StatusId = 1, Description = "Description", RegisterDate = DateTime.Now },
                    new Device(){ DeviceNumber = new Guid("4F903717-86E4-453C-AC2F-E143A3AD4E46"), LocationId = 1, TimestampConfigurationId = 1, StatusId = 1, Description = "Description", RegisterDate = DateTime.Now },
                    new Device(){ DeviceNumber = new Guid("181660F3-AFBE-45B7-8DB7-DD02F7F929E2"), LocationId = 1, TimestampConfigurationId = 1, StatusId = 1, Description = "Description", RegisterDate = DateTime.Now },
                    new Device(){ DeviceNumber = new Guid("E80BA8CD-6419-4525-9AD5-E0BF80B523E0"), LocationId = 1, TimestampConfigurationId = 1, StatusId = 1, Description = "Description", RegisterDate = DateTime.Now },
                    new Device(){ DeviceNumber = new Guid("D69DC173-3BED-46E1-97E9-7EEAC36F642A"), LocationId = 1, TimestampConfigurationId = 1, StatusId = 1, Description = "Description", RegisterDate = DateTime.Now },
                    new Device(){ DeviceNumber = new Guid("82B45C9B-EDA5-4E4F-9F6D-A8EE6E3624BE"), LocationId = 1, TimestampConfigurationId = 1, StatusId = 1, Description = "Description", RegisterDate = DateTime.Now },
                    new Device(){ DeviceNumber = new Guid("1D25DF2F-9848-4188-8C8F-238A69D1C9BA"), LocationId = 1, TimestampConfigurationId = 1, StatusId = 1, Description = "Description", RegisterDate = DateTime.Now },
                    new Device(){ DeviceNumber = new Guid("942E075D-6BD7-4F3F-B4CF-2ACAB7A45EAC"), LocationId = 1, TimestampConfigurationId = 1, StatusId = 1, Description = "Description", RegisterDate = DateTime.Now },
                };
            }
        }
    }
}
