namespace Raports.Application.Handlers.Create;

public class CreateRaportHandler(RaportsDBContext database, IHubContext<RaportsHub> hub, IPublishEndpoint publishEndpoint) : IRequestHandler<CreateRaportCommand, CreateRaportResponse>
{
    public async Task<CreateRaportResponse> Handle(CreateRaportCommand request, CancellationToken cancellationToken)
    {
        var foundPeriod = await database.Periods.FirstOrDefaultAsync(x => x.ID == request.PeriodID, cancellationToken);
        if (foundPeriod is null)
        {
            throw new EntityNotFoundException(nameof(Period), request.PeriodID);
        }

        var foundLocations = new List<Location>();
        foreach (var reqLocID in request.RequestedLocationsIDs)
        {
            var foundLocation = await database.Locations.FirstOrDefaultAsync(x => x.ID == reqLocID, cancellationToken);
            if (foundLocation is null)
            {
                throw new EntityNotFoundException(nameof(Location), reqLocID);
            }

            foundLocations.Add(foundLocation);
        }

        var foundMeasurements = new List<Measurement>();
        foreach (var reqMesID in request.RequestedMeasurementsIDs)
        {
            var foundMeasurement = await database.Measurements.FirstOrDefaultAsync(x => x.ID == reqMesID, cancellationToken);
            if (foundMeasurement is null)
            {
                throw new EntityNotFoundException(nameof(Measurement), reqMesID);
            }

            foundMeasurements.Add(foundMeasurement);
        }

        var pendingStatus = await database.Statuses.FirstOrDefaultAsync(x => x.Name == "Pending");
        if (pendingStatus is null)
        {
            throw new EntityNotFoundException(nameof(Status), "Pending");
        }

        var newRaport = new Raport()
        {
            RaportCreationDate = DateTime.UtcNow,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Message = string.Empty,
            Period = foundPeriod,
            Status = pendingStatus,
        };

        //  Create new Raport 
        await database.Raports.AddAsync(newRaport, cancellationToken);
        await database.SaveChangesAsync();

        //  Create RequestedMeasruement for that Raport
        foreach (var reqMes in foundMeasurements)
        {
            var newRequestedMeasurement = new RequestedMeasurement()
            {
                Raport = newRaport,
                Measurement = reqMes
            };

            await database.RequestedMeasurements.AddAsync(newRequestedMeasurement, cancellationToken);
        }

        //  Create RequestedLocation for that Raport
        foreach (var reqLoc in foundLocations)
        {
            var newRequestedLocation = new RequestedLocation()
            {
                Raport = newRaport,
                Location = reqLoc
            };

            await database.RequestedLocations.AddAsync(newRequestedLocation, cancellationToken);
        }

        await database.SaveChangesAsync();

        //  Get refreshed Raport
        newRaport = await database.Raports
            .Include(x => x.RequestedMeasurements)
            .Include(x => x.RequestedLocations)
            .Include(x => x.Period)
            .FirstOrDefaultAsync(x => x.ID == newRaport.ID);

        var dto = newRaport.Adapt<DefaultRaportDTO>();
        var response = new CreateRaportResponse(dto);

        await hub.Clients.All.SendAsync("RaportCreated", dto, cancellationToken);

        var message = new RaportPending()
        {
            Raport = dto
        };
        await publishEndpoint.Publish(message, context =>
        {
            context.Headers.Set("PeriodName", message.Raport.Period.Name);
        });

        return response;
    }
}