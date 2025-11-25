namespace Devices.Application.Mappers;

internal class LocationMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        TypeAdapterConfig<Location, DevicesMessage_DefaultLocation>
            .NewConfig()
            .Map(x => x.ID, y => y.ID)
            .Map(x => x.Hash, y => y.Hash)
            .Map(x => x.Name, y => y.Name);

        TypeAdapterConfig<DevicesMessage_DefaultLocation, Location>
            .NewConfig()
            .Map(x => x.ID, y => y.ID)
            .Map(x => x.Hash, y => y.Hash)
            .Map(x => x.Name, y => y.Name);

        //  --------------- Mappings with messaging types ---------------
        TypeAdapterConfig<LocationDTO, DevicesMessage_DefaultLocation>
            .NewConfig()
            .Map(x => x.ID, y => y.ID)
            .Map(x => x.Hash, y => y.Hash)
            .Map(x => x.Name, y => y.Name);

        TypeAdapterConfig<DevicesMessage_DefaultLocation, LocationDTO>
            .NewConfig()
            .Map(x => x.ID, y => y.ID)
            .Map(x => x.Hash, y => y.Hash)
            .Map(x => x.Name, y => y.Name);

        //  --------------- Mappings with dtos types ---------------
        TypeAdapterConfig<LocationDTO, Location>
            .NewConfig()
            .Map(x => x.ID, y => y.ID)
            .Map(x => x.Hash, y => y.Hash)
            .Map(x => x.Name, y => y.Name);

        TypeAdapterConfig<Location, LocationDTO>
            .NewConfig()
            .Map(x => x.ID, y => y.ID)
            .Map(x => x.Hash, y => y.Hash)
            .Map(x => x.Name, y => y.Name);
    }
}
