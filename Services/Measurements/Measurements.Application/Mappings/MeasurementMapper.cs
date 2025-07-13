namespace Measurements.Application.Mappings;

internal class MeasurementMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        TypeAdapterConfig<MeasurementSet, MeasurementSetDTO>
            .NewConfig()
            .Map(x=>x.CO, y=>y.CO)
            .Map(x=>x.ParticulateMatter2v5, y=>y.ParticulateMatter2v5)
            .Map(x => x.VOC, y=> y.VOC);
    }
}
