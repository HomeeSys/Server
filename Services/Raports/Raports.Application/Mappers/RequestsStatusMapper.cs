namespace Raports.Application.Mappers;

internal class RequestsStatusMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        TypeAdapterConfig<RequestStatus, DefaultRequestStatusDTO>
            .NewConfig()
            .Map(x => x.ID, y => y.ID)
            .Map(x => x.Name, y => y.Name)
            .Map(x => x.Description, y => y.Description);
    }
}
