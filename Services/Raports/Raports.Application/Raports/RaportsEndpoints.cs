using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Raports.Application.Dtos;
using Raports.Application.Raports.EnqueueDailyRaportGeneration;

namespace Raports.Application.Raports
{
    public class RaportsEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/raports/enqueuedaily/{RaportDate}", async (DateTime RaportDate, ISender sender) =>
            {
                var response = await sender.Send(new EnqueueDailyRaportCommand(RaportDate));

                var statusDto = new EnqueueDailyRaportDto() { Status = response.Success };

                return Results.Ok(statusDto);
            });
        }
    }
}
