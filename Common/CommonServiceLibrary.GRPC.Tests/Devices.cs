using CommonServiceLibrary.GRPC.Client;
using CommonServiceLibrary.GRPC.Entities;
using Microsoft.Extensions.Configuration;

namespace CommonServiceLibrary.GRPC.Tests;

public class Devices
{
    private readonly IConfiguration Config;
    public Devices()
    {
        var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddUserSecrets<Devices>();

        Config = builder.Build();
    }

    [Fact]
    public async Task GetAllDevices()
    {
        CommonClientGRPC client = new CommonClientGRPC(Config);
        var devices = await client.Devices.GetAllDevices();

        List<DeviceGRPC> devicesList = devices.ToList();

        Assert.Equal(9, devices.Count());
    }

    [Fact]
    public async Task GetDeviceByDeviceNumber()
    {
        CommonClientGRPC client = new CommonClientGRPC(Config);

        Guid deviceNumber = new Guid("B9558A38-8F92-48B0-8530-42D258B710C8");

        DeviceGRPC device = await client.Devices.GetDeviceByDeviceNumber(deviceNumber);

        Assert.NotNull(device);
    }
}
