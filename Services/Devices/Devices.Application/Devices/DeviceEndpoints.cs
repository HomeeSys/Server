namespace Devices.Application.Devices
{
    public class DeviceEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/devices/all", async (ISender sender) =>
            {
                GetAllDevicesResponse response = await sender.Send(new GetDeviceCommands());

                IEnumerable<Device> models = response.Devices;

                IEnumerable<DeviceDTO> dtos = models.Adapt<IEnumerable<DeviceDTO>>();

                return Results.Ok(dtos);
            });

            app.MapGet("/devices/{id}", async (int id, ISender sender) =>
            {
                GetDeviceResponse response = await sender.Send(new GetDeviceByIDCommand(id));

                Device model = response.Device;

                DeviceDTO dto = model.Adapt<DeviceDTO>();

                return Results.Ok(dto);
            });

            app.MapGet("/devices/devicenumber/{devicenumber}", async (Guid devicenumber, ISender sender) =>
            {
                GetDeviceResponse response = await sender.Send(new GetDeviceByDeviceNumberCommand(devicenumber));

                Device model = response.Device;

                DeviceDTO dto = model.Adapt<DeviceDTO>();

                return Results.Ok(dto);
            });

            app.MapPost("/devices", async (RegisterDeviceDTO registerDeviceDTO, ISender sender) =>
            {
                CreateDeviceResponse response = await sender.Send(new CreateDeviceCommand(registerDeviceDTO));

                Device model = response.Device;

                DeviceDTO dto = model.Adapt<DeviceDTO>();

                return Results.Ok(dto);
            });

            app.MapPut("/devices/{devicenumber}", async (Guid devicenumber, UpdateDeviceDTO body, ISender sender) =>
            {
                GetDeviceResponse response = await sender.Send(new UpdateDeviceCommand(devicenumber, body));

                Device model = response.Device;

                DeviceDTO dto = model.Adapt<DeviceDTO>();

                return Results.Ok(dto);
            });

            app.MapDelete("/devices/{devicenumber}", async (Guid devicenumber, ISender sender) =>
            {
                DeleteDeviceCommand command = new DeleteDeviceCommand(devicenumber);

                GetDeviceResponse response = await sender.Send(command);

                Device model = response.Device;

                DeviceDTO dto = model.Adapt<DeviceDTO>();

                return Results.Ok(dto);
            });
        }
    }
}