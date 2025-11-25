namespace Devices.Application.Mappers;

internal class StatusMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        TypeAdapterConfig<Status, DevicesMessage_DefaultStatus>
        .NewConfig()
        .Map(x => x.ID, y => y.ID)
        .Map(x => x.Type, y => y.Type);

        TypeAdapterConfig<DevicesMessage_DefaultStatus, Status>
            .NewConfig()
            .Map(x => x.ID, y => y.ID)
            .Map(x => x.Type, y => y.Type);

        //  --------------- Mappings with messaging types ---------------
        TypeAdapterConfig<StatusDTO, DevicesMessage_DefaultStatus>
            .NewConfig()
            .Map(x => x.ID, y => y.ID)
            .Map(x => x.Type, y => y.Type);

        TypeAdapterConfig<DevicesMessage_DefaultStatus, StatusDTO>
            .NewConfig()
            .Map(x => x.ID, y => y.ID)
            .Map(x => x.Type, y => y.Type);

        //  --------------- Mappings with messaging types ---------------
        TypeAdapterConfig<StatusDTO, Status>
            .NewConfig()
            .Map(x => x.ID, y => y.ID);

        TypeAdapterConfig<Status, StatusDTO>
            .NewConfig()
            .Map(x => x.ID, y => y.ID);
    }
}
