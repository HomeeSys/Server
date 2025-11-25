namespace Raports.Application.Mappers;

internal class StatusMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        TypeAdapterConfig<Status, DefaultStatusDTO>
            .NewConfig()
            .Map(x => x.ID, y => y.ID)
            .Map(x => x.Name, y => y.Name)
            .Map(x => x.Description, y => y.Description);
    }
}
