namespace Measurements.Domain.Models;

public class MeasurementSet
{
    [JsonProperty(PropertyName = "id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid DeviceNumber { get; set; }



    public DateTime RegisterDate { get; set; }

    /// <summary>
    /// Room temperature.
    /// Default unit: deg C
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Measurement? Temperature { get; set; }

    /// <summary>
    /// Room humidity.
    /// Default unit: % RH
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Measurement? Humidity { get; set; }

    /// <summary>
    /// Carbon dioxide.
    /// Default unit: ppm
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Measurement? CO2 { get; set; }

    /// <summary>
    /// Volatile organic compounds.
    /// Default unit: ug/m3
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Measurement? VOC { get; set; }

    /// <summary>
    /// Particles with diameter < 1 µm.
    /// Default unit: ug/m3
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Measurement? ParticulateMatter1 { get; set; }

    /// <summary>
    /// Particles with diameter < 2.5 µm.
    /// Default unit: ug/m3
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Measurement? ParticulateMatter2v5 { get; set; }

    /// <summary>
    /// Particles with diameter < 10 µm.
    /// Default unit: ug/m3
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Measurement? ParticulateMatter10 { get; set; }

    /// <summary>
    /// Formaldehyde level.
    /// Default unit: ug/m3
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Measurement? Formaldehyde { get; set; }

    /// <summary>
    /// Carbon monoxide.
    /// Default unit: ppm
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Measurement? CO { get; set; }

    /// <summary>
    /// Ozone concentration.
    /// Default unit: ppb
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Measurement? O3 { get; set; }

    /// <summary>
    /// Ammonia concentration.
    /// Default unit: mg/m3
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Measurement? Ammonia { get; set; }

    /// <summary>
    /// Airflow rate.
    /// Default unit: CFM
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Measurement? Airflow { get; set; }

    /// <summary>
    /// Air ionization level.
    /// Default unit: ions/cm3
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Measurement? AirIonizationLevel { get; set; }

    /// <summary>
    /// Oxygen concentration.
    /// Default unit: %
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Measurement? O2 { get; set; }

    /// <summary>
    /// Radon concentration.
    /// Default unit: Bq/m3
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Measurement? Radon { get; set; }

    /// <summary>
    /// Light level.
    /// Default unit: lux
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Measurement? Illuminance { get; set; }

    /// <summary>
    /// Noise level.
    /// Default unit: dB
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Measurement? SoundLevel { get; set; }
}