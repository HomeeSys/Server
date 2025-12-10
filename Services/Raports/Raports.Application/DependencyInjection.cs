using CommonServiceLibrary.GRPC;

namespace Raports.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddCarter();

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddMediatR(x =>
        {
            x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            x.AddOpenBehavior(typeof(ValidationBehavior<,>));
            x.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        services.AddGRPCMappings();
        
        var measurementsGrpcClient = services.AddGrpcClient<MeasurementService.MeasurementServiceClient>(options =>
        {
            string grpcConnectionString = string.Empty;
            if (environment.IsDevelopment())
            {
                grpcConnectionString = configuration.GetConnectionString("MeasurementsGRPC_Dev");
            }
            else if (environment.IsStaging())
            {
                grpcConnectionString = configuration.GetConnectionString("MeasurementsGRPC_Dev");
            }
            else
            {
                grpcConnectionString = configuration.GetConnectionString("MeasurementsGRPC_Prod");
            }

            options.Address = new Uri(grpcConnectionString);
        });

        var devicesGrpcClient = services.AddGrpcClient<DevicesService.DevicesServiceClient>(options =>
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
        });

        // Configure HTTP message handlers based on environment
        if (environment.IsDevelopment())
        {
            measurementsGrpcClient.ConfigurePrimaryHttpMessageHandler(() =>
            {
                return new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
            });

            devicesGrpcClient.ConfigurePrimaryHttpMessageHandler(() =>
            {
                return new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
            });
            
            // Configure both clients to use HTTP/2 for local development
            measurementsGrpcClient.ConfigureHttpClient(client =>
            {
                client.DefaultRequestVersion = new Version(2, 0);
                client.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrHigher;
            });

            devicesGrpcClient.ConfigureHttpClient(client =>
            {
                client.DefaultRequestVersion = new Version(2, 0);
                client.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrHigher;
            });
        }
        else
        {
            // Production/Staging: Use gRPC-Web for Azure App Service compatibility
            measurementsGrpcClient.ConfigurePrimaryHttpMessageHandler(() =>
            {
                var httpHandler = new HttpClientHandler();
                return new Grpc.Net.Client.Web.GrpcWebHandler(Grpc.Net.Client.Web.GrpcWebMode.GrpcWeb, httpHandler);
            });

            devicesGrpcClient.ConfigurePrimaryHttpMessageHandler(() =>
            {
                var httpHandler = new HttpClientHandler();
                return new Grpc.Net.Client.Web.GrpcWebHandler(Grpc.Net.Client.Web.GrpcWebMode.GrpcWeb, httpHandler);
            });
            
            // Use HTTP/1.1 for gRPC-Web
            measurementsGrpcClient.ConfigureHttpClient(client =>
            {
                client.DefaultRequestVersion = new Version(1, 1);
            });

            devicesGrpcClient.ConfigureHttpClient(client =>
            {
                client.DefaultRequestVersion = new Version(1, 1);
            });
        }

        services.AddMemoryCache();

        var config = new TypeAdapterConfig();

        config.Apply(new PeriodMapper());
        config.Apply(new StatusMapper());
        config.Apply(new MeasurementMapper());
        config.Apply(new LocationMapper());
        config.Apply(new RaportMapper());

        services.AddSingleton(config);

        //  MassTransit - Azure Service Bus
        services.AddMassTransit(config =>
        {
            config.AddConsumer<ValidateRaportConsumer>();
            config.AddConsumer<RaportFailedConsumer>();
            config.AddConsumer<AdjustRaportConsumer>();
            config.AddConsumer<GenerateSummaryConsumer>();
            config.AddConsumer<GenerateDocumentConsumer>();
            config.AddConsumer<AppendRaportConsumer>();
            config.AddConsumer<ProcessRaportReady>();

            config.UsingAzureServiceBus((context, configurator) =>
            {
                configurator.Host(configuration.GetConnectionString("AzureServiceBus"));

                // Map message types to existing topics
                if (environment.IsProduction())
                {
                    //  Producer/Consumer
                    configurator.Message<RaportProduceDocument>(x => x.SetEntityName("homee.raportsdocumenttopic.prod"));
                    configurator.Message<RaportPending>(x => x.SetEntityName("homee.raportspendingtopic.prod"));
                    configurator.Message<RaportToSummary>(x => x.SetEntityName("homee.raportssummarytopic.prod"));
                    configurator.Message<RaportReady>(x => x.SetEntityName("homee.raportsgeneratedtopic.prod"));
                    configurator.Message<ValidateRaport>(x => x.SetEntityName("homee.raportsvalidatedata.prod"));
                    configurator.Message<RaportFailed>(x => x.SetEntityName("homee.raportfailed.prod"));
                    configurator.Message<AdjustRaport>(x => x.SetEntityName("homee.raportsadjust.prod"));
                    configurator.Message<GenerateSummary>(x => x.SetEntityName("homee.raportsgeneratesummary.prod"));
                    configurator.Message<GenerateDocument>(x => x.SetEntityName("homee.raportsgeneratedocument.prod"));
                }
                else
                {
                    //  Producer/Consumer
                    configurator.Message<RaportProduceDocument>(x => x.SetEntityName("homee.raportsdocumenttopic.dev"));
                    configurator.Message<RaportPending>(x => x.SetEntityName("homee.raportspendingtopic.dev"));
                    configurator.Message<RaportToSummary>(x => x.SetEntityName("homee.raportssummarytopic.dev"));
                    configurator.Message<RaportReady>(x => x.SetEntityName("homee.raportsgeneratedtopic.dev"));
                    configurator.Message<ValidateRaport>(x => x.SetEntityName("homee.raportsvalidatedata.dev"));
                    configurator.Message<RaportFailed>(x => x.SetEntityName("homee.raportfailed.dev"));
                    configurator.Message<AdjustRaport>(x => x.SetEntityName("homee.raportsadjust.dev"));
                    configurator.Message<GenerateSummary>(x => x.SetEntityName("homee.raportsgeneratesummary.dev"));
                    configurator.Message<GenerateDocument>(x => x.SetEntityName("homee.raportsgeneratedocument.dev"));
                }

                //  Subscriptions
                //  Generate document
                configurator.SubscriptionEndpoint<GenerateDocument>("raports-generate-document-subscription", e =>
                {
                    e.ConfigureConsumer<GenerateDocumentConsumer>(context);
                    e.ConfigureConsumeTopology = false;
                });

                //  Summary
                configurator.SubscriptionEndpoint<GenerateSummary>("raports-generate-summary-subscription", e =>
                {
                    e.ConfigureConsumer<GenerateSummaryConsumer>(context);
                    e.ConfigureConsumeTopology = false;
                });

                //  Adjust
                configurator.SubscriptionEndpoint<AdjustRaport>("raports-adjust-subscription", e =>
                {
                    e.ConfigureConsumer<AdjustRaportConsumer>(context);
                    e.ConfigureConsumeTopology = false;
                });

                //  Failed
                configurator.SubscriptionEndpoint<RaportFailed>("raports-failed-subscription", e =>
                {
                    e.ConfigureConsumer<RaportFailedConsumer>(context);
                    e.ConfigureConsumeTopology = false;
                });

                //  Validate
                configurator.SubscriptionEndpoint<ValidateRaport>("raports-validate-data-subscription", e =>
                {
                    e.ConfigureConsumer<ValidateRaportConsumer>(context);
                    e.ConfigureConsumeTopology = false;
                });

                //  Pending
                configurator.SubscriptionEndpoint<RaportPending>("raports-append-raport", e =>
                {
                    e.ConfigureConsumer<AppendRaportConsumer>(context);
                    e.ConfigureConsumeTopology = false;
                });

                //  Raport ready
                configurator.SubscriptionEndpoint<RaportReady>("raports-raport-ready", e =>
                {
                    e.ConfigureConsumer<ProcessRaportReady>(context);
                    e.ConfigureConsumeTopology = false;
                });
            });
        });

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
