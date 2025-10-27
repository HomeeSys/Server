namespace Raports.Domain.Entities;

public class Raport
{
    public int ID { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int PeriodID { get; set; }
    public Period Period { get; set; }
    public int RequestID { get; set; }
    public Request Request { get; set; }
}
