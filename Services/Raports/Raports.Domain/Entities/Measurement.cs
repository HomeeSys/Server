namespace Raports.Domain.Entities;

public class Measurement
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Unit { get; set; }
    public int MinChartYValue { get; set; }
    public int MaxChartYValue { get; set; }
}
