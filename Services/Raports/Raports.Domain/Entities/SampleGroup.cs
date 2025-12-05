namespace Raports.Domain.Entities;

public class SampleGroup
{
    public int ID { get; set; }
    public DateTime Date { get; set; }
    public double Value { get; set; }

    public int LocationGroupID { get; set; }
    public LocationGroup LocationGroup { get; set; }
}
