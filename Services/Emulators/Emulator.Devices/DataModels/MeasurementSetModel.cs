namespace Emulator.Devices.DataModels;

internal class MeasurementSetModel
{
    public Guid DeviceNumber { get; set; }
    public DateTime RegisterDate { get; set; }
    public MeasurementModel? Temperature { get; set; }
    public MeasurementModel? Humidity { get; set; }
    public MeasurementModel? CO2 { get; set; }
    public MeasurementModel? VOC { get; set; }
    public MeasurementModel? ParticulateMatter1 { get; set; }
    public MeasurementModel? ParticulateMatter2v5 { get; set; }
    public MeasurementModel? ParticulateMatter10 { get; set; }
    public MeasurementModel? Formaldehyde { get; set; }
    public MeasurementModel? CO { get; set; }
    public MeasurementModel? O3 { get; set; }
    public MeasurementModel? Ammonia { get; set; }
    public MeasurementModel? Airflow { get; set; }
    public MeasurementModel? AirIonizationLevel { get; set; }
    public MeasurementModel? O2 { get; set; }
    public MeasurementModel? Radon { get; set; }
    public MeasurementModel? Illuminance { get; set; }
    public MeasurementModel? SoundLevel { get; set; }
}
