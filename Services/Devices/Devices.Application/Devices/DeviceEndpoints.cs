namespace Devices.Application.Devices
{
    public class DeviceEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/devices/all", async (ISender sender) =>
            {
                GetAllDevicesResponse response = await sender.Send(new GetDeviceCommands());

                var dtos = response.DeviceDTOs;

                return Results.Ok(dtos);
            });

            app.MapGet("/devices/locations/all", async (ISender sender) =>
            {
                var response = await sender.Send(new GetAllLocationsComand());

                var models = response.Locations;

                var dtos = models.Select(x => x.Adapt<LocationDTO>());

                return Results.Ok(dtos);
            });

            app.MapGet("/devices/{id}", async (int id, ISender sender) =>
            {
                GetDeviceResponse response = await sender.Send(new GetDeviceByIDCommand(id));

                var dto = response.DeviceDTO;

                return Results.Ok(dto);
            });

            app.MapGet("/devices/devicenumber/{devicenumber}", async (Guid devicenumber, ISender sender) =>
            {
                GetDeviceResponse response = await sender.Send(new GetDeviceByDeviceNumberCommand(devicenumber));

                var dto = response.DeviceDTO;

                return Results.Ok(dto);
            });

            app.MapGet("/devices/timestampconfigurations/all", async (ISender sender) =>
            {
                GetAllTimestampConfigurationsResponse response = await sender.Send(new GetAllTimestampConfigurationsCommand());

                IEnumerable<TimestampConfiguration> models = response.Configurations;

                IEnumerable<TimestampConfigurationDTO> dtos = models.Adapt<IEnumerable<TimestampConfigurationDTO>>();

                return Results.Ok(dtos);
            });

            app.MapGet("/devices/measurementconfig/{devicenumber}", async (Guid devicenumber, ISender sender) =>
            {
                GetMeasurementConfigResponse? response = await sender.Send(new GetMeasurementConfigByDeviceNumberCommand(devicenumber));

                var dto = response.MeasurementConfigurationDTO;

                return Results.Ok(dto);
            });

            app.MapPut("/devices/measurementconfig/{deviceID}", async (int deviceID, UpdateMeasurementConfigDTO body, ISender sender) =>
            {
                GetMeasurementConfigResponse? response = await sender.Send(new UpdateDeviceMeasurementConfigCommand(deviceID, body));

                var dto = response.MeasurementConfigurationDTO;

                return Results.Ok(dto);
            });

            app.MapPost("/devices", async (RegisterDeviceDTO registerDeviceDTO, ISender sender) =>
            {
                CreateDeviceResponse response = await sender.Send(new CreateDeviceCommand(registerDeviceDTO));

                var dto = response.Device;

                return Results.Ok(dto);
            });

            app.MapPut("/devices/{devicenumber}", async (Guid devicenumber, UpdateDeviceDTO body, ISender sender) =>
            {
                GetDeviceResponse response = await sender.Send(new UpdateDeviceCommand(devicenumber, body));

                var dto = response.DeviceDTO;

                return Results.Ok(dto);
            });

            app.MapPut("/devices/status/{DeviceID}", async (int DeviceID, UpdateDeviceStatusDTO body, ISender sender) =>
            {
                GetDeviceStatusResponse response = await sender.Send(new UpdateDeviceStatusCommand(DeviceID, body.StatusType));

                UpdateDeviceStatusDTO dto = new UpdateDeviceStatusDTO(response.StatusID, response.StatusType);

                return Results.Ok(dto);
            });

            app.MapDelete("/devices/{devicenumber}", async (Guid devicenumber, ISender sender) =>
            {
                DeleteDeviceCommand command = new DeleteDeviceCommand(devicenumber);

                GetDeviceResponse response = await sender.Send(command);

                var dto = response.DeviceDTO;

                return Results.Ok(dto);
            });
        }
    }
}