namespace Measurements.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        var endpoint = configuration["ConnectionStrings:CosmosDB_Endpoint"];
        var key = configuration["ConnectionStrings:CosmosDB_Key"];
        var databaseName = configuration["ConnectionStrings:CosmosDB_Database"];

        var containerName = environment.IsProduction()
            ? configuration["ConnectionStrings:CosmosDB_ContainerProd"]
            : configuration["ConnectionStrings:CosmosDB_ContainerDev"];

        var cosmosClient = new CosmosClient(endpoint, key, new CosmosClientOptions
        {
            ConnectionMode = environment.IsProduction() ? ConnectionMode.Direct : ConnectionMode.Gateway,
            SerializerOptions = new CosmosSerializationOptions
            {
                PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
            }
        });

        var container = cosmosClient.GetContainer(databaseName, containerName);

        services.AddSingleton(cosmosClient);
        services.AddSingleton(container);

        return services;
    }
}