using CommonServiceLibrary.GRPC;
using Measurements.Application.Mappers;

namespace Measurements.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        var mappings = new TypeAdapterConfig();

        mappings.Apply(new MeasurementMapper());

        services.AddSingleton(mappings);

        services.AddCarter();

        services.AddMediatR(x =>
        {
            x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            x.AddOpenBehavior(typeof(ValidationBehavior<,>));
            x.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        services.AddGRPCMappings();
        services.AddGrpcClient<DevicesService.DevicesServiceClient>(options =>
        {
            string grpcConnectionString = string.Empty;
            if (environment.IsDevelopment())
            {
                grpcConnectionString = configuration.GetConnectionString("DevicesGRPC_Dev");
            }
            else if (environment.IsStaging())
            {
                grpcConnectionString = configuration.GetConnectionString("DevicesGRPC_Dev");
            }
            else
            {
                grpcConnectionString = configuration.GetConnectionString("DevicesGRPC_Prod");
            }

            options.Address = new Uri(grpcConnectionString);
        })
        .ConfigurePrimaryHttpMessageHandler(() =>
        {
            return new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
        });

        //  MassTransit - Azure Service Bus
        services.AddMassTransit(config =>
        {
            config.AddConsumer<CreateMeasurementConsumer>();

            config.UsingAzureServiceBus((context, configurator) =>
            {
                configurator.Host(configuration.GetConnectionString("AzureServiceBus"));

                //  Topics
                if (environment.IsProduction())
                {
                    configurator.Message<CreateMeasurement>(x => x.SetEntityName("homee.createmeasurement.prod"));
                }
                else
                {
                    configurator.Message<CreateMeasurement>(x => x.SetEntityName("homee.createmeasurement.dev"));
                }

                //  Subscription endpoints
                configurator.SubscriptionEndpoint<CreateMeasurement>("measurements-create-measurement", e =>
                {
                    e.ConfigureConsumer<CreateMeasurementConsumer>(context);
                    e.ConfigureConsumeTopology = false;
                });
            });
        });

        services.AddExceptionHandler<CustomExceptionHandler>();

        services.AddSignalR();

        return services;
    }

    public static WebApplication AddApplicationServicesUsage(this WebApplication app)
    {
        app.MapCarter();

        app.UseExceptionHandler(x => { });

        app.MapHub<MeasurementHub>("/measurementhub");

        return app;
    }
}
