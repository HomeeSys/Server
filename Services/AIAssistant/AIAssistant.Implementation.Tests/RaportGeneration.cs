using AIAssistant.Implementation.General;
using Microsoft.Extensions.Configuration;

namespace AIAssistant.Implementation.Tests;

public class RaportGeneration
{
    private readonly IConfiguration _configuration;
    public RaportGeneration()
    {
        var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddUserSecrets<RaportGeneration>();

        _configuration = builder.Build();
    }

    [Fact]
    public async Task HelloWorldAsync()
    {
        OllamaRaportChat model = new OllamaRaportChat(_configuration);

        string response = await model.HelloWorld();

        Assert.NotNull(response);
    }

    [Fact]
    public async Task DailyReportDescription()
    {
        OllamaRaportChat model = new OllamaRaportChat(_configuration);

        string data = "{\"MeasurementType\":\"Temperatures\",\"Time\":[\"2025-06-14T00:00:00\",\"2025-06-14T01:00:00\",\"2025-06-14T02:00:00\",\"2025-06-14T03:00:00\",\"2025-06-14T04:00:00\",\"2025-06-14T05:00:00\",\"2025-06-14T06:00:00\",\"2025-06-14T07:00:00\",\"2025-06-14T08:00:00\",\"2025-06-14T09:00:00\",\"2025-06-14T10:00:00\",\"2025-06-14T11:00:00\",\"2025-06-14T12:00:00\",\"2025-06-14T13:00:00\",\"2025-06-14T14:00:00\",\"2025-06-14T15:00:00\",\"2025-06-14T16:00:00\",\"2025-06-14T17:00:00\",\"2025-06-14T18:00:00\",\"2025-06-14T19:00:00\",\"2025-06-14T20:00:00\",\"2025-06-14T21:00:00\",\"2025-06-14T22:00:00\",\"2025-06-14T23:00:00\"],\"Measurements\":[{\"Name\":\"Living room\",\"Data\":[22.813120281741462,21.53733382595747,21.452823400681286,20.466864966325375,22.34631836179205,21.401579784729122,21.364174530811372,21.609006791524372,23.933846825132388,20.931732924211985,22.533716059705565,20.85559985509928,23.465287021824857,21.073352857297486,21.715608822270557,20.8293751265858,22.52262392153989,21.180864859147178,23.570948550964847,22.821620047066546,23.67428388159204,23.04958895481139,22.76221009566965,20.45289524313639]},{\"Name\":\"Kitchen\",\"Data\":[22.724623318120916,22.003810210206183,20.61595629050464,21.571800041896957,21.90911559429828,19.482787674236878,22.53096107721977,22.349476181581686,23.461255352697666,23.739712633165333,21.92045125986393,20.884234865886576,23.86389547434074,22.44341764810645,20.806629480119916,21.875904470444365,20.16924027626732,21.995922163064094,22.82670614170255,22.552434279913818,20.69935232427854,21.38218589342292,21.823500817451002,23.56586008956434]}]}";

        string response = await model.GenerateDescription(data);

        Assert.NotNull(response);

        Console.WriteLine(response);
    }
}
