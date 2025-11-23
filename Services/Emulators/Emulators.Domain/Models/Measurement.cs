namespace Emulators.Domain.Models;

public class Measurement
{
    public int ID { get; set; }

    /// <summary>
    /// Name of chart, for example: 'Air Temperature'
    /// </summary>
    public string Name { get; set; }
    public string Unit { get; set; }
}
