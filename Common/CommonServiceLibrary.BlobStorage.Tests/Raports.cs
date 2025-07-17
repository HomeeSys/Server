using Microsoft.Extensions.Configuration;

namespace CommonServiceLibrary.BlobStorage.Tests;

public class Raports
{
    private readonly IConfiguration Config;
    public Raports()
    {
        var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddUserSecrets<Raports>();

        Config = builder.Build();
    }
    [Fact]
    public async Task RetrieveItemsAsync()
    {
        RaportContainer container = new RaportContainer(Config);
        Assert.NotNull(container);

        var items = await container.GetExistingRaportsNames();
    }
}
