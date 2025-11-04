namespace Measurements.Domain.Models;

public class MeasurementTMP
{
    public Guid ID { get; set; }
    public Guid DeviceNumber { get; set; }
    public DateTime RegisterDate { get; set; }
    public double? Temperature { get; set; }
    public double? Humidity { get; set; }
    public double? CO2 { get; set; }
    public double? VOC { get; set; }
    public double? ParticulateMatter1 { get; set; }
    public double? ParticulateMatter2v5 { get; set; }
    public double? ParticulateMatter10 { get; set; }
    public double? Formaldehyde { get; set; }
    public double? CO { get; set; }
    public double? O3 { get; set; }
    public double? Ammonia { get; set; }
    public double? Airflow { get; set; }
    public double? AirIonizationLevel { get; set; }
    public double? O2 { get; set; }
    public double? Radon { get; set; }
    public double? Illuminance { get; set; }
    public double? SoundLevel { get; set; }
}
