using CommonServiceLibrary.Messaging.Events;
using Devices.Application.Hubs;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace Devices.Application.Devices.UpdateDevice;

public class UpdateDeviceStatusHandler(DevicesDBContext context, IPublishEndpoint publisher, IHubContext<DeviceHub> hubContext) : IRequestHandler<UpdateDeviceStatusCommand, GetDeviceStatusResponse>
{
    public async Task<GetDeviceStatusResponse> Handle(UpdateDeviceStatusCommand request, CancellationToken cancellationToken)
    {
        Device? foundDevice = await context.Devices
            .Include(x => x.TimestampConfiguration)
            .Include(x => x.Status)
            .Include(x => x.MeasurementConfiguration)
            .Include(x => x.Location)
            .FirstOrDefaultAsync(x => x.Id == request.DeviceID);
        if (foundDevice == null)
        {
            throw new DeviceNotFoundException(request.DeviceID);
        }

        //MeasurementConfig? foundMeasurementConfig = await context.MeasurementConfigs.FirstOrDefaultAsync(x => x.Id == foundDevice.MeasurementConfigId);
        //if (foundMeasurementConfig == null)
        //{
        //    throw new EntityNotFoundException(nameof(MeasurementConfig), foundDevice.MeasurementConfigId);
        //}

        //foundDevice.MeasurementConfiguration = foundMeasurementConfig;

        Status? foundStatus = await context.Statuses.FirstOrDefaultAsync(x => x.Type == request.StatusType);
        if (foundStatus == null)
        {
            throw new EntityNotFoundException(nameof(Status), request.StatusType);
        }

        if (foundDevice.Status.Type == request.StatusType)
        {
            var resp = new GetDeviceStatusResponse(foundStatus.Id, foundStatus.Type);
            return resp;
        }

        foundDevice.StatusId = foundStatus.Id;
        await context.SaveChangesAsync();

        var response = new GetDeviceStatusResponse(foundDevice.StatusId, foundStatus.Type);
        var dto = foundDevice.Adapt<DefaultDeviceDTO>();

        var mqMessage = new DeviceStatusChangedMessage()
        {
            Payload = dto,
        };
        await publisher.Publish(mqMessage, cancellationToken);

        await hubContext.Clients.All.SendAsync("DeviceUpdated", dto, cancellationToken);

        return response;
    }
}

public class UpdateDeviceHandler(DevicesDBContext context, IPublishEndpoint publisher, IHubContext<DeviceHub> hubContext) : IRequestHandler<UpdateDeviceCommand, GetDeviceResponse>
{
    public async Task<GetDeviceResponse> Handle(UpdateDeviceCommand request, CancellationToken cancellationToken)
    {
        var updateDTO = request.Device;

        Device? foundDevice = await context.Devices
            .Include(x => x.MeasurementConfiguration)
            .Include(x => x.TimestampConfiguration)
            .Include(x => x.Status)
            .Include(x => x.Location)
            .FirstOrDefaultAsync(x => x.DeviceNumber == request.DeviceNumber);

        if (foundDevice == null)
        {
            throw new DeviceNotFoundException(request.DeviceNumber);
        }

        if (updateDTO.LocationID == null && updateDTO.StatusID == null && updateDTO.TimestampConfigurationID == null && updateDTO.Name == null && updateDTO.MeasurementConfiguration == null)
        {
            throw new NotEnoughDataUpdateException("Device");
        }

        if (updateDTO.Name != null)
        {
            foundDevice.Name = updateDTO.Name;
        }

        //  Update `Status`
        if (updateDTO.StatusID != null)
        {
            Status? status = await context.Statuses.FindAsync(updateDTO.StatusID, cancellationToken);
            if (status == null)
            {
                throw new StatusNotFoundException(updateDTO.StatusID);
            }
            else
            {
                foundDevice.StatusId = status.Id;
            }
        }

        //  Update `Location`
        if (updateDTO.LocationID != null)
        {
            Location? location = await context.Locations.FindAsync(updateDTO.LocationID, cancellationToken);
            if (location == null)
            {
                throw new StatusNotFoundException(updateDTO.LocationID);
            }
            else
            {
                foundDevice.LocationId = location.Id;
            }
        }

        //  Update `Location`
        if (updateDTO.TimestampConfigurationID != null)
        {
            TimestampConfiguration? timestamp = await context.TimestampConfigurations.FindAsync(updateDTO.TimestampConfigurationID, cancellationToken);
            if (timestamp == null)
            {
                throw new StatusNotFoundException(updateDTO.TimestampConfigurationID);
            }
            else
            {
                foundDevice.TimestampConfigurationId = timestamp.Id;
            }
        }

        if (updateDTO.MeasurementConfiguration is not null)
        {
            var config = updateDTO.MeasurementConfiguration;
            var foundConfig = foundDevice.MeasurementConfiguration;

            if (config.Temperature != null)
            {
                foundConfig.Temperature = (bool)config.Temperature;
            }

            if (config.Humidity != null)
            {
                foundConfig.Humidity = (bool)config.Humidity;
            }

            if (config.CarbonDioxide != null)
            {
                foundConfig.CarbonDioxide = (bool)config.CarbonDioxide;
            }

            if (config.VolatileOrganicCompounds != null)
            {
                foundConfig.VolatileOrganicCompounds = (bool)config.VolatileOrganicCompounds;
            }

            if (config.PM1 != null)
            {
                foundConfig.PM1 = (bool)config.PM1;
            }

            if (config.PM25 != null)
            {
                foundConfig.PM25 = (bool)config.PM25;
            }

            if (config.PM10 != null)
            {
                foundConfig.PM10 = (bool)config.PM10;
            }

            if (config.Formaldehyde != null)
            {
                foundConfig.Formaldehyde = (bool)config.Formaldehyde;
            }

            if (config.CarbonMonoxide != null)
            {
                foundConfig.CarbonMonoxide = (bool)config.CarbonMonoxide;
            }

            if (config.Ozone != null)
            {
                foundConfig.Ozone = (bool)config.Ozone;
            }

            if (config.Ammonia != null)
            {
                foundConfig.Ammonia = (bool)config.Ammonia;
            }

            if (config.Airflow != null)
            {
                foundConfig.Airflow = (bool)config.Airflow;
            }

            if (config.AirIonizationLevel != null)
            {
                foundConfig.AirIonizationLevel = (bool)config.AirIonizationLevel;
            }

            if (config.Oxygen != null)
            {
                foundConfig.Oxygen = (bool)config.Oxygen;
            }

            if (config.Radon != null)
            {
                foundConfig.Radon = (bool)config.Radon;
            }

            if (config.Illuminance != null)
            {
                foundConfig.Illuminance = (bool)config.Illuminance;
            }

            if (config.SoundLevel != null)
            {
                foundConfig.SoundLevel = (bool)config.SoundLevel;
            }
        }

        await context.SaveChangesAsync();

        var dto = foundDevice.Adapt<DefaultDeviceDTO>();
        var response = new GetDeviceResponse(dto);

        var mqMessage = new DeviceStatusChangedMessage()
        {
            Payload = dto,
        };
        await publisher.Publish(mqMessage, cancellationToken);

        await hubContext.Clients.All.SendAsync("DeviceUpdated", dto, cancellationToken);

        return response;
    }
}

public class UpdateDeviceMeasurementConfigHandler(DevicesDBContext context, IPublishEndpoint publisher, IHubContext<DeviceHub> hubContext) : IRequestHandler<UpdateDeviceMeasurementConfigCommand, GetMeasurementConfigResponse>
{
    public async Task<GetMeasurementConfigResponse> Handle(UpdateDeviceMeasurementConfigCommand request, CancellationToken cancellationToken)
    {
        MeasurementConfig? foundConfig = await context.MeasurementConfigs.FirstOrDefaultAsync(x => x.DeviceId == request.DeviceID);
        Device? foundDevice = await context.Devices
            .Include(x => x.MeasurementConfiguration)
            .Include(x => x.TimestampConfiguration)
            .Include(x => x.Status)
            .Include(x => x.Location)
            .FirstOrDefaultAsync(x => x.Id == request.DeviceID);

        if (foundConfig == null || foundDevice == null)
        {
            throw new EntityNotFoundException("MeasurementConfig", request.DeviceID);
        }

        if (request.Config.Temperature != null)
        {
            foundConfig.Temperature = (bool)request.Config.Temperature;
        }

        if (request.Config.Humidity != null)
        {
            foundConfig.Humidity = (bool)request.Config.Humidity;
        }

        if (request.Config.CarbonDioxide != null)
        {
            foundConfig.CarbonDioxide = (bool)request.Config.CarbonDioxide;
        }

        if (request.Config.VolatileOrganicCompounds != null)
        {
            foundConfig.VolatileOrganicCompounds = (bool)request.Config.VolatileOrganicCompounds;
        }

        if (request.Config.PM1 != null)
        {
            foundConfig.PM1 = (bool)request.Config.PM1;
        }

        if (request.Config.PM25 != null)
        {
            foundConfig.PM25 = (bool)request.Config.PM25;
        }

        if (request.Config.PM10 != null)
        {
            foundConfig.PM10 = (bool)request.Config.PM10;
        }

        if (request.Config.Formaldehyde != null)
        {
            foundConfig.Formaldehyde = (bool)request.Config.Formaldehyde;
        }

        if (request.Config.CarbonMonoxide != null)
        {
            foundConfig.CarbonMonoxide = (bool)request.Config.CarbonMonoxide;
        }

        if (request.Config.Ozone != null)
        {
            foundConfig.Ozone = (bool)request.Config.Ozone;
        }

        if (request.Config.Ammonia != null)
        {
            foundConfig.Ammonia = (bool)request.Config.Ammonia;
        }

        if (request.Config.Airflow != null)
        {
            foundConfig.Airflow = (bool)request.Config.Airflow;
        }

        if (request.Config.AirIonizationLevel != null)
        {
            foundConfig.AirIonizationLevel = (bool)request.Config.AirIonizationLevel;
        }

        if (request.Config.Oxygen != null)
        {
            foundConfig.Oxygen = (bool)request.Config.Oxygen;
        }

        if (request.Config.Radon != null)
        {
            foundConfig.Radon = (bool)request.Config.Radon;
        }

        if (request.Config.Illuminance != null)
        {
            foundConfig.Illuminance = (bool)request.Config.Illuminance;
        }

        if (request.Config.SoundLevel != null)
        {
            foundConfig.SoundLevel = (bool)request.Config.SoundLevel;
        }

        await context.SaveChangesAsync();

        var dto = foundConfig.Adapt<DefaultMeasurementConfigurationDTO>();
        var response = new GetMeasurementConfigResponse(dto);

        var devideDTO = foundDevice.Adapt<DefaultDeviceDTO>();

        var mqMessage = new DeviceStatusChangedMessage()
        {
            Payload = devideDTO,
        };
        await publisher.Publish(mqMessage, cancellationToken);

        await hubContext.Clients.All.SendAsync("DeviceUpdated", devideDTO, cancellationToken);

        return response;
    }
}
