namespace Emulator.Devices.DataModels;

internal class MeasurementConfigurationModel
{
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

    public bool IsEqual(MeasurementConfigurationModel other)
    {
        return Temperature == other.Temperature &&
            Humidity == other.Humidity &&
            CarbonDioxide == other.CarbonDioxide &&
            VolatileOrganicCompounds == other.VolatileOrganicCompounds &&
            PM1 == other.PM1 &&
            PM25 == other.PM25 &&
            PM10 == other.PM10 &&
            Formaldehyde == other.Formaldehyde &&
            CarbonMonoxide == other.CarbonMonoxide &&
            Ozone == other.Ozone &&
            Ammonia == other.Ammonia &&
            Airflow == other.Airflow &&
            AirIonizationLevel == other.AirIonizationLevel &&
            Oxygen == other.Oxygen &&
            Radon == other.Radon &&
            Illuminance == other.Illuminance &&
            SoundLevel == other.SoundLevel;
    }

    public void Update(MeasurementConfigurationModel other)
    {
        Temperature = other.Temperature;
        Humidity = other.Humidity;
        CarbonDioxide = other.CarbonDioxide;
        VolatileOrganicCompounds = other.VolatileOrganicCompounds;
        PM1 = other.PM1;
        PM25 = other.PM25;
        PM10 = other.PM10;
        Formaldehyde = other.Formaldehyde;
        CarbonMonoxide = other.CarbonMonoxide;
        Ozone = other.Ozone;
        Ammonia = other.Ammonia;
        Airflow = other.Airflow;
        AirIonizationLevel = other.AirIonizationLevel;
        Oxygen = other.Oxygen;
        Radon = other.Radon;
        Illuminance = other.Illuminance;
        SoundLevel = other.SoundLevel;
    }
}
