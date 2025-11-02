namespace Raports.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCarter();

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddMediatR(x =>
            {
                x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                x.AddOpenBehavior(typeof(ValidationBehavior<,>));
                x.AddOpenBehavior(typeof(LoggingBehavior<,>));
            });

            var config = new TypeAdapterConfig();

            config.Apply(new PeriodMapper());
            config.Apply(new RequestsStatusMapper());
            config.Apply(new RequestMapper());
            config.Apply(new RaportMapper());

            services.AddSingleton(config);

            services.AddScoped<RaportContainer>();
            services.AddScoped<MeasurementPacketGenerator>();

            services.AddMessageBroker(configuration, Assembly.GetExecutingAssembly());

            services.AddExceptionHandler<CustomExceptionHandler>();

            services.AddSignalR();

            return services;
        }

        public static WebApplication UseApplicationServices(this WebApplication app)
        {
            app.MapCarter();

            app.MapHub<RaportsHub>("/raportshub");

            app.UseExceptionHandler(x => { });

            return app;
        }
    }
}
