namespace Devices.Domain.Models;

public class DeviceMeasurementType
{
    public int ID { get; set; }

    public Device Device { get; set; }
    public int DeviceID { get; set; }
    public MeasurementType MeasurementType { get; set; }
    public int MeasurementTypeID { get; set; }
}
