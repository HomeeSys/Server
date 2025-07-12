namespace Devices.Application.Devices.DeleteDevice
{
    public record DeleteDeviceCommand(Guid DeviceNumber) : IRequest<GetDeviceResponse>;
    public class DeleteDeviceCommandValidator : AbstractValidator<DeleteDeviceCommand>
    {
        public DeleteDeviceCommandValidator()
        {
        }
    }
}
