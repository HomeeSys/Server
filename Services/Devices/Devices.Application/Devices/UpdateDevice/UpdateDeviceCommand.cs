namespace Devices.Application.Devices.UpdateDevice
{
    #region Get all devices
    public record UpdateDeviceCommand(Guid DeviceNumber, UpdateDeviceDTO Device) : IRequest<GetDeviceResponse>;
    public class UpdateDeviceCommandValidator : AbstractValidator<UpdateDeviceCommand>
    {
        public UpdateDeviceCommandValidator()
        {
        }
    }
    #endregion
}
