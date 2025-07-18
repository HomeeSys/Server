namespace CommonServiceLibrary.GRPC.Entities;

public class MeasurementSetGRPC
{
    public Guid ID { get; set; }
    public Guid DeviceNumber { get; set; }
    public DateTime RegisterDate { get; set; }
    public MeasurementGRPC? Temperature { get; set; }
    public MeasurementGRPC? Humidity { get; set; }
    public MeasurementGRPC? CO2 { get; set; }
    public MeasurementGRPC? VOC { get; set; }
    public MeasurementGRPC? PartuculateMatter1 { get; set; }
    public MeasurementGRPC? PartuculateMatter2v5 { get; set; }
    public MeasurementGRPC? PartuculateMatter10 { get; set; }
    public MeasurementGRPC? Formaldehyde { get; set; }
    public MeasurementGRPC? CO { get; set; }
    public MeasurementGRPC? O3 { get; set; }
    public MeasurementGRPC? Ammonia { get; set; }
    public MeasurementGRPC? Airflow { get; set; }
    public MeasurementGRPC? AirIonizationLevel { get; set; }
    public MeasurementGRPC? O2 { get; set; }
    public MeasurementGRPC? Radon { get; set; }
    public MeasurementGRPC? Illuminance { get; set; }
    public MeasurementGRPC? SoundLevel { get; set; }
}