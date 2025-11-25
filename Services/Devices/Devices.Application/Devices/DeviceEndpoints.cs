namespace Devices.Application.Devices;

public class DeviceEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        //  ---------- Post ----------

        app.MapPost("/devices/device",
            async ([FromQuery] string Name,
                   [FromQuery] Guid DeviceNumber,
                   [FromQuery] int LocationID,
                   [FromQuery] int StatusID,
                   [FromQuery] int TimestampID,
                   ISender sender) =>
        {
            var response = await sender.Send(new CreateDeviceCommand(Name, DeviceNumber, LocationID, StatusID, TimestampID));
            var dto = response.DeviceDTO;

            return Results.Ok(dto);
        });

        //  ---------- Get All ----------

        app.MapGet("/devices/devices/all", async (ISender sender) =>
        {
            var response = await sender.Send(new GetDeviceCommands());
            var dtos = response.DeviceDTOs;

            return Results.Ok(dtos);
        });

        app.MapGet("/devices/statuses/all", async (ISender sender) =>
        {
            var response = await sender.Send(new GetAllStatusesCommand());
            var dtos = response.Statuses;

            return Results.Ok(dtos);
        });

        app.MapGet("/devices/locations/all", async (ISender sender) =>
        {
            var response = await sender.Send(new GetAllLocationsComand());
            var models = response.Locations;
            var dtos = models.Select(x => x.Adapt<LocationDTO>());

            return Results.Ok(dtos);
        });

        app.MapGet("/devices/timestamps/all", async (ISender sender) =>
        {
            var response = await sender.Send(new GetAllTimestampsCommand());
            var models = response.Timestamps;
            var dtos = models.Adapt<IEnumerable<TimestampDTO>>();

            return Results.Ok(dtos);
        });

        app.MapGet("/devices/measurementtypes/all", async (ISender sender) =>
        {
            var response = await sender.Send(new GetAllMeasurementTypesCommand());
            var dtos = response.MeasurementTypes;

            return Results.Ok(dtos);
        });

        //  ---------- Get ----------

        app.MapGet("/devices/device/",
            async ([FromQuery] int DeviceID,
                   ISender sender) =>
        {
            var response = await sender.Send(new GetDeviceByIDCommand(DeviceID));
            var dto = response.DeviceDTO;

            return Results.Ok(dto);
        });

        //  ---------- Put ----------

        app.MapPut("/devices/device/",
            async ([FromQuery] int DeviceID,
                   [FromQuery] string? Name,
                   [FromQuery] int? LocationID,
                   [FromQuery] int? TimestampID,
                   [FromQuery] int? StatusID,
                   ISender sender) =>
        {
            var response = await sender.Send(new UpdateDeviceCommand(DeviceID, Name, LocationID, TimestampID, StatusID));
            var dto = response.DeviceDTO;

            return Results.Ok(dto);
        });

        app.MapPut("/devices/device/status/",
            async ([FromQuery] int DeviceID,
                   [FromQuery] int StatusID,
                   ISender sender) =>
        {
            var response = await sender.Send(new UpdateDeviceStatusCommand(DeviceID, StatusID));
            var dto = response.DeviceDTO;

            return Results.Ok(dto);
        });

        app.MapPut("/devices/device/measurementtype/",
            async ([FromQuery] int DeviceID,
                   [FromQuery] int[] MeasurementTypeIDs,
                   ISender sender) =>
        {
            var response = await sender.Send(new UpdateDeviceMeasurementTypeCommand(DeviceID, MeasurementTypeIDs));
            var dto = response.DeviceDTO;

            return Results.Ok(dto);
        });

        //  ---------- Delete ----------

        app.MapDelete("/devices/{devicenumber}",
            async ([FromQuery] int DeviceID,
                   ISender sender) =>
        {
            var response = await sender.Send(new DeleteDeviceCommand(DeviceID));
            var dto = response.DeviceDTO;

            return Results.Ok(dto);
        });
    }
}