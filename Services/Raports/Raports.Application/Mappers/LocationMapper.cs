namespace Raports.Application.Mappers;

internal class LocationMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        TypeAdapterConfig<Location, DefaultLocationDTO>
            .NewConfig()
            .Map(x => x.ID, y => y.ID)
            .Map(x => x.Hash, y => y.Hash)
            .Map(x => x.Name, y => y.Name);
    }
}
