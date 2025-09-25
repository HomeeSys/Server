namespace Devices.Domain.Models;

public class MeasurementConfig
{
    public int Id { get; set; }

    public int DeviceId { get; set; }
    public Device Device { get; set; }

    public bool Temperature { get; set; }
    public bool Humidity { get; set; }
    public bool CarbonDioxide { get; set; }
    public bool VolatileOrganicCompounds { get; set; }
    public bool PM1 { get; set; }
    public bool PM25 { get; set; }
    public bool PM10 { get; set; }
    public bool Formaldehyde { get; set; }
    public bool CarbonMonoxide { get; set; }
    public bool Ozone { get; set; }
    public bool Ammonia { get; set; }
    public bool Airflow { get; set; }
    public bool AirIonizationLevel { get; set; }
    public bool Oxygen { get; set; }
    public bool Radon { get; set; }
    public bool Illuminance { get; set; }
    public bool SoundLevel { get; set; }
}