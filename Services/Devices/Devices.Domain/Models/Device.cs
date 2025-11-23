namespace Devices.Domain.Models;

public class Device
{
    public int ID { get; set; }

    public string Name { get; set; }
    public Guid DeviceNumber { get; set; }
    public DateTime RegisterDate { get; set; }

    public int LocationID { get; set; }
    public Location Location { get; set; }
    public int TimestampID { get; set; }
    public Timestamp Timestamp { get; set; }
    public int StatusID { get; set; }
    public Status Status { get; set; }
    public ICollection<MeasurementType> MeasurementTypes { get; set; }
}
