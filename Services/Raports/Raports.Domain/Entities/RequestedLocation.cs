namespace Raports.Domain.Entities;

public class RequestedLocation
{
    public int ID { get; set; }

    public int LocationID { get; set; }
    public Location Location { get; set; }
    public int RaportID { get; set; }
    public Raport Raport { get; set; }
}
