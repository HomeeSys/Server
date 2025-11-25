namespace Devices.Application.Devices.GetDevice
{
    public class GetDeviceByIDHandler(DevicesDBContext context) : IRequestHandler<GetDeviceByIDCommand, GetDeviceResponse>
    {
        public async Task<GetDeviceResponse> Handle(GetDeviceByIDCommand request, CancellationToken cancellationToken)
        {
            var result = await context.Devices
                .Include(x => x.Location)
                .Include(x => x.Timestamp)
                .Include(x => x.MeasurementTypes)
                .Include(x => x.Status)
                .FirstOrDefaultAsync(x => x.ID == request.ID, cancellationToken);
            if (result == null)
            {
                throw new DeviceNotFoundException(request.ID);
            }

            var dto = result.Adapt<DefaultDeviceDTO>();
            var response = new GetDeviceResponse(dto);

            return response;
        }
    }

    public class GetDeviceByDeviceNumberHandler(DevicesDBContext context) : IRequestHandler<GetDeviceByDeviceNumberCommand, GetDeviceResponse>
    {
        public async Task<GetDeviceResponse> Handle(GetDeviceByDeviceNumberCommand request, CancellationToken cancellationToken)
        {
            var result = await context.Devices
                .Include(x => x.Location)
                .Include(x => x.Timestamp)
                .Include(x => x.MeasurementTypes)
                .Include(x => x.Status)
                .Where(x => x.DeviceNumber == request.DeviceNumber)
                .FirstOrDefaultAsync(cancellationToken);
            if (result == null)
            {
                throw new DeviceNotFoundException(request.DeviceNumber);
            }

            var dto = result.Adapt<DefaultDeviceDTO>();
            var response = new GetDeviceResponse(dto);

            return response;
        }
    }

    public class GetAllDevicesHandler(DevicesDBContext context) : IRequestHandler<GetDeviceCommands, GetAllDevicesResponse>
    {
        public async Task<GetAllDevicesResponse> Handle(GetDeviceCommands request, CancellationToken cancellationToken)
        {
            var result = await context.Devices
                .Include(x => x.Location)
                .Include(x => x.Status)
                .Include(x => x.MeasurementTypes)
                .Include(x => x.Timestamp).Include(x => x.Status).ToListAsync(cancellationToken);

            var dtos = result.Adapt<List<DefaultDeviceDTO>>();

            var response = new GetAllDevicesResponse(dtos);

            return response;
        }
    }

    public class GetAllStatusesHandler(DevicesDBContext context) : IRequestHandler<GetAllStatusesCommand, GetAllStatusesResponse>
    {
        public async Task<GetAllStatusesResponse> Handle(GetAllStatusesCommand request, CancellationToken cancellationToken)
        {
            var items = await context.Statuses.ToListAsync(cancellationToken);
            var dtos = items.Adapt<List<StatusDTO>>();
            var response = new GetAllStatusesResponse(dtos);

            return response;
        }
    }

    public class GetAllMeasurementTypesHandler(DevicesDBContext context) : IRequestHandler<GetAllMeasurementTypesCommand, GetAllMeasurementTypesResponse>
    {
        public async Task<GetAllMeasurementTypesResponse> Handle(GetAllMeasurementTypesCommand request, CancellationToken cancellationToken)
        {
            var items = await context.MeasurementTypes.ToListAsync(cancellationToken);
            var dtos = items.Adapt<List<DefaultMeasurementTypeDTO>>();
            var response = new GetAllMeasurementTypesResponse(dtos);

            return response;
        }
    }

    public class GetAllLocationsHandler(DevicesDBContext context) : IRequestHandler<GetAllLocationsComand, GetAllLocationsResponse>
    {
        public async Task<GetAllLocationsResponse> Handle(GetAllLocationsComand request, CancellationToken cancellationToken)
        {
            var models = await context.Locations.ToListAsync(cancellationToken);

            var response = new GetAllLocationsResponse(models);

            return response;
        }
    }

    public class GetAllMeasurementConfigsHandler(DevicesDBContext context) : IRequestHandler<GetAllTimestampsCommand, GetAllTimestampsResponse>
    {
        public async Task<GetAllTimestampsResponse> Handle(GetAllTimestampsCommand request, CancellationToken cancellationToken)
        {
            var items = await context.Timestamps.ToListAsync(cancellationToken);

            var response = new GetAllTimestampsResponse(items);

            return response;
        }
    }

    public class GetMeasurementConfigByDeviceNumber(DevicesDBContext context) : IRequestHandler<GetMeasurementTypeByDeviceNumberCommand, GetMeasurementTypeResponse>
    {
        public async Task<GetMeasurementTypeResponse> Handle(GetMeasurementTypeByDeviceNumberCommand request, CancellationToken cancellationToken)
        {
            var device = await context.Devices.Include(x => x.MeasurementTypes).FirstOrDefaultAsync(x => x.DeviceNumber == request.DeviceNumber);

            var measurements = device.MeasurementTypes;

            var dto = measurements.Adapt<IEnumerable<DefaultMeasurementTypeDTO>>();

            var response = new GetMeasurementTypeResponse(dto);

            return response;
        }
    }
}
