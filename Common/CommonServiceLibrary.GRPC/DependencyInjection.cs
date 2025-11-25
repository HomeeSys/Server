namespace CommonServiceLibrary.GRPC;

public static class DependencyInjection
{
    public static IServiceCollection AddGRPCMappings(this IServiceCollection services)
    {
        //  Devices
        TypeAdapterConfig<GrpcDeviceModel, DeviceGRPC>
            .NewConfig()
            .Map(dest => dest.ID, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.DeviceNumber, src => src.DeviceNumber)
            .Map(dest => dest.RegisterDate, src => src.RegisterDate)
            .Map(dest => dest.Location, src => src.Location)
            .Map(dest => dest.Timestamp, src => src.TimestampConfiguration)
            .Map(dest => dest.Status, src => src.Status);

        TypeAdapterConfig<GrpcTimestampConfigurationModel, TimestampGRPC>
            .NewConfig()
            .Map(dest => dest.ID, src => src.Id)
            .Map(dest => dest.Cron, src => src.Cron);

        TypeAdapterConfig<GrpcLocationModel, LocationGRPC>
            .NewConfig()
            .Map(dest => dest.ID, src => src.Id)
            .Map(dest => dest.Hash, src => src.Hash)
            .Map(dest => dest.Name, src => src.Name);

        TypeAdapterConfig<GrpcStatusModel, StatusGRPC>
            .NewConfig()
            .Map(dest => dest.ID, src => src.Id)
            .Map(dest => dest.Type, src => src.Type);

        //  gRPC client mapping for Measurement

        TypeAdapterConfig<MeasurementGrpcModel, MeasurementGRPC>
            .NewConfig()
            .Map(x => x.ID, y => y.Id)
            .Map(x => x.DeviceNumber, y => y.DeviceNumber)
            .Map(x => x.MeasurementCaptureDate, y => y.MeasurementCaptureDate)
            .Map(x => x.LocationHash, y => y.LocationHash)
            .Map(x => x.Temperature, y => y.Temperature)
            .Map(x => x.Humidity, y => y.Humidity)
            .Map(x => x.CarbonDioxide, y => y.Co2)
            .Map(x => x.VolatileOrganicCompounds, y => y.Voc)
            .Map(x => x.ParticulateMatter1, y => y.ParticulateMatter1)
            .Map(x => x.ParticulateMatter2v5, y => y.ParticulateMatter2V5)
            .Map(x => x.ParticulateMatter10, y => y.ParticulateMatter10)
            .Map(x => x.Formaldehyde, y => y.Formaldehyde)
            .Map(x => x.CarbonMonoxide, y => y.Co)
            .Map(x => x.Ozone, y => y.O3)
            .Map(x => x.Ammonia, y => y.Ammonia)
            .Map(x => x.Airflow, y => y.Airflow)
            .Map(x => x.AirIonizationLevel, y => y.AirIonizationLevel)
            .Map(x => x.Oxygen, y => y.O2)
            .Map(x => x.Radon, y => y.Radon)
            .Map(x => x.Illuminance, y => y.Illuminance)
            .Map(x => x.SoundLevel, y => y.SoundLevel);

        TypeAdapterConfig<MeasurementGRPC, MeasurementGrpcModel>
            .NewConfig()
            .Map(x => x.Id, y => y.ID)
            .Map(x => x.DeviceNumber, y => y.DeviceNumber)
            .Map(dest => dest.MeasurementCaptureDate, src => src.MeasurementCaptureDate.ToString("o"))
            .Map(x => x.LocationHash, y => y.LocationHash)
            .Map(x => x.Temperature, y => y.Temperature)
            .Map(x => x.Humidity, y => y.Humidity)
            .Map(x => x.Co2, y => y.CarbonDioxide)
            .Map(x => x.Voc, y => y.VolatileOrganicCompounds)
            .Map(x => x.ParticulateMatter1, y => y.ParticulateMatter1)
            .Map(x => x.ParticulateMatter2V5, y => y.ParticulateMatter2v5)
            .Map(x => x.ParticulateMatter10, y => y.ParticulateMatter10)
            .Map(x => x.Formaldehyde, y => y.Formaldehyde)
            .Map(x => x.Co, y => y.CarbonMonoxide)
            .Map(x => x.O3, y => y.Ozone)
            .Map(x => x.Ammonia, y => y.Ammonia)
            .Map(x => x.Airflow, y => y.Airflow)
            .Map(x => x.AirIonizationLevel, y => y.AirIonizationLevel)
            .Map(x => x.O2, y => y.Oxygen)
            .Map(x => x.Radon, y => y.Radon)
            .Map(x => x.Illuminance, y => y.Illuminance)
            .Map(x => x.SoundLevel, y => y.SoundLevel);


        return services;
    }
}
