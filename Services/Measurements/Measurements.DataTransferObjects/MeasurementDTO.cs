namespace Measurements.DataTransferObjects;

/// <summary>
/// Represents a request to create a new environmental measurement record.
/// </summary>
public record CreateMeasurementDTO(
    Guid DeviceNumber,
    int LocationID,

    /// <summary>Temperature measured in degrees Celsius (°C).</summary>
    double? Temperature,

    /// <summary>Relative humidity measured as a percentage (% RH).</summary>
    double? Humidity,

    /// <summary>Carbon dioxide concentration measured in parts per million (ppm).</summary>
    double? CarbonDioxide,

    /// <summary>Volatile organic compounds (VOCs) concentration measured in micrograms per cubic meter (µg/m³).</summary>
    double? VolatileOrganicCompounds,

    /// <summary>Particulate matter (PM1) concentration measured in micrograms per cubic meter (µg/m³).</summary>
    double? ParticulateMatter1,

    /// <summary>Particulate matter (PM2.5) concentration measured in micrograms per cubic meter (µg/m³).</summary>
    double? ParticulateMatter2v5,

    /// <summary>Particulate matter (PM10) concentration measured in micrograms per cubic meter (µg/m³).</summary>
    double? ParticulateMatter10,

    /// <summary>Formaldehyde concentration measured in micrograms per cubic meter (µg/m³).</summary>
    double? Formaldehyde,

    /// <summary>Carbon monoxide concentration measured in parts per million (ppm).</summary>
    double? CarbonMonoxide,

    /// <summary>Ozone concentration measured in parts per billion (ppb).</summary>
    double? Ozone,

    /// <summary>Ammonia concentration measured in milligrams per cubic meter (mg/m³).</summary>
    double? Ammonia,

    /// <summary>Airflow rate measured in cubic feet per minute (CFM).</summary>
    double? Airflow,

    /// <summary>Air ionization level measured in ions per cubic centimeter (ions/cm³).</summary>
    double? AirIonizationLevel,

    /// <summary>Oxygen concentration measured as a percentage (%).</summary>
    double? Oxygen,

    /// <summary>Radon concentration measured in becquerels per cubic meter (Bq/m³).</summary>
    double? Radon,

    /// <summary>Illuminance measured in lux (lx).</summary>
    double? Illuminance,

    /// <summary>Sound level measured in decibels (dB).</summary>
    double? SoundLevel
);

/// <summary>
/// Represents a default environmental measurement data transfer object, typically returned from the API.
/// </summary>
public record DefaultMeasurementDTO(
    Guid ID,
    Guid DeviceNumber,
    int LocationID,
    DateTime RecordedAt,

    /// <summary>Temperature measured in degrees Celsius (°C).</summary>
    double? Temperature,

    /// <summary>Relative humidity measured as a percentage (% RH).</summary>
    double? Humidity,

    /// <summary>Carbon dioxide concentration measured in parts per million (ppm).</summary>
    double? CarbonDioxide,

    /// <summary>Volatile organic compounds (VOCs) concentration measured in micrograms per cubic meter (µg/m³).</summary>
    double? VolatileOrganicCompounds,

    /// <summary>Particulate matter (PM1) concentration measured in micrograms per cubic meter (µg/m³).</summary>
    double? ParticulateMatter1,

    /// <summary>Particulate matter (PM2.5) concentration measured in micrograms per cubic meter (µg/m³).</summary>
    double? ParticulateMatter2v5,

    /// <summary>Particulate matter (PM10) concentration measured in micrograms per cubic meter (µg/m³).</summary>
    double? ParticulateMatter10,

    /// <summary>Formaldehyde concentration measured in micrograms per cubic meter (µg/m³).</summary>
    double? Formaldehyde,

    /// <summary>Carbon monoxide concentration measured in parts per million (ppm).</summary>
    double? CarbonMonoxide,

    /// <summary>Ozone concentration measured in parts per billion (ppb).</summary>
    double? Ozone,

    /// <summary>Ammonia concentration measured in milligrams per cubic meter (mg/m³).</summary>
    double? Ammonia,

    /// <summary>Airflow rate measured in cubic feet per minute (CFM).</summary>
    double? Airflow,

    /// <summary>Air ionization level measured in ions per cubic centimeter (ions/cm³).</summary>
    double? AirIonizationLevel,

    /// <summary>Oxygen concentration measured as a percentage (%).</summary>
    double? Oxygen,

    /// <summary>Radon concentration measured in becquerels per cubic meter (Bq/m³).</summary>
    double? Radon,

    /// <summary>Illuminance measured in lux (lx).</summary>
    double? Illuminance,

    /// <summary>Sound level measured in decibels (dB).</summary>
    double? SoundLevel
);

/// <summary>
/// Provides full data about captured measurement.
/// </summary>
/// <param name="ID"></param>
/// <param name="RecordedAt"></param>
/// <param name="DeviceID"></param>
/// <param name="DeviceName"></param>
/// <param name="DeviceNumber"></param>
/// <param name="LocationID"></param>
/// <param name="LocationName"></param>
/// <param name="Temperature"></param>
/// <param name="Humidity"></param>
/// <param name="CarbonDioxide"></param>
/// <param name="VolatileOrganicCompounds"></param>
/// <param name="ParticulateMatter1"></param>
/// <param name="ParticulateMatter2v5"></param>
/// <param name="ParticulateMatter10"></param>
/// <param name="Formaldehyde"></param>
/// <param name="CarbonMonoxide"></param>
/// <param name="Ozone"></param>
/// <param name="Ammonia"></param>
/// <param name="Airflow"></param>
/// <param name="AirIonizationLevel"></param>
/// <param name="Oxygen"></param>
/// <param name="Radon"></param>
/// <param name="Illuminance"></param>
/// <param name="SoundLevel"></param>
public record CombinedMeasurementDTO(
    /// <summary>Measurement unique ID.</summary>
    Guid ID,

    /// <summary>Date of measurement registration.</summary>
    DateTime RecordedAt,

    /// <summary>ID of device that captured data.</summary>
    int DeviceID,

    /// <summary>Name of device that captured data.</summary>
    string DeviceName,

    /// <summary>Number of device that captured data.</summary>
    Guid DeviceNumber,

    /// <summary>ID of location where the data was captured.</summary>
    int LocationID,

    /// <summary>Name of location where the data was captured.</summary>
    string LocationName,

    /// <summary>Temperature measured in degrees Celsius (°C).</summary>
    double? Temperature,

    /// <summary>Relative humidity measured as a percentage (% RH).</summary>
    double? Humidity,

    /// <summary>Carbon dioxide concentration measured in parts per million (ppm).</summary>
    double? CarbonDioxide,

    /// <summary>Volatile organic compounds (VOCs) concentration measured in micrograms per cubic meter (µg/m³).</summary>
    double? VolatileOrganicCompounds,

    /// <summary>Particulate matter (PM1) concentration measured in micrograms per cubic meter (µg/m³).</summary>
    double? ParticulateMatter1,

    /// <summary>Particulate matter (PM2.5) concentration measured in micrograms per cubic meter (µg/m³).</summary>
    double? ParticulateMatter2v5,

    /// <summary>Particulate matter (PM10) concentration measured in micrograms per cubic meter (µg/m³).</summary>
    double? ParticulateMatter10,

    /// <summary>Formaldehyde concentration measured in micrograms per cubic meter (µg/m³).</summary>
    double? Formaldehyde,

    /// <summary>Carbon monoxide concentration measured in parts per million (ppm).</summary>
    double? CarbonMonoxide,

    /// <summary>Ozone concentration measured in parts per billion (ppb).</summary>
    double? Ozone,

    /// <summary>Ammonia concentration measured in milligrams per cubic meter (mg/m³).</summary>
    double? Ammonia,

    /// <summary>Airflow rate measured in cubic feet per minute (CFM).</summary>
    double? Airflow,

    /// <summary>Air ionization level measured in ions per cubic centimeter (ions/cm³).</summary>
    double? AirIonizationLevel,

    /// <summary>Oxygen concentration measured as a percentage (%).</summary>
    double? Oxygen,

    /// <summary>Radon concentration measured in becquerels per cubic meter (Bq/m³).</summary>
    double? Radon,

    /// <summary>Illuminance measured in lux (lx).</summary>
    double? Illuminance,

    /// <summary>Sound level measured in decibels (dB).</summary>
    double? SoundLevel
);