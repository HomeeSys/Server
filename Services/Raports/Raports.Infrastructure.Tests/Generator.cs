using Microsoft.Extensions.Configuration;
using Raports.Infrastructure.Generators;

namespace Raports.Infrastructure.Tests
{
    public class Generator
    {
        private readonly IConfiguration _configuration;
        public Generator()
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true)
                    .AddUserSecrets<Generator>();

            _configuration = builder.Build();
        }
        [Fact]
        public async Task Daily()
        {
            try
            {
                RaportContainer container = new RaportContainer(_configuration);
                MeasurementPacketGenerator generator = new MeasurementPacketGenerator(_configuration);
                var result = await generator.ProcessDailyDataForReport(DateTime.Parse("2025-06-14 00:00:00"));

                //  Generate daily raport.
                var raport = RaportGenerator.GenerateRaport(result, DateTime.Now);

                bool wasAdded = await container.UploadDocumentAsync(raport);
                Assert.True(wasAdded);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
