namespace Measurements.Domain.Models
{
    public class QueryableMeasurementSet
    {
        public Guid ID { get; set; }
        public Guid DeviceNumber { get; set; }
        public string DeviceName { get; set; }
        public string Location { get; set; }
        public DateTime RegisterDate { get; set; }
        public Measurement? Temperature { get; set; }
        public Measurement? Humidity { get; set; }
        public Measurement? CO2 { get; set; }
        public Measurement? VOC { get; set; }
        public Measurement? ParticulateMatter1 { get; set; }
        public Measurement? ParticulateMatter2v5 { get; set; }
        public Measurement? ParticulateMatter10 { get; set; }
        public Measurement? Formaldehyde { get; set; }
        public Measurement? CO { get; set; }
        public Measurement? O3 { get; set; }
        public Measurement? Ammonia { get; set; }
        public Measurement? Airflow { get; set; }
        public Measurement? AirIonizationLevel { get; set; }
        public Measurement? O2 { get; set; }
        public Measurement? Radon { get; set; }
        public Measurement? Illuminance { get; set; }
        public Measurement? SoundLevel { get; set; }
    }
}
