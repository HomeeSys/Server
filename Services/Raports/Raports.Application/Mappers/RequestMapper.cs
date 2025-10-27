namespace Raports.Application.Mappers;

internal class RequestMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        TypeAdapterConfig<Request, DefaultRequestDTO>
            .NewConfig()
            .Map(x => x.ID, y => y.ID)
            .Map(x => x.RequestCreationDate, y => y.RequestCreationDate)
            .Map(x => x.StartDate, y => y.StartDate)
            .Map(x => x.EndDate, y => y.EndDate)
            .Map(x => x.Period, y => y.Period)
            .Map(x => x.Status, y => y.Status);

        TypeAdapterConfig<Request, RequestDTONoRaport>
            .NewConfig()
            .Map(x => x.ID, y => y.ID)
            .Map(x => x.RequestCreationDate, y => y.RequestCreationDate)
            .Map(x => x.StartDate, y => y.StartDate)
            .Map(x => x.EndDate, y => y.EndDate)
            .Map(x => x.Period, y => y.Period)
            .Map(x => x.Status, y => y.Status);
    }
}
