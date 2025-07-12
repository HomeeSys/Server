namespace Devices.Application.Devices.UpdateDevice
{
    public class UpdateDeviceHandler(DevicesDBContext context) : IRequestHandler<UpdateDeviceCommand, GetDeviceResponse>
    {
        public async Task<GetDeviceResponse> Handle(UpdateDeviceCommand request, CancellationToken cancellationToken)
        {
            UpdateDeviceDTO updateDTO = request.Device;

            Device? foundDevice = await context.Devices
                .Include(x => x.Location)
                .Include(x => x.TimestampConfiguration)
                .Include(x => x.Status)
                .FirstOrDefaultAsync(x => x.DeviceNumber == request.DeviceNumber, cancellationToken);

            if (foundDevice == null)
            {
                throw new DeviceNotFoundException(request.DeviceNumber);
            }

            if(updateDTO.Description == null && updateDTO.LocationID == null && updateDTO.StatusID == null && updateDTO.TimestampConfigurationID == null)
            {
                throw new NotEnoughDataUpdateException("Device");
            }

            //  Update `Description`
            if (updateDTO.Description != null)
            {
                foundDevice.Description = updateDTO.Description;
            }

            //  Update `Status`
            if (updateDTO.StatusID != null)
            {
                Status? status = await context.Statuses.FindAsync(updateDTO.StatusID, cancellationToken);
                if(status == null)
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

            await context.SaveChangesAsync();

            var response = new GetDeviceResponse(foundDevice);

            return response;
        }
    }
}
