namespace Devices.Application.Devices.DeleteDevice;

public record DeleteDeviceCommand(int DeviceID) : IRequest<GetDeviceResponse>;
public class DeleteDeviceCommandValidator : AbstractValidator<DeleteDeviceCommand>
{
    public DeleteDeviceCommandValidator()
    {
    }
}
