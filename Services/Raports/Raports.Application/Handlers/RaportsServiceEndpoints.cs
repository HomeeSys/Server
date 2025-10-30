namespace Raports.Application.Handlers;

public class RaportsServiceEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        //  Requests
        app.MapPost("/raports/requests", async (ISender sender, CreateRequestDTO createRequestDTO) =>
        {
            var response = await sender.Send(new CreateRequestCommand(createRequestDTO));

            var dto = response.RequestDTO;

            return Results.Ok(dto);
        });

        app.MapPut("/raports/requests", async (ISender sender) =>
        {
            Console.WriteLine("Update");
        });

        app.MapDelete("/raports/requests", async (ISender sender) =>
        {
            Console.WriteLine("Delete");
        });

        app.MapGet("/raports/requests/{ID}", async (ISender sender, int ID) =>
        {
            var response = await sender.Send(new ReadRequestCommand(ID));

            var dto = response.RequestDTO;

            return Results.Ok(dto);
        });

        app.MapGet("/raports/requests/all", async (ISender sender) =>
        {
            var response = await sender.Send(new ReadAllRequestCommand());

            var dto = response.RequestsDTOs;

            return Results.Ok(dto);
        });

        //  Raports 
        app.MapDelete("/raports/raports", async (ISender sender) =>
        {
            Console.WriteLine("Delete");
        });

        app.MapGet("/raports/raports/{ID}", async (ISender sender, int ID) =>
        {
            var response = await sender.Send(new ReadRaportCommand(ID));

            var dto = response.RaportDTO;

            return Results.Ok(dto);
        });

        app.MapGet("/raports/raports/all", async (ISender sender) =>
        {
            var response = await sender.Send(new ReadAllRaportsCommand());

            var dto = response.RaportDTOs;

            return Results.Ok(dto);
        });

        //  Statuses 
        app.MapGet("/raports/statuses/{ID}", async (ISender sender, int ID) =>
        {
            var response = await sender.Send(new ReadRaportStatusesCommand(ID));

            var dto = response.RequestStatusDTO;

            return Results.Ok(dto);
        });

        app.MapGet("/raports/statuses/all", async (ISender sender) =>
        {
            var response = await sender.Send(new ReadAllRaportStatusesCommand());

            var dto = response.RequestStatusesDTOs;

            return Results.Ok(dto);
        });

        //  Periods
        app.MapGet("/raports/periods/{ID}", async (ISender sender, int ID) =>
        {
            var response = await sender.Send(new ReadPeriodCommand(ID));

            var dto = response.PeriodDTO;

            return Results.Ok(dto);
        });

        app.MapGet("/raports/periods/all", async (ISender sender) =>
        {
            var response = await sender.Send(new ReadAllPeriodCommand());

            var dto = response.PeriodsDTOs;

            return Results.Ok(dto);
        });
    }
}