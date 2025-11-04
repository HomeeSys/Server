namespace CommonServiceLibrary.Messaging;

public static class DependencyInjection
{
    public static IServiceCollection AddMessageBroker(this IServiceCollection services, IConfiguration configuration, Assembly assembly = null)
    {
        services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();

            if (assembly != null)
            {
                config.AddConsumers(assembly);
            }

            config.UsingAzureServiceBus((context, configurator) =>
            {
                configurator.Host(configuration.GetConnectionString("AzureServiceBus"));

                configurator.Message<GenerateDailyReportMessage>(configTopology =>
                {
                    configTopology.SetEntityName("Generate-Daily-Report-Topic");
                });
                configurator.Message<EnqueueDailyRaportGenerationMessage>(configTopology =>
                {
                    configTopology.SetEntityName("Enqueue-Daily-Raport-Generation-Topic");
                });
                configurator.Message<DeviceCreatedMessage>(configTopology =>
                {
                    configTopology.SetEntityName("Device-Created-Topic");
                });
                configurator.Message<DeviceDeletedMessage>(configTopology =>
                {
                    configTopology.SetEntityName("Device-Deleted-Topic");
                });
                configurator.Message<DeviceStatusChangedMessage>(configTopology =>
                {
                    configTopology.SetEntityName("Device-Status-Changed-Topic");
                });

                configurator.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}