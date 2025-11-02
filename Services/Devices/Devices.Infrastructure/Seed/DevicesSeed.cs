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
                    new Device(){ Name = "KP12", DeviceNumber = new Guid("4cd40176-4872-44a5-b355-ec51f60ca3cd"), LocationID = 1, TimestampConfigurationID = 1, StatusID = 1, RegisterDate = DateTime.Now, MeasurementConfigurationID = 1 },
                    new Device(){ Name = "KP13", DeviceNumber = new Guid("83f17437-0048-416e-a57a-a8a13a06a1df"), LocationID = 1, TimestampConfigurationID = 1, StatusID = 1, RegisterDate = DateTime.Now, MeasurementConfigurationID = 2 },
                    new Device(){ Name = "KP14", DeviceNumber = new Guid("945bb002-587e-4ffa-8b99-fa65523a1489"), LocationID = 1, TimestampConfigurationID = 1, StatusID = 1, RegisterDate = DateTime.Now, MeasurementConfigurationID = 3 },
                    new Device(){ Name = "KP15", DeviceNumber = new Guid("f35d4d49-95ac-4909-960b-45e96bb93f67"), LocationID = 1, TimestampConfigurationID = 1, StatusID = 1, RegisterDate = DateTime.Now, MeasurementConfigurationID = 4 },
                    new Device(){ Name = "KP16", DeviceNumber = new Guid("13cd5515-9978-4ab6-bbd4-940c91bcb5b5"), LocationID = 2, TimestampConfigurationID = 1, StatusID = 1, RegisterDate = DateTime.Now, MeasurementConfigurationID = 5 },
                    new Device(){ Name = "KP17", DeviceNumber = new Guid("9759f5f4-f642-4660-9247-a33bc8e1aba8"), LocationID = 2, TimestampConfigurationID = 1, StatusID = 1, RegisterDate = DateTime.Now, MeasurementConfigurationID = 6 },
                    new Device(){ Name = "KP18", DeviceNumber = new Guid("2ce4a7cc-c8c0-47b2-bc2a-79629a3b491b"), LocationID = 2, TimestampConfigurationID = 1, StatusID = 1, RegisterDate = DateTime.Now, MeasurementConfigurationID = 7 },
                    new Device(){ Name = "KP19", DeviceNumber = new Guid("05d895a2-047c-4c21-b23c-dee53bc246e9"), LocationID = 2, TimestampConfigurationID = 1, StatusID = 1, RegisterDate = DateTime.Now, MeasurementConfigurationID = 8 },
                    new Device(){ Name = "KP20", DeviceNumber = new Guid("353f0875-c1d9-4bfa-8cec-8c35e76eb80c"), LocationID = 2, TimestampConfigurationID = 1, StatusID = 1, RegisterDate = DateTime.Now, MeasurementConfigurationID = 9 },
                    new Device(){ Name = "KP21", DeviceNumber = new Guid("4ce8c17a-87d7-43f8-af4b-dc7dfbe09dff"), LocationID = 2, TimestampConfigurationID = 1, StatusID = 1, RegisterDate = DateTime.Now, MeasurementConfigurationID = 10 },
                };
            }
        }
    }
}
