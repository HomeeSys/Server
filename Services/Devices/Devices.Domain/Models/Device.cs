namespace Devices.Domain.Models
{
    public class Device
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Guid DeviceNumber { get; set; }
        public DateTime RegisterDate { get; set; }
        public int LocationID { get; set; }
        public Location Location { get; set; } = default!;
        public int TimestampConfigurationID { get; set; }
        public TimestampConfiguration TimestampConfiguration { get; set; }
        public int StatusID { get; set; }
        public Status Status { get; set; }
        public int MeasurementConfigurationID { get; set; }
        public MeasurementConfiguration MeasurementConfiguration { get; set; }
    }
}
