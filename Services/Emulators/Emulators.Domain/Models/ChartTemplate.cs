namespace Emulators.Domain.Models;

public class ChartTemplate
{
    public int ID { get; set; }

    public Measurement Measurement { get; set; }
    public int MeasurementID { get; set; }

    public ICollection<Sample> Samples { get; set; }
}