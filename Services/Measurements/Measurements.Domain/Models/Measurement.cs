namespace Measurements.Domain.Models;

public class Measurement
{
    public Guid ID { get; set; }
    public Guid DeviceNumber { get; set; }
    public DateTime MeasurementCaptureDate { get; set; }
    public Guid LocationHash { get; set; }

    /// <summary>
    /// Temperature measured in degrees Celsius (°C).
    /// </summary>
    public double? Temperature { get; set; }

    /// <summary>
    /// Relative humidity measured as a percentage (% RH).
    /// </summary>
    public double? Humidity { get; set; }

    /// <summary>
    /// Carbon dioxide concentration measured in parts per million (ppm).
    /// </summary>
    public double? CarbonDioxide { get; set; }

    /// <summary>
    /// Volatile organic compounds (VOCs) concentration measured in micrograms per cubic meter (µg/m³).
    /// </summary>
    public double? VolatileOrganicCompounds { get; set; }

    /// <summary>
    /// Particulate matter (PM1) concentration measured in micrograms per cubic meter (µg/m³).
    /// </summary>
    public double? ParticulateMatter1 { get; set; }

    /// <summary>
    /// Particulate matter (PM2.5) concentration measured in micrograms per cubic meter (µg/m³).
    /// </summary>
    public double? ParticulateMatter2v5 { get; set; }

    /// <summary>
    /// Particulate matter (PM10) concentration measured in micrograms per cubic meter (µg/m³).
    /// </summary>
    public double? ParticulateMatter10 { get; set; }

    /// <summary>
    /// Formaldehyde concentration measured in micrograms per cubic meter (µg/m³).
    /// </summary>
    public double? Formaldehyde { get; set; }

    /// <summary>
    /// Carbon monoxide concentration measured in parts per million (ppm).
    /// </summary>
    public double? CarbonMonoxide { get; set; }

    /// <summary>
    /// Ozone concentration measured in parts per billion (ppb).
    /// </summary>
    public double? Ozone { get; set; }

    /// <summary>
    /// Ammonia concentration measured in milligrams per cubic meter (mg/m³).
    /// </summary>
    public double? Ammonia { get; set; }

    /// <summary>
    /// Airflow rate measured in cubic feet per minute (CFM).
    /// </summary>
    public double? Airflow { get; set; }

    /// <summary>
    /// Air ionization level measured in ions per cubic centimeter (ions/cm³).
    /// </summary>
    public double? AirIonizationLevel { get; set; }

    /// <summary>
    /// Oxygen concentration measured as a percentage (%).
    /// </summary>
    public double? Oxygen { get; set; }

    /// <summary>
    /// Radon concentration measured in becquerels per cubic meter (Bq/m³).
    /// </summary>
    public double? Radon { get; set; }

    /// <summary>
    /// Illuminance measured in lux (lx).
    /// </summary>
    public double? Illuminance { get; set; }

    /// <summary>
    /// Sound level measured in decibels (dB).
    /// </summary>
    public double? SoundLevel { get; set; }
}
