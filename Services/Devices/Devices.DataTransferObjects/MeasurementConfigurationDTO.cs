namespace Devices.DataTransferObjects;

public record DefaultMeasurementConfigurationDTO(
    int ID,
    bool Temperature,
    bool Humidity,
    bool CarbonDioxide,
    bool VolatileOrganicCompounds,
    bool PM1,
    bool PM25,
    bool PM10,
    bool Formaldehyde,
    bool CarbonMonoxide,
    bool Ozone,
    bool Ammonia,
    bool Airflow,
    bool AirIonizationLevel,
    bool Oxygen,
    bool Radon,
    bool Illuminance,
    bool SoundLevel
);

public record MeasurementConfigDTO(int ID, int DeviceID, bool Temperature, bool Humidity, bool CarbonDioxide,
    bool VolatileOrganicCompounds, bool PM1, bool PM25, bool PM10, bool Formaldehyde, bool CarbonMonoxide,
    bool Ozone, bool Ammonia, bool Airflow, bool AirIonizationLevel, bool Oxygen, bool Radon, bool Illuminance, bool SoundLevel
);

public record UpdateMeasurementConfigDTO(bool? Temperature, bool? Humidity, bool? CarbonDioxide,
    bool? VolatileOrganicCompounds, bool? PM1, bool? PM25, bool? PM10, bool? Formaldehyde, bool? CarbonMonoxide,
    bool? Ozone, bool? Ammonia, bool? Airflow, bool? AirIonizationLevel, bool? Oxygen, bool? Radon, bool? Illuminance, bool? SoundLevel
);
