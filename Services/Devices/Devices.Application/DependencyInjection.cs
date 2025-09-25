using CommonServiceLibrary.Messaging;
using Devices.Application.Hubs;
using Devices.Application.Mappers;
using Microsoft.Extensions.Configuration;

namespace Devices.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCarter();

            services.AddMediatR(x =>
            {
                x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                x.AddOpenBehavior(typeof(ValidationBehavior<,>));
                x.AddOpenBehavior(typeof(LoggingBehavior<,>));
            });

            var config = new TypeAdapterConfig();

            config.Apply(new MeasurementConfigMapper());

            services.AddSingleton(config);

            services.AddExceptionHandler<CustomExceptionHandler>();

            services.AddMessageBroker(configuration, Assembly.GetExecutingAssembly());

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
}
