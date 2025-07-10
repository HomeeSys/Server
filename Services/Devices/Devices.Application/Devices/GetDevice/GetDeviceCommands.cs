namespace Devices.Application.Devices.GetDevice
{
    #region Get all devices
    public record GetDeviceCommands() : IRequest<GetAllDevicesResponse>;
    public record GetAllDevicesResponse(IEnumerable<Device> Devices);
    public class GetAllDevicesCommandValidator : AbstractValidator<GetDeviceCommands>
    {
        public GetAllDevicesCommandValidator()
        {

        }
    }
    #endregion

    #region Get device by ID
    public record GetDeviceByIDCommand(int ID) : IRequest<GetDeviceResponse>;
    public record GetDeviceResponse(Device Device);
    public class GetDeviceByIDCommandValidator : AbstractValidator<GetDeviceByIDCommand>
    {
        public GetDeviceByIDCommandValidator()
        {
        }
    }
    #endregion

    #region Get device by Device Number
    public record GetDeviceByDeviceNumberCommand(Guid DeviceNumber) : IRequest<GetDeviceResponse>;
    public class GetDeviceByDeviceNumberCommandValidator : AbstractValidator<GetDeviceByDeviceNumberCommand>
    {
        public GetDeviceByDeviceNumberCommandValidator()
        {

        }
    }
    #endregion
}