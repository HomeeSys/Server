namespace Raports.Application.Handlers;

public class RaportsServiceEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        //  ---------- Post ----------

        app.MapPost("/raports/raport", async (ISender sender, [FromQuery] DateTime StartDate, [FromQuery] DateTime EndDate, [FromQuery] int PeriodID, [FromQuery] int[] RequestedLocationsIDs, [FromQuery] int[] RequestedMeasurementsIDs) =>
        {
            var response = await sender.Send(new CreateRaportCommand(StartDate, EndDate, PeriodID, RequestedLocationsIDs, RequestedMeasurementsIDs));
            var dto = response.RaportDTO;

            return Results.Ok(dto);
        });

        //  ---------- Get All ----------

        app.MapGet("/raports/locations/all", async (ISender sender) =>
        {
            var response = await sender.Send(new ReadAllLocationsCommand());
            var dto = response.LocationsDTOs;

            return Results.Ok(dto);
        });


        app.MapGet("/raports/measurements/all", async (ISender sender) =>
        {
            var response = await sender.Send(new ReadAllMeasurementsCommand());
            var dto = response.MeasurementDTOs;

            return Results.Ok(dto);
        });

        app.MapGet("/raports/periods/all", async (ISender sender) =>
        {
            var response = await sender.Send(new ReadAllPeriodCommand());
            var dto = response.PeriodsDTO;

            return Results.Ok(dto);
        });

        app.MapGet("/raports/statuses/all", async (ISender sender) =>
        {
            var response = await sender.Send(new ReadAllStatusesCommand());
            var dto = response.StatusesDTOs;

            return Results.Ok(dto);
        });

        app.MapGet("/raports/raports/all", async (ISender sender) =>
        {
            var response = await sender.Send(new ReadAllRaportsCommand());
            var dto = response.RaportDTOs;

            return Results.Ok(dto);
        });

        app.MapGet("/raports/query", async (ISender sender, [FromQuery] DateTime? RaportCreationDateFrom, [FromQuery] DateTime? RaportCreationDateTo, [FromQuery] string? SortOrder, [FromQuery] string? PeriodName, [FromQuery] string? StatusName, [FromQuery] int Page, [FromQuery] int PageSize) =>
        {
            var response = await sender.Send(new ReadAllRaportsQueryCommand(RaportCreationDateFrom, RaportCreationDateTo, SortOrder, PeriodName, StatusName, Page, PageSize));

            return Results.Ok(response);
        });

        //  ---------- Get ----------

        app.MapGet("/raports/raports/{ID}", async (ISender sender, int ID) =>
        {
            var response = await sender.Send(new ReadRaportCommand(ID));
            var dto = response.RaportDTO;

            return Results.Ok(dto);
        });

        //  ---------- Download raport ----------

        app.MapGet("/raports/raports/download/{RaportID}", async (ISender sender, int RaportID) =>
        {
            var response = await sender.Send(new DownloadRaportCommand(RaportID));

            return Results.File(response.FileStream, response.ContentType, response.FileName);
        });

        //  ---------- Put ----------

        app.MapPut("/raports/raport/retry", async ([FromQuery] int RaportID, ISender sender) =>
        {
            var response = await sender.Send(new RetryRaportCommand(RaportID));
            var dto = response.RaportDTO;

            return Results.Ok(dto);
        });

        app.MapPut("/raports/raport/status", async ([FromQuery] int RaportID, [FromQuery] int StatusID, ISender sender) =>
        {
            var response = await sender.Send(new UpdateRaportStatusCommand(RaportID, StatusID));
            var dto = response.RaportDTO;

            return Results.Ok(dto);
        });

        app.MapPut("/raports/raport", async ([FromQuery] int RaportID, [FromQuery] DateTime? StartDate, [FromQuery] DateTime? EndDate, [FromQuery] int? PeriodID, ISender sender) =>
        {
            var response = await sender.Send(new UpdateRaportCommand(RaportID, StartDate, EndDate, PeriodID));
            var dto = response.RaportDTO;

            return Results.Ok(dto);
        });
    }
}