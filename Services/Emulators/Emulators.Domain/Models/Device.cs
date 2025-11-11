namespace Emulators.Domain.Models;

public class Device
{
    public int ID { get; set; }
    public Guid DeviceNumber { get; set; }
    /// <summary>
    /// Measurement value spread, expressed in percentage.
    /// </summary>
    public double Spread { get; set; }
}
