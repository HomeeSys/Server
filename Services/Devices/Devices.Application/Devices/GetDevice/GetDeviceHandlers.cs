namespace Devices.Application.Devices.GetDevice
{
    public class GetDeviceByIDHandler(DevicesDBContext context) : IRequestHandler<GetDeviceByIDCommand, GetDeviceResponse>
    {
        public async Task<GetDeviceResponse> Handle(GetDeviceByIDCommand request, CancellationToken cancellationToken)
        {
            var result = await context.Devices
                .Include(x => x.Location)
                .Include(x => x.TimestampConfiguration)
                .Include(x => x.MeasurementConfiguration)
                .Include(x => x.Status)
                .FirstOrDefaultAsync(x => x.Id == request.ID, cancellationToken);
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
                .Include(x => x.TimestampConfiguration)
                .Include(x => x.MeasurementConfiguration)
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
                .Include(x => x.MeasurementConfiguration)
                .Include(x => x.TimestampConfiguration).Include(x => x.Status).ToListAsync(cancellationToken);

            var dtos = result.Adapt<List<DefaultDeviceDTO>>();

            var response = new GetAllDevicesResponse(dtos);

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

    public class GetAllMeasurementConfigsHandler(DevicesDBContext context) : IRequestHandler<GetAllTimestampConfigurationsCommand, GetAllTimestampConfigurationsResponse>
    {
        public async Task<GetAllTimestampConfigurationsResponse> Handle(GetAllTimestampConfigurationsCommand request, CancellationToken cancellationToken)
        {
            var items = await context.TimestampConfigurations.ToListAsync(cancellationToken);

            var response = new GetAllTimestampConfigurationsResponse(items);

            return response;
        }
    }

    public class GetMeasurementConfigByDeviceNumber(DevicesDBContext context) : IRequestHandler<GetMeasurementConfigByDeviceNumberCommand, GetMeasurementConfigResponse>
    {
        public async Task<GetMeasurementConfigResponse> Handle(GetMeasurementConfigByDeviceNumberCommand request, CancellationToken cancellationToken)
        {
            var device = await context.Devices.FirstOrDefaultAsync(x => x.DeviceNumber == request.DeviceNumber);

            var result = await context.MeasurementConfigs.FirstOrDefaultAsync(x => x.DeviceId == device.Id);

            var dto = result.Adapt<DefaultMeasurementConfigurationDTO>();
            var response = new GetMeasurementConfigResponse(dto);

            return response;
        }
    }
}
