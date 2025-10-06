namespace Devices.Application.Devices.GetDevice
{
    #region Get all devices
    public record GetDeviceCommands() : IRequest<GetAllDevicesResponse>;
    public record GetAllDevicesResponse(IEnumerable<DefaultDeviceDTO> DeviceDTOs);
    public class GetAllDevicesCommandValidator : AbstractValidator<GetDeviceCommands>
    {
        public GetAllDevicesCommandValidator()
        {

        }
    }
    #endregion

    #region Get device by ID
    public record GetDeviceByIDCommand(int ID) : IRequest<GetDeviceResponse>;
    public record GetDeviceResponse(DefaultDeviceDTO DeviceDTO);
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

    #region Get Device `Measurement config` by device number
    public record GetMeasurementConfigByDeviceNumberCommand(Guid DeviceNumber) : IRequest<GetMeasurementConfigResponse>;
    public record GetMeasurementConfigResponse(DefaultMeasurementConfigurationDTO MeasurementConfigurationDTO);
    public class GetMeasurementConfigResponseValidator : AbstractValidator<GetMeasurementConfigByDeviceNumberCommand>
    {
        public GetMeasurementConfigResponseValidator()
        {

        }
    }

    public record GetAllTimestampConfigurationsCommand() : IRequest<GetAllTimestampConfigurationsResponse>;
    public record GetAllTimestampConfigurationsResponse(IEnumerable<TimestampConfiguration> Configurations);
    public class GetAllTimestampConfigurationsCommandValidator : AbstractValidator<GetAllTimestampConfigurationsCommand>
    {
        public GetAllTimestampConfigurationsCommandValidator()
        {

        }
    }
    #endregion

    #region Get all locations
    public record GetAllLocationsComand() : IRequest<GetAllLocationsResponse>;
    public record GetAllLocationsResponse(IEnumerable<Location> Locations);
    public class GetAllLocationsResponseValidator : AbstractValidator<GetAllLocationsComand>
    {
        public GetAllLocationsResponseValidator()
        {

        }
    }
    #endregion
}