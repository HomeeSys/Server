namespace Raports.Application.Mappers;

internal class RaportMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        TypeAdapterConfig<Raport, DefaultRaportDTO>
            .NewConfig()
            .Map(x => x.ID, y => y.ID)
            .Map(x => x.StartDate, y => y.StartDate)
            .Map(x => x.EndDate, y => y.EndDate)
            .Map(x => x.Period, y => y.Period);
    }
}
