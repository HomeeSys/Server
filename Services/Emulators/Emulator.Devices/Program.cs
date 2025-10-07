MapingConfiguration.Configure();

DevicesManager manager = new DevicesManager();
await manager.InitializeAsync();
await manager.WaitForMicroservicesAsync();
await manager.RefreshDevicesAsync();

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

    //cfg.ReceiveEndpoint("device-added-message", e =>
    //{
    //    e.Handler<DeviceStatusChangedMessage>(async ctx =>
    //    {
    //        var model = ctx.Message.Payload.Adapt<DeviceModel>();

    //        await manager.DeviceAdded_HandlerAsync(model);
    //    });
    //});

    //cfg.ReceiveEndpoint("device-removed-message", e =>
    //{
    //    e.Handler<DeviceStatusChangedMessage>(ctx =>
    //    {
    //        var model = ctx.Message.Payload.Adapt<DeviceModel>();

    //        manager.DeviceDeleted_HandlerAsync(model);

    //        return Task.CompletedTask;
    //    });
    //});

    //cfg.ReceiveEndpoint("device-updated-message", e =>
    //{
    //    e.Handler<DeviceStatusChangedMessage>(async ctx =>
    //    {
    //        var model = ctx.Message.Payload.Adapt<DeviceModel>();

    //        await manager.DeviceUpdated_HandlerAsync(model);
    //    });
    //});
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