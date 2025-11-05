namespace Measurements.GRPCServer;

public static class DependencyInjection
{
    public static IServiceCollection AddGRPCServerServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddGrpc();

        TypeAdapterConfig<MeasurementTMP, MeasurementGrpcModel>
            .NewConfig()
            .Map(dest => dest.Co, src => src.CO)
            .Map(dest => dest.Voc, src => src.VOC)
            .Map(dest => dest.ParticulateMatter1, src => src.ParticulateMatter1)
            .Map(dest => dest.ParticulateMatter2V5, src => src.ParticulateMatter2v5)
            .Map(dest => dest.ParticulateMatter10, src => src.ParticulateMatter10)
            .Map(dest => dest.Id, src => src.ID)
            .Map(dest => dest.Co2, src => src.CO2)
            .Map(dest => dest.RegisterDate, src => src.RegisterDate.ToString("yyyy-MM-dd HH:mm:ss"));

        return services;
    }

    public static WebApplication AddGRPCServerServicesUsage(this WebApplication app)
    {
        app.MapGrpcService<MeasurementsServerService>();

        return app;
    }
}
