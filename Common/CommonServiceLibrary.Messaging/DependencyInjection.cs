using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

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

            config.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(new Uri(configuration["MessageBroker:Host"]!), hostConfig =>
                {
                    hostConfig.Username(configuration["MessageBroker.UserName"]!);
                    hostConfig.Username(configuration["MessageBroker.Password"]!);
                });

                configurator.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}