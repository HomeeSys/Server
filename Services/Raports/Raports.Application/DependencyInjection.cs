using CommonServiceLibrary.Messaging;
using Raports.Infrastructure.Generators;

namespace Raports.Application
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

            services.AddScoped<RaportContainer>();
            services.AddScoped<MeasurementPacketGenerator>();

            services.AddMessageBroker(configuration, Assembly.GetExecutingAssembly());

            services.AddExceptionHandler<CustomExceptionHandler>();

            return services;
        }

        public static WebApplication UseApplicationServices(this WebApplication app)
        {
            app.MapCarter();

            app.UseExceptionHandler(x => { });

            return app;
        }
    }
}
