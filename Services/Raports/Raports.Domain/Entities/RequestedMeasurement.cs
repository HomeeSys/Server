namespace Raports.Domain.Entities;

public class RequestedMeasurement
{
    public int ID { get; set; }

    public int MeasurementID { get; set; }
    public Measurement Measurement { get; set; }
    public int RaportID { get; set; }
    public Raport Raport { get; set; }
}
