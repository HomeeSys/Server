namespace Emulator.Devices;

internal static class MapingConfiguration
{
    public static void Configure()
    {
        TypeAdapterConfig<LocationDTO, LocationModel>
            .NewConfig()
            .Map(dest => dest.Name, src => src.Name);

        TypeAdapterConfig<MeasurementConfigDTO, MeasurementConfigurationModel>
            .NewConfig()
            .Map(dest => dest.Temperature, src => src.Temperature);

        TypeAdapterConfig<TimestampConfigurationDTO, TimestampConfigurationModel>
            .NewConfig()
            .Map(dest => dest.CRON, src => src.Cron);

        TypeAdapterConfig<StatusDTO, StatusModel>
            .NewConfig()
            .Map(dest => dest.Type, src => src.Type);

        TypeAdapterConfig<DefaultDeviceDTO, DeviceModel>
            .NewConfig()
            .Map(dest => dest.DeviceNumber, src => src.DeviceNumber)
            .Map(dest => dest.Location, src => src.Location)
            .Map(dest => dest.Timestamp, src => src.TimestampConfiguration)
            .Map(dest => dest.Status, src => src.Status)
            .Map(dest => dest.Configuration, src => src.MeasurementConfiguration)
            .Map(dest => dest.Name, src => src.Name);
    }
}
