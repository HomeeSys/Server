namespace Devices.Application.Mappers;

internal class TimestampMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        TypeAdapterConfig<Timestamp, DevicesMessage_DefaultTimestamp>
            .NewConfig()
            .Map(x => x.ID, y => y.ID)
            .Map(x => x.Cron, y => y.Cron);

        TypeAdapterConfig<DevicesMessage_DefaultTimestamp, Timestamp>
            .NewConfig()
            .Map(x => x.ID, y => y.ID)
            .Map(x => x.Cron, y => y.Cron);

        //  --------------- Mappings with messaging types ---------------
        TypeAdapterConfig<TimestampDTO, DevicesMessage_DefaultTimestamp>
            .NewConfig()
            .Map(x => x.ID, y => y.ID)
            .Map(x => x.Cron, y => y.Cron);

        TypeAdapterConfig<DevicesMessage_DefaultTimestamp, TimestampDTO>
            .NewConfig()
            .Map(x => x.ID, y => y.ID)
            .Map(x => x.Cron, y => y.Cron);

        //  --------------- Mappings with dtos types ---------------
        TypeAdapterConfig<TimestampDTO, Timestamp>
            .NewConfig()
            .Map(x => x.ID, y => y.ID)
            .Map(x => x.Cron, y => y.Cron);

        TypeAdapterConfig<Timestamp, TimestampDTO>
            .NewConfig()
            .Map(x => x.ID, y => y.ID)
            .Map(x => x.Cron, y => y.Cron);
    }
}
