namespace Raports.Domain.Entities;

public class LocationGroup
{
    public int ID { get; set; }
    public string Summary { get; set; }

    public int LocationID { get; set; }
    public Location Location { get; set; }

    public int MeasurementGroupID { get; set; }
    public MeasurementGroup MeasurementGroup { get; set; }

    public ICollection<SampleGroup> SampleGroups { get; set; }
}
