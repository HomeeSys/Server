namespace Devices.Application.Mappers;

internal class MeasurementConfigMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        TypeAdapterConfig<MeasurementConfig, MeasurementConfigDTO>
            .NewConfig()
            .Map(x => x.ID, y => y.Id)
            .Map(x => x.DeviceID, y => y.DeviceId)
            .Map(x => x.PM1, y => y.PM1)
            .Map(x => x.PM25, y => y.PM25)
            .Map(x => x.PM10, y => y.PM10);
    }
}
