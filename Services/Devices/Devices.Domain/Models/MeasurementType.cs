namespace Devices.Domain.Models;

public class MeasurementType
{
    public int ID { get; set; }

    public string Name { get; set; }
    public string Unit { get; set; }
    public ICollection<Device> Devices { get; set; }
}
