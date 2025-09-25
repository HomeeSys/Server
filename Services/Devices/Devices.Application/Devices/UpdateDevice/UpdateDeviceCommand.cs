namespace Devices.Application.Devices.UpdateDevice
{
    #region Update device
    public record UpdateDeviceCommand(Guid DeviceNumber, UpdateDeviceDTO Device) : IRequest<GetDeviceResponse>;
    public class UpdateDeviceCommandValidator : AbstractValidator<UpdateDeviceCommand>
    {
        public UpdateDeviceCommandValidator()
        {
        }
    }
    #endregion

    #region Update device measurement config
    public record UpdateDeviceMeasurementConfigCommand(int DeviceID, UpdateMeasurementConfigDTO Config) : IRequest<GetMeasurementConfigResponse>;
    public class UpdateDeviceMeasurementConfigCommandValidator : AbstractValidator<UpdateDeviceMeasurementConfigCommand>
    {
        public UpdateDeviceMeasurementConfigCommandValidator()
        {
        }
    }
    #endregion

    #region Update device status
    public record UpdateDeviceStatusCommand(int DeviceID, string StatusType) : IRequest<GetDeviceStatusResponse>;
    public record GetDeviceStatusResponse(int StatusID, string StatusType);
    public class UpdateDeviceStatusCommandValidator : AbstractValidator<UpdateDeviceStatusCommand>
    {
        public UpdateDeviceStatusCommandValidator()
        {
        }
    }
    #endregion
}
