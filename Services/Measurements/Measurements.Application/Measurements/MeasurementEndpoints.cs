namespace Measurements.Application.Measurements;

public class MeasurementEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/measurements", async (CreateMeasurementSetDTO request, ISender sender) =>
        {
            GetMeasurementSetResponse response = await sender.Send(new CreateMeasurementCommand(request));

            MeasurementSet model = response.Measurement;

            return Results.Ok(model);
        });

        // Gets all measurements from all devices.
        app.MapGet("/measurements/all", async (ISender sender) =>
        {
            GetAllMeasurementSetsResponse response = await sender.Send(new GetMeasurementSetsCommand());

            IEnumerable<MeasurementSet> models = response.Measurements;

            IEnumerable<MeasurementSetDTO> dtos = models.Adapt<IEnumerable<MeasurementSetDTO>>();

            return Results.Ok(dtos);
        });

        // Get measurement with this ID.
        app.MapGet("/measurements/{ID}", async (Guid ID, ISender sender) =>
        {
            GetMeasurementSetResponse response = await sender.Send(new GetMeasurementSetCommand(ID));

            MeasurementSet model = response.Measurement;

            MeasurementSetDTO dto = model.Adapt<MeasurementSetDTO>();

            return Results.Ok(dto);
        });
        
        // Gets all measurements from given device.
        app.MapGet("/measurements/devices/{DeviceNumber}", async (Guid DeviceNumber, ISender sender) =>
        {
            GetAllMeasurementSetsResponse response = await sender.Send(new GetMeasurementSetsFromDeviceCommand(DeviceNumber));

            IEnumerable<MeasurementSet> models = response.Measurements;

            IEnumerable<MeasurementSetDTO> dtos = models.Adapt<IEnumerable<MeasurementSetDTO>>();

            return Results.Ok(dtos);
        });

        // Gets all measurements from given device with day filtering.
        app.MapGet("/measurements/devices/{DeviceNumber}/day/{Date}", async (Guid DeviceNumber, DateTime Date, ISender sender) =>
        {
            GetAllMeasurementSetsResponse response = await sender.Send(new GetAllMeasurementSetsFromDeviceByDayCommand(DeviceNumber, Date));

            IEnumerable<MeasurementSet> models = response.Measurements;

            IEnumerable<MeasurementSetDTO> dtos = models.Adapt<IEnumerable<MeasurementSetDTO>>();

            return Results.Ok(dtos);
        });

        // Gets all measurements from given device with week filtering.
        app.MapGet("/measurements/devices/{DeviceNumber}/week/{Date}", async (Guid DeviceNumber, DateTime Date, ISender sender) =>
        {
            GetAllMeasurementSetsResponse response = await sender.Send(new GetAllMeasurementSetsFromDeviceByWeekCommand(DeviceNumber, Date));

            IEnumerable<MeasurementSet> models = response.Measurements;

            IEnumerable<MeasurementSetDTO> dtos = models.Adapt<IEnumerable<MeasurementSetDTO>>();

            return Results.Ok(dtos);
        });

        // Gets all measurements from given device with month filtering.
        app.MapGet("/measurements/devices/{DeviceNumber}/month/{Date}", async (Guid DeviceNumber, DateTime Date, ISender sender) =>
        {
            GetAllMeasurementSetsResponse response = await sender.Send(new GetAllMeasurementSetsFromDeviceByMonthCommand(DeviceNumber, Date));

            IEnumerable<MeasurementSet> models = response.Measurements;

            IEnumerable<MeasurementSetDTO> dtos = models.Adapt<IEnumerable<MeasurementSetDTO>>();

            return Results.Ok(dtos);
        });

        //  Update device
        //app.MapPut("/measurement/devices/{DeviceNumer}", async (Guid DeviceNumber, UpdateMeasurementSetDTO measurementSetDTO, ISender sender) =>
        //{
        //    GetMeasurementSetResponse response = await sender.Send(new UpdateMeasurementSetCommand(DeviceNumber, measurementSetDTO));

        //    MeasurementSet model = response.Measurement;

        //    MeasurementSetDTO dto = model.Adapt<MeasurementSetDTO>();

        //    return Results.Ok(dto);
        //});

        //  Delete measureent with given ID.
        app.MapDelete("/measurements/{ID}", async (Guid ID, ISender sender) =>
        {
            DeleteMeasurementSetResponse response = await sender.Send(new DeleteMeasurementSetCommand(ID));

            return Results.Ok(response.Status);
        });

        //  Delete all measureents from Device.
        app.MapDelete("/measurements/devices/{ID}", async (Guid ID, ISender sender) =>
        {
            DeleteMeasurementSetResponse response = await sender.Send(new DeleteAllMeasurementSetsFromDeviceCommand(ID));

            return Results.Ok(response.Status);
        });
    }
}