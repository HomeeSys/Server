namespace Devices.Application.Devices.UpdateDevice;

public class UpdateDeviceStatusHandler(DevicesDBContext context, IPublishEndpoint publisher, IHubContext<DeviceHub> hubContext) : IRequestHandler<UpdateDeviceStatusCommand, GetDeviceResponse>
{
    public async Task<GetDeviceResponse> Handle(UpdateDeviceStatusCommand request, CancellationToken cancellationToken)
    {
        Device? foundDevice = await context.Devices
            .Include(x => x.Timestamp)
            .Include(x => x.Status)
            .Include(x => x.MeasurementTypes)
            .Include(x => x.Location)
            .FirstOrDefaultAsync(x => x.ID == request.DeviceID);
        if (foundDevice == null)
        {
            throw new DeviceNotFoundException(request.DeviceID);
        }

        Status? foundStatus = await context.Statuses.FirstOrDefaultAsync(x => x.ID == request.StatusID);
        if (foundStatus == null)
        {
            throw new EntityNotFoundException(nameof(Status), request.StatusID);
        }

        if (foundDevice.Status.ID == request.StatusID)
        {
            var dto = foundDevice.Adapt<DefaultDeviceDTO>();
            var resp = new GetDeviceResponse(dto);
            return resp;
        }

        foundDevice.StatusID = foundStatus.ID;
        await context.SaveChangesAsync();

        var deviceDTO = foundDevice.Adapt<DefaultDeviceDTO>();
        var message = new DeviceStatusChanged()
        {
            Device = deviceDTO,
        };

        await publisher.Publish(message, cancellationToken);
        await hubContext.Clients.All.SendAsync("DeviceUpdated", deviceDTO, cancellationToken);

        var response = new GetDeviceResponse(deviceDTO);

        return response;
    }
}

public class UpdateDeviceMeasurementTypeHandler(DevicesDBContext database, IPublishEndpoint publisher, IHubContext<DeviceHub> hub) : IRequestHandler<UpdateDeviceMeasurementTypeCommand, GetDeviceResponse>
{
    public async Task<GetDeviceResponse> Handle(UpdateDeviceMeasurementTypeCommand request, CancellationToken cancellationToken)
    {
        var foundDevice = await database.Devices
            .FirstOrDefaultAsync(x => x.ID == request.DeviceID);

        if (foundDevice is null)
        {
            throw new EntityNotFoundException(nameof(Device), request.DeviceID);
        }

        var foundDeviceMeasurementTypes = await database.DeviceMeasurementTypes
            .Where(x => x.DeviceID == request.DeviceID)
            .ToListAsync();

        database.DeviceMeasurementTypes.RemoveRange(foundDeviceMeasurementTypes);
        await database.SaveChangesAsync();

        foreach (var mesTypeID in request.MeasurementTypeIDs)
        {
            var foundMeasurementType = await database.MeasurementTypes.FirstOrDefaultAsync(x => x.ID == mesTypeID);
            if (foundMeasurementType is null)
            {
                throw new EntityNotFoundException(nameof(MeasurementType), mesTypeID);
            }

            var newDeviceMeasurementType = new DeviceMeasurementType()
            {
                Device = foundDevice,
                MeasurementType = foundMeasurementType
            };

            await database.DeviceMeasurementTypes.AddAsync(newDeviceMeasurementType);
        }

        await database.SaveChangesAsync();

        foundDevice = await database.Devices
            .Include(x => x.Location)
            .Include(x => x.Status)
            .Include(x => x.MeasurementTypes)
            .Include(x => x.Timestamp)
            .FirstOrDefaultAsync(x => x.ID == request.DeviceID);


        var dto = foundDevice.Adapt<DefaultDeviceDTO>();
        var message = new DeviceUpdated()
        {
            Device = dto,
        };

        await publisher.Publish(message, cancellationToken);
        await hub.Clients.All.SendAsync("DeviceUpdated", dto, cancellationToken);

        var response = new GetDeviceResponse(dto);

        return response;
    }
}

public class UpdateDeviceHandler(DevicesDBContext context, IPublishEndpoint publisher, IHubContext<DeviceHub> hubContext) : IRequestHandler<UpdateDeviceCommand, GetDeviceResponse>
{
    public async Task<GetDeviceResponse> Handle(UpdateDeviceCommand request, CancellationToken cancellationToken)
    {
        var foundDevice = await context.Devices
            .Include(x => x.MeasurementTypes)
            .Include(x => x.Timestamp)
            .Include(x => x.Status)
            .Include(x => x.Location)
            .FirstOrDefaultAsync(x => x.ID == request.DeviceID);

        if (foundDevice is null)
        {
            throw new DeviceNotFoundException(request.DeviceID);
        }

        if (request.Name is not null)
        {
            foundDevice.Name = request.Name;
        }

        if (request.StatusID is not null)
        {
            var dbStatus = await context.Statuses.FindAsync(request.StatusID, cancellationToken);
            if (dbStatus is null)
            {
                throw new EntityNotFoundException(nameof(Status), request.StatusID);
            }
            else
            {
                foundDevice.Status = dbStatus;
            }
        }

        if (request.LocationID is not null)
        {
            var dbLocation = await context.Locations.FindAsync(request.LocationID, cancellationToken);
            if (dbLocation is null)
            {
                throw new EntityNotFoundException(nameof(Location), request.LocationID);
            }
            else
            {
                foundDevice.Location = dbLocation;
            }
        }

        if (request.TimestampID is not null)
        {
            var dbTimestamp = await context.Timestamps.FindAsync(request.TimestampID, cancellationToken);
            if (dbTimestamp is null)
            {
                throw new EntityNotFoundException(nameof(Timestamp), request.TimestampID);
            }
            else
            {
                foundDevice.Timestamp = dbTimestamp;
            }
        }

        await context.SaveChangesAsync();

        var dto = foundDevice.Adapt<DefaultDeviceDTO>();
        var message = new DeviceUpdated()
        {
            Device = dto,
        };

        await publisher.Publish(message, cancellationToken);
        await hubContext.Clients.All.SendAsync("DeviceUpdated", dto, cancellationToken);

        var response = new GetDeviceResponse(dto);

        return response;
    }
}