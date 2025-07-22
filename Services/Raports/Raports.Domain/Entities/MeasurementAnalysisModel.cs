using System.Text.Json;

namespace Raports.Domain.Entities;

public class MeasurementAnalysisModel
{
    public string MeasurementType { get; set; }
    public DateTime[] Time { get; set; }
    public List<MeasurementData> Measurements { get; set; }

    public string ToJson()
    {
        string result = JsonSerializer.Serialize(this);

        return result;
    }
}


public static class MeasurementAnalysisModelExtensions
{
    public static MeasurementAnalysisModel ToAnalysisModel(this MeasurementPacket entity)
    {
        return new MeasurementAnalysisModel()
        {
            Time = entity.Time,
            MeasurementType = entity.MeasurementName,
            Measurements = entity.Measurements
        };
    }
}
