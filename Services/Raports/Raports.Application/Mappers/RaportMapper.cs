namespace Raports.Application.Mappers;

internal class RaportMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        TypeAdapterConfig<Raport, DefaultRaportDTO>
            .NewConfig()
            .Map(x => x.ID, y => y.ID)
            .Map(x => x.CreationDate, y => y.CreationDate)
            .Map(x => x.StartDate, y => y.StartDate)
            .Map(x => x.EndDate, y => y.EndDate)
            .Map(x => x.Period, y => y.Period)
            .Map(x => x.Request, y => y.Request);

        TypeAdapterConfig<Raport, RaportDTONoRequest>
            .NewConfig()
            .Map(x => x.ID, y => y.ID)
            .Map(x => x.CreationDate, y => y.CreationDate)
            .Map(x => x.StartDate, y => y.StartDate)
            .Map(x => x.EndDate, y => y.EndDate)
            .Map(x => x.Period, y => y.Period);
    }
}
