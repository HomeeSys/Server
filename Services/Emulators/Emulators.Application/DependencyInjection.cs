namespace Emulators.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
        });

        services.AddMemoryCache();

        services.AddQuartz(options =>
        {
            options.AddJob<EnqueueMeasurementGenerationJob>(x => x.StoreDurably().WithIdentity(nameof(EnqueueMeasurementGenerationJob)));

            options.UsePersistentStore(persistanceOptions =>
            {
                persistanceOptions.UseSqlServer(config =>
                {
                    if (environment.IsProduction())
                    {
                        config.ConnectionString = configuration.GetConnectionString("EmulatorsDB_Prod");
                    }
                    else
                    {
                        config.ConnectionString = configuration.GetConnectionString("EmulatorsDB_Dev");
                    }
                });

                persistanceOptions.UseNewtonsoftJsonSerializer();
            });
        });

        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

        services.AddMassTransit(config =>
        {
            config.AddConsumer<DeviceActivatedConsumer>();
            config.AddConsumer<DeviceCreatedConsumer>();
            config.AddConsumer<DeviceDeletedConsumer>();
            config.AddConsumer<DeviceUpdatedConsumer>();
            config.AddConsumer<DeviceStatusChangedConsumer>();
            config.AddConsumer<DeviceGenerateMeasurementConsumer>();

            config.UsingAzureServiceBus((context, configurator) =>
            {
                configurator.Host(configuration.GetConnectionString("AzureServiceBus"));

                // Map message types to existing topics
                if (environment.IsProduction())
                {
                    //  Consumers
                    configurator.Message<DeviceActivated>(x => x.SetEntityName("homee.deviceactivated.prod"));
                    configurator.Message<DeviceCreated>(x => x.SetEntityName("homee.devicecreatedtopic.prod"));
                    configurator.Message<DeviceDeleted>(x => x.SetEntityName("homee.devicedeletedtopic.prod"));
                    configurator.Message<DeviceUpdated>(x => x.SetEntityName("homee.deviceupdatedtopic.prod"));
                    configurator.Message<DeviceStatusChanged>(x => x.SetEntityName("homee.devicestatuschangedtopic.prod"));

                    //  Producers
                    configurator.Message<CreateMeasurement>(x => x.SetEntityName("homee.createmeasurement.prod"));
                    configurator.Message<ActivateDevices>(x => x.SetEntityName("homee.activatedevices.prod"));

                    //  Producer/Consumer
                    configurator.Message<DeviceGenerateMeasurement>(x => x.SetEntityName("homee.devicegeneratemeasurement.prod"));
                }
                else
                {
                    //  Consumers
                    configurator.Message<DeviceActivated>(x => x.SetEntityName("homee.deviceactivated.dev"));
                    configurator.Message<DeviceCreated>(x => x.SetEntityName("homee.devicecreatedtopic.dev"));
                    configurator.Message<DeviceDeleted>(x => x.SetEntityName("homee.devicedeletedtopic.dev"));
                    configurator.Message<DeviceUpdated>(x => x.SetEntityName("homee.deviceupdatedtopic.dev"));
                    configurator.Message<DeviceStatusChanged>(x => x.SetEntityName("homee.devicestatuschangedtopic.dev"));

                    //  Producers
                    configurator.Message<CreateMeasurement>(x => x.SetEntityName("homee.createmeasurement.dev"));
                    configurator.Message<ActivateDevices>(x => x.SetEntityName("homee.activatedevices.dev"));

                    //  Producer/Consumer
                    configurator.Message<DeviceGenerateMeasurement>(x => x.SetEntityName("homee.devicegeneratemeasurement.dev"));
                }

                configurator.PublishTopology.GetMessageTopology<CreateMeasurement>().EnablePartitioning = false;
                configurator.PublishTopology.GetMessageTopology<ActivateDevices>().EnablePartitioning = false;

                // Subscription endpoints
                configurator.SubscriptionEndpoint<DeviceActivated>("emulator-device-activated", e =>
                {
                    e.ConfigureConsumer<DeviceActivatedConsumer>(context);
                    e.ConfigureConsumeTopology = false;
                });

                configurator.SubscriptionEndpoint<DeviceCreated>("emulator-device-created", e =>
                {
                    e.ConfigureConsumer<DeviceCreatedConsumer>(context);
                    e.ConfigureConsumeTopology = false;
                });

                configurator.SubscriptionEndpoint<DeviceUpdated>("emulator-device-updated", e =>
                {
                    e.ConfigureConsumer<DeviceUpdatedConsumer>(context);
                    e.ConfigureConsumeTopology = false;
                });

                configurator.SubscriptionEndpoint<DeviceStatusChanged>("emulator-device-status-changed", e =>
                {
                    e.ConfigureConsumer<DeviceStatusChangedConsumer>(context);
                    e.ConfigureConsumeTopology = false;
                });

                configurator.SubscriptionEndpoint<DeviceDeleted>("emulator-device-deleted", e =>
                {
                    e.ConfigureConsumer<DeviceDeletedConsumer>(context);
                    e.ConfigureConsumeTopology = false;
                });

                configurator.SubscriptionEndpoint<DeviceGenerateMeasurement>("emulator-device-measurement-generation-enqueued", e =>
                {
                    e.ConfigureConsumer<DeviceGenerateMeasurementConsumer>(context);
                    e.ConfigureConsumeTopology = false;
                });

            });
        });

        services.AddHostedService<StartupPublisher>();

        return services;
    }
}
