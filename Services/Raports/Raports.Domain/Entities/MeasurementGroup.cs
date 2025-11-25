namespace Raports.Domain.Entities;

public class MeasurementGroup
{
    public int ID { get; set; }
    public string Summary { get; set; }

    public int RaportID { get; set; }
    public Raport Raport { get; set; }

    public int MeasurementID { get; set; }
    public Measurement Measurement { get; set; }

    public ICollection<LocationGroup> LocationGroups { get; set; }
}
