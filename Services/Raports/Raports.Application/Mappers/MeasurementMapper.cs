namespace Raports.Application.Mappers;

internal class MeasurementMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        TypeAdapterConfig<Measurement, DefaultMeasurementDTO>
            .NewConfig()
            .Map(x => x.ID, y => y.ID)
            .Map(x => x.Unit, y => y.Unit)
            .Map(x => x.Name, y => y.Name);
    }
}
