namespace Devices.Application.Devices.CreateDevice
{
    public record CreateDeviceCommand(RegisterDeviceDTO Device) : IRequest<CreateDeviceResponse>;

    public record CreateDeviceResponse(Device Device);

    public class CreateDeviceCommandValidator : AbstractValidator<CreateDeviceCommand>
    {
        public CreateDeviceCommandValidator()
        {
            RuleFor(x=>x.Device.Description).NotNull();
            RuleFor(x=>x.Device.Description).NotEmpty();
            RuleFor(x=>x.Device.DeviceNumber).NotEmpty();
        }
    }
}
