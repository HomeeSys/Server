namespace Measurements.GRPCServer.Mappings;

internal class MeasurementMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        //  gRPC server mapping for Measurement

        TypeAdapterConfig<Measurement, MeasurementGrpcModel>
            .NewConfig()
            .Map(dest => dest.Id, src => src.ID)
            .Map(dest => dest.DeviceNumber, src => src.DeviceNumber)
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

        TypeAdapterConfig<MeasurementGrpcModel, Measurement>
            .NewConfig()
            .Map(dest => dest.ID, src => src.Id)
            .Map(dest => dest.DeviceNumber, src => src.DeviceNumber)
            .Map(dest => dest.MeasurementCaptureDate, src => src.MeasurementCaptureDate)
            .Map(dest => dest.LocationHash, src => src.LocationHash)

            .Map(dest => dest.Temperature, src => src.Temperature)
            .Map(dest => dest.Humidity, src => src.Humidity)
            .Map(dest => dest.CarbonDioxide, src => src.Co2)
            .Map(dest => dest.VolatileOrganicCompounds, src => src.Voc)
            .Map(dest => dest.ParticulateMatter1, src => src.ParticulateMatter1)
            .Map(dest => dest.ParticulateMatter2v5, src => src.ParticulateMatter2V5)
            .Map(dest => dest.ParticulateMatter10, src => src.ParticulateMatter10)
            .Map(dest => dest.Formaldehyde, src => src.Formaldehyde)
            .Map(dest => dest.CarbonMonoxide, src => src.Co)
            .Map(dest => dest.Ozone, src => src.O3)
            .Map(dest => dest.Ammonia, src => src.Ammonia)
            .Map(dest => dest.Airflow, src => src.Airflow)
            .Map(dest => dest.AirIonizationLevel, src => src.AirIonizationLevel)
            .Map(dest => dest.Oxygen, src => src.O2)
            .Map(dest => dest.Radon, src => src.Radon)
            .Map(dest => dest.Illuminance, src => src.Illuminance)
            .Map(dest => dest.SoundLevel, src => src.SoundLevel);

    }
}
