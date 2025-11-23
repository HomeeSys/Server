namespace Emulators.Domain.Models;

public class Sample
{
    public int ID { get; set; }

    public int ChartTemplateID { get; set; }
    public ChartTemplate ChartTemplate { get; set; }

    public TimeOnly Time { get; set; }
    public double Value { get; set; }
}