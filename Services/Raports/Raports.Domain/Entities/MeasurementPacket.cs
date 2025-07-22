namespace Raports.Domain.Entities
{
    public class MeasurementPacket
    {
        public string MeasurementName { get; set; }
        public string Description { get; set; }
        public DateTime[] Time { get; set; }
        public List<MeasurementData> Measurements { get; set; }
        public MeasurementPacket(DateTime[] time, List<MeasurementData> measurements, int dataDisplayModulo,
            int maxY, int minY, string name, string description)
        {
            Time = time;
            Measurements = measurements;
            MeasurementName = name;
            Description = description;
        }
    }
}