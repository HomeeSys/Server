using OpenAI.Chat;

namespace Raports.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config, IWebHostEnvironment environment)
    {
        //  Configure Azure Blob Storage
        string azureBloblStorageConnectionString = config.GetConnectionString("AzureBlobStorage");

        string azureBlobStorageContainerName = environment.IsProduction()
            ? config["AzureBlob:ContainerName_Prod"]
            : config["AzureBlob:ContainerName_Dev"];

        var blobServiceClient = new BlobServiceClient(azureBloblStorageConnectionString);
        var blobContainerClient = blobServiceClient.GetBlobContainerClient(azureBlobStorageContainerName);
        services.AddSingleton(blobServiceClient);
        services.AddSingleton(blobContainerClient);

        //  Configure OpenAI
        var model = config["OpenAI:Model"];
        var openAIApiKey = config["OpenAI:API_KEY"];
        var openAIClient = new ChatClient(model, openAIApiKey);

        services.AddSingleton(openAIClient);

        //  Configure Database
        services.AddDbContext<RaportsDBContext>(options =>
        {
            string databaseConnectionString = string.Empty;
            if (environment.IsDevelopment())
            {
                databaseConnectionString = config.GetConnectionString("RaportsDB_Dev");
            }
            else if (environment.IsStaging())
            {
                databaseConnectionString = config.GetConnectionString("RaportsDB_Dev");
            }
            else
            {
                databaseConnectionString = config.GetConnectionString("RaportsDB_Prod");
            }

            options.UseSqlServer(databaseConnectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null
                );
            });
        });

        return services;
    }
}
