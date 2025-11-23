namespace Devices.Application.Devices.CreateDevice;

public record CreateDeviceCommand(string Name, Guid DeviceNumber, int LocationID, int StatusID, int TimestampID) : IRequest<GetDeviceResponse>;
public class CreateDeviceCommandValidator : AbstractValidator<CreateDeviceCommand>
{
    public CreateDeviceCommandValidator()
    {
    }
}
