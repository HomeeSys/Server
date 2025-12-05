namespace Raports.Domain.Entities;

public class Period
{
    public int ID { get; set; }
    public string Name { get; set; }
    public TimeSpan TimeFrame { get; set; }
    public int MaxAcceptableMissingTimeFrame { get; set; }
}