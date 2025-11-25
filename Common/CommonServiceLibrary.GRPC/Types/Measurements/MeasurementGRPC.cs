namespace CommonServiceLibrary.GRPC.Types.Measurements;

public record MeasurementGRPC(
    Guid ID,
    Guid DeviceNumber,
    DateTime MeasurementCaptureDate,
    Guid LocationHash,
    double? Temperature,
    double? Humidity,
    double? CarbonDioxide,
    double? VolatileOrganicCompounds,
    double? ParticulateMatter1,
    double? ParticulateMatter2v5,
    double? ParticulateMatter10,
    double? Formaldehyde,
    double? CarbonMonoxide,
    double? Ozone,
    double? Ammonia,
    double? Airflow,
    double? AirIonizationLevel,
    double? Oxygen,
    double? Radon,
    double? Illuminance,
    double? SoundLevel
);