namespace Measurements.Application.Measurements;

public class MeasurementEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/measurements", async (CreateMeasurementDTO createMeasurementDto, ISender sender) =>
        {
            var response = await sender.Send(new CreateMeasurementCommand(createMeasurementDto));

            var dto = response.Measurement;

            return Results.Ok(dto);
        });

        app.MapGet("/measurements/", async (Guid MeasurementID, ISender sender) =>
        {
            var response = await sender.Send(new GetMeasurementCommand(MeasurementID));

            var dto = response.Measurement;

            return Results.Ok(dto);
        });

        app.MapGet("/measurements/all/", async (ISender sender, int Page, int PageSize, string? SortOrder, Guid? DeviceNumber, DateTime? DateStart, DateTime? DateEnd, int? LocationID) =>
        {
            var response = await sender.Send(new GetAllMeasurementCommand(Page, PageSize, SortOrder, DeviceNumber, DateStart, DateEnd, LocationID));

            var paginatedDtos = response.PaginatedMeasurements;

            return Results.Ok(paginatedDtos);
        });

        app.MapGet("/measurements/combined/all/", async (ISender sender, int Page, int PageSize, string? SortOrder, Guid? DeviceNumber, DateTime? DateStart, DateTime? DateEnd, int? LocationID) =>
        {
            var response = await sender.Send(new GetAllCombinedMeasurementCommand(Page, PageSize, SortOrder, DeviceNumber, DateStart, DateEnd, LocationID));

            var paginatedDtos = response.PaginatedMeasurements;

            return Results.Ok(paginatedDtos);
        });

        app.MapDelete("/measurements/{ID}", async (Guid ID, ISender sender) =>
        {
            DeleteMeasurementSetResponse response = await sender.Send(new DeleteMeasurementSetCommand(ID));

            return Results.Ok(response.Status);
        });

        app.MapDelete("/measurements/devices/{ID}", async (Guid ID, ISender sender) =>
        {
            DeleteMeasurementSetResponse response = await sender.Send(new DeleteAllMeasurementSetsFromDeviceCommand(ID));

            return Results.Ok(response.Status);
        });
    }
}