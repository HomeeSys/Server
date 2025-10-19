namespace Measurements.DataTransferObjects
{
    public record QueryableMeasurementSetDTO(
        Guid ID,
        Guid DeviceNumber,
        string DeviceName,
        string Location,
        DateTime RegisterDate,
        MeasurementDTO? Temperature,
        MeasurementDTO? Humidity,
        MeasurementDTO? CO2,
        MeasurementDTO? VOC,
        MeasurementDTO? ParticulateMatter1,
        MeasurementDTO? ParticulateMatter2v5,
        MeasurementDTO? ParticulateMatter10,
        MeasurementDTO? Formaldehyde,
        MeasurementDTO? CO,
        MeasurementDTO? O3,
        MeasurementDTO? Ammonia,
        MeasurementDTO? Airflow,
        MeasurementDTO? AirIonizationLevel,
        MeasurementDTO? O2,
        MeasurementDTO? Radon,
        MeasurementDTO? Illuminance,
        MeasurementDTO? SoundLevel
    );
}
