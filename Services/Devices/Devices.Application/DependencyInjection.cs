namespace Devices.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        TypeAdapterConfig<LocationDTO, Location>
            .NewConfig()
            .Map(x => x.ID, y => y.ID)
            .Map(x => x.Name, y => y.Name);

        TypeAdapterConfig<Location, LocationDTO>
            .NewConfig()
            .Map(x => x.ID, y => y.ID)
            .Map(x => x.Name, y => y.Name);

        TypeAdapterConfig<TimestampDTO, Timestamp>
            .NewConfig()
            .Map(x => x.ID, y => y.ID)
            .Map(x => x.Cron, y => y.Cron);

        TypeAdapterConfig<Timestamp, TimestampDTO>
            .NewConfig()
            .Map(x => x.ID, y => y.ID)
            .Map(x => x.Cron, y => y.Cron);

        TypeAdapterConfig<DefaultDeviceDTO, Device>
            .NewConfig()
            .Map(x => x.ID, y => y.ID);

        TypeAdapterConfig<Device, DefaultDeviceDTO>
            .NewConfig()
            .Map(x => x.ID, y => y.ID);

        TypeAdapterConfig<DefaultMeasurementTypeDTO, MeasurementType>
            .NewConfig()
            .Map(x => x.Name, y => y.Name)
            .Map(x => x.Unit, y => y.Unit)
            .Map(x => x.ID, y => y.ID);

        TypeAdapterConfig<MeasurementType, DefaultMeasurementTypeDTO>
            .NewConfig()
            .Map(x => x.Name, y => y.Name)
            .Map(x => x.Unit, y => y.Unit)
            .Map(x => x.ID, y => y.ID);

        TypeAdapterConfig<StatusDTO, Status>
            .NewConfig()
            .Map(x => x.ID, y => y.ID);

        TypeAdapterConfig<Status, StatusDTO>
            .NewConfig()
            .Map(x => x.ID, y => y.ID);

        services.AddCarter();

        services.AddMediatR(x =>
        {
            x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            x.AddOpenBehavior(typeof(ValidationBehavior<,>));
            x.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        services.AddExceptionHandler<CustomExceptionHandler>();

        //  MassTransit - Azure Service Bus
        services.AddMassTransit(config =>
        {
            config.AddConsumer<ActivateDevicesConsumer>();

            config.UsingAzureServiceBus((context, configurator) =>
            {
                configurator.Host(configuration.GetConnectionString("AzureServiceBus"));

                //  Topics
                if (environment.IsProduction())
                {
                    //  Consumers
                    configurator.Message<ActivateDevices>(x => x.SetEntityName("homee.activatedevices.prod"));

                    //  Producers
                    configurator.Message<DeviceActivated>(x => x.SetEntityName("homee.deviceactivated.prod"));
                    configurator.Message<DeviceCreated>(x => x.SetEntityName("homee.devicecreatedtopic.prod"));
                    configurator.Message<DeviceDeleted>(x => x.SetEntityName("homee.devicedeletedtopic.prod"));
                    configurator.Message<DeviceUpdated>(x => x.SetEntityName("homee.deviceupdatedtopic.prod"));
                    configurator.Message<DeviceStatusChanged>(x => x.SetEntityName("homee.devicestatuschangedtopic.prod"));
                }
                else
                {
                    //  Consumers
                    configurator.Message<ActivateDevices>(x => x.SetEntityName("homee.activatedevices.dev"));

                    //  Producers
                    configurator.Message<DeviceActivated>(x => x.SetEntityName("homee.deviceactivated.dev"));
                    configurator.Message<DeviceCreated>(x => x.SetEntityName("homee.devicecreatedtopic.dev"));
                    configurator.Message<DeviceDeleted>(x => x.SetEntityName("homee.devicedeletedtopic.dev"));
                    configurator.Message<DeviceUpdated>(x => x.SetEntityName("homee.deviceupdatedtopic.dev"));
                    configurator.Message<DeviceStatusChanged>(x => x.SetEntityName("homee.devicestatuschangedtopic.dev"));
                }

                configurator.PublishTopology.GetMessageTopology<DeviceActivated>().EnablePartitioning = false;
                configurator.PublishTopology.GetMessageTopology<DeviceCreated>().EnablePartitioning = false;
                configurator.PublishTopology.GetMessageTopology<DeviceDeleted>().EnablePartitioning = false;
                configurator.PublishTopology.GetMessageTopology<DeviceUpdated>().EnablePartitioning = false;
                configurator.PublishTopology.GetMessageTopology<DeviceStatusChanged>().EnablePartitioning = false;

                //  Subscriptions
                configurator.SubscriptionEndpoint<ActivateDevices>("devices-activate-devices", e =>
                {
                    e.ConfigureConsumer<ActivateDevicesConsumer>(context);
                    e.ConfigureConsumeTopology = false;
                });

                configurator.ConfigureEndpoints(context);
            });
        });

        services.AddSignalR();

        return services;
    }

    public static WebApplication AddApplicationServicesUsage(this WebApplication app)
    {
        app.MapCarter();

        app.UseExceptionHandler(x => { });

        app.MapHub<DeviceHub>("/devicehub");

        return app;
    }
}
