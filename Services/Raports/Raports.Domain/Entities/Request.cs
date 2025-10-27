namespace Raports.Domain.Entities;

public class Request
{
    public int ID { get; set; }
    public DateTime RequestCreationDate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int StatusID { get; set; }
    public RequestStatus Status { get; set; }
    public int RaportID { get; set; }
    public Raport Raport { get; set; }
    public int PeriodID { get; set; }
    public Period Period { get; set; }
}
