namespace Devices.Application.Mappers;

internal class DeviceMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        TypeAdapterConfig<Device, DevicesMessage_DefaultDevice>
            .NewConfig()
            .Map(x => x.ID, y => y.ID)
            .Map(x => x.DeviceNumber, y => y.DeviceNumber)
            .Map(x => x.RegisterDate, y => y.RegisterDate)
            .Map(x => x.Name, y => y.Name);

        TypeAdapterConfig<DevicesMessage_DefaultDevice, Device>
            .NewConfig()
            .Map(x => x.ID, y => y.ID)
            .Map(x => x.DeviceNumber, y => y.DeviceNumber)
            .Map(x => x.RegisterDate, y => y.RegisterDate)
            .Map(x => x.Name, y => y.Name);

        //  --------------- Mappings with messaging types ---------------
        TypeAdapterConfig<DefaultDeviceDTO, DevicesMessage_DefaultDevice>
            .NewConfig()
            .Map(x => x.ID, y => y.ID)
            .Map(x => x.DeviceNumber, y => y.DeviceNumber)
            .Map(x => x.RegisterDate, y => y.RegisterDate)
            .Map(x => x.Name, y => y.Name);

        TypeAdapterConfig<DevicesMessage_DefaultDevice, DefaultDeviceDTO>
            .NewConfig()
            .Map(x => x.ID, y => y.ID)
            .Map(x => x.DeviceNumber, y => y.DeviceNumber)
            .Map(x => x.RegisterDate, y => y.RegisterDate)
            .Map(x => x.Name, y => y.Name);

        //  --------------- Mappings with dto types ---------------
        TypeAdapterConfig<DefaultDeviceDTO, Device>
            .NewConfig()
            .Map(x => x.ID, y => y.ID);

        TypeAdapterConfig<Device, DefaultDeviceDTO>
            .NewConfig()
            .Map(x => x.ID, y => y.ID);
    }
}
