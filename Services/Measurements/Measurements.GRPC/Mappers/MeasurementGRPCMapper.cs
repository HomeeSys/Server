namespace Measurements.GRPC.Mappers;

public class MeasurementGRPCMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        TypeAdapterConfig<MeasurementSet, MeasurementSetModel>
            .NewConfig()
            .Map(x => x.Co, y => y.CO)
            .Map(x => x.Co2, y => y.CO2)
            .Map(x => x.ParticulateMatter2V5, y => y.ParticulateMatter2v5)
            .Map(x => x.Voc, y => y.VOC);
    }
}
