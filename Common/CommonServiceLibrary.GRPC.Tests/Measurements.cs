using CommonServiceLibrary.GRPC.Client;
using CommonServiceLibrary.GRPC.Entities;
using Microsoft.Extensions.Configuration;

namespace CommonServiceLibrary.GRPC.Tests;

public class Measurements
{
    private readonly IConfiguration Config;
    public Measurements()
    {
        var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddUserSecrets<Devices>();

        Config = builder.Build();
    }

    [Fact]
    public async Task GetAllMeasurementsFromDay()
    {
        CommonClientGRPC client = new CommonClientGRPC(Config);

        DateTime date = DateTime.Parse("2025-06-14 00:00:00");

        IEnumerable<MeasurementSetGRPC>? measurements = await client.Measurements.GetAllMeasurementsFromDay(date);

        List<MeasurementSetGRPC> devicesList = measurements.ToList();

        Assert.Equal(214, measurements.Count());
    }

    [Fact]
    public async Task GetAllMeasurementsFromWeek()
    {
        CommonClientGRPC client = new CommonClientGRPC(Config);

        DateTime date = DateTime.Parse("2025-06-24 00:00:00");

        IEnumerable<MeasurementSetGRPC>? measurements = await client.Measurements.GetAllMeasurementsFromWeek(date);

        List<MeasurementSetGRPC> devicesList = measurements.ToList();

        Assert.Equal(1484, measurements.Count());
    }

    [Fact]
    public async Task GetAllMeasurementsFromMonth()
    {
        CommonClientGRPC client = new CommonClientGRPC(Config);

        DateTime date = DateTime.Parse("2025-06-24 00:00:00");

        IEnumerable<MeasurementSetGRPC>? measurements = await client.Measurements.GetAllMeasurementsFromMonth(date);

        List<MeasurementSetGRPC> devicesList = measurements.ToList();

        Assert.Equal(2095, measurements.Count());
    }

    [Fact]
    public async Task GetAllMeasurementsFromDeviceFromDay()
    {
        CommonClientGRPC client = new CommonClientGRPC(Config);

        DateTime date = DateTime.Parse("2025-06-14 00:00:00");
        Guid deviceNumber = new Guid("83f17437-0048-416e-a57a-a8a13a06a1df");

        IEnumerable<MeasurementSetGRPC>? measurements = await client.Measurements.GetAllMeasurementsFromDay(deviceNumber, date);

        List<MeasurementSetGRPC> devicesList = measurements.ToList();

        Assert.Equal(24, measurements.Count());
    }

    [Fact]
    public async Task GetAllMeasurementsFromDeviceFromWeek()
    {
        CommonClientGRPC client = new CommonClientGRPC(Config);

        DateTime date = DateTime.Parse("2025-06-24 00:00:00");
        Guid deviceNumber = new Guid("83f17437-0048-416e-a57a-a8a13a06a1df");

        IEnumerable<MeasurementSetGRPC>? measurements = await client.Measurements.GetAllMeasurementsFromWeek(deviceNumber, date);

        List<MeasurementSetGRPC> devicesList = measurements.ToList();

        Assert.Equal(168, measurements.Count());
    }

    [Fact]
    public async Task GetAllMeasurementsFromDeviceFromMonth()
    {
        CommonClientGRPC client = new CommonClientGRPC(Config);

        DateTime date = DateTime.Parse("2025-06-24 00:00:00");
        Guid deviceNumber = new Guid("83f17437-0048-416e-a57a-a8a13a06a1df");

        IEnumerable<MeasurementSetGRPC>? measurements = await client.Measurements.GetAllMeasurementsFromMonth(deviceNumber, date);

        List<MeasurementSetGRPC> devicesList = measurements.ToList();

        Assert.Equal(240, measurements.Count());
    }
}