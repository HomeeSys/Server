namespace Raports.Domain.Entities;

public class Raport
{
    public int ID { get; set; }
    public DateTime RaportCreationDate { get; set; }
    public DateTime RaportCompletedDate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Message { get; set; }

    public Guid DocumentHash { get; set; }

    public int PeriodID { get; set; }
    public Period Period { get; set; }

    public int StatusID { get; set; }
    public Status Status { get; set; }

    public ICollection<MeasurementGroup> MeasurementGroups { get; set; }
    public ICollection<Measurement> RequestedMeasurements { get; set; }
    public ICollection<Location> RequestedLocations { get; set; }
}
