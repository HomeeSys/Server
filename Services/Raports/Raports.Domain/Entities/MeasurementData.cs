namespace Raports.Domain.Entities;

public class MeasurementData
{
    /// <summary>
    /// Data label. For for example: 'Living room', 'Kitchen' or 'Device 1'.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Raw data
    /// </summary>
    public double[] Data { get; set; }
    public MeasurementData(string name, double[] data)
    {
        Name = name;
        Data = data;
    }
}
