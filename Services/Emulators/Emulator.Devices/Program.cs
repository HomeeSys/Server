MapingConfiguration.Configure();

DevicesManager manager = new DevicesManager();
await manager.InitializeAsync();
await manager.WaitForMicroservicesAsync();
await manager.RefreshDevicesAsync();

TypeAdapterConfig<MeasurementSetModel, MeasurementSetDTO>
    .NewConfig()
    .Map(x => x.CO, y => y.CO)
    .Map(x => x.ParticulateMatter2v5, y => y.ParticulateMatter2v5)
    .Map(x => x.VOC, y => y.VOC);

var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
{
    cfg.Host("rabbitmq", "/", h =>
    {
        h.Username("guest");
        h.Password("guest");
    });

    cfg.ReceiveEndpoint("device-status-message", e =>
    {
        e.Handler<DeviceStatusChangedMessage>(async ctx =>
        {
            var model = ctx.Message.Payload.Adapt<DeviceModel>();

            await manager.DeviceStatusChange_HandlerAsync(model);
        });

        e.Handler<DeviceCreatedMessage>(async ctx =>
        {
            var model = ctx.Message.NewDevice.Adapt<DeviceModel>();

            await manager.DeviceAdded_HandlerAsync(model);
        });

        e.Handler<DeviceDeletedMessage>(async ctx =>
        {
            var model = ctx.Message.DeletedDevice.Adapt<DeviceModel>();

            await manager.DeviceDeleted_HandlerAsync(model);
        });
    });
});

await busControl.StartAsync();
Console.WriteLine("Bus started. Listening for messages...");

try
{
    Console.WriteLine("Press any key to exit");
    await Task.Run(() => Console.Read());
}
finally
{
    await busControl.StopAsync();
}