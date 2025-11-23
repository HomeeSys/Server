namespace Devices.Application.Devices.GetDevice;

public record GetDeviceCommands() : IRequest<GetAllDevicesResponse>;
public record GetAllDevicesResponse(IEnumerable<DefaultDeviceDTO> DeviceDTOs);
public class GetAllDevicesCommandValidator : AbstractValidator<GetDeviceCommands>
{
    public GetAllDevicesCommandValidator()
    {

    }
}

public record GetDeviceByIDCommand(int ID) : IRequest<GetDeviceResponse>;
public record GetDeviceResponse(DefaultDeviceDTO DeviceDTO);
public class GetDeviceByIDCommandValidator : AbstractValidator<GetDeviceByIDCommand>
{
    public GetDeviceByIDCommandValidator()
    {
    }
}

public record GetDeviceByDeviceNumberCommand(Guid DeviceNumber) : IRequest<GetDeviceResponse>;
public class GetDeviceByDeviceNumberCommandValidator : AbstractValidator<GetDeviceByDeviceNumberCommand>
{
    public GetDeviceByDeviceNumberCommandValidator()
    {

    }
}

public record GetMeasurementTypeByDeviceNumberCommand(Guid DeviceNumber) : IRequest<GetMeasurementTypeResponse>;
public record GetMeasurementTypeResponse(IEnumerable<DefaultMeasurementTypeDTO> MeasurementTypesDTO);
public class GetMeasurementTypeResponseValidator : AbstractValidator<GetMeasurementTypeByDeviceNumberCommand>
{
    public GetMeasurementTypeResponseValidator()
    {

    }
}

public record GetAllTimestampsResponse(IEnumerable<Timestamp> Timestamps);
public record GetAllTimestampsCommand() : IRequest<GetAllTimestampsResponse>;
public class GetAllTimestampsCommandValidator : AbstractValidator<GetAllTimestampsCommand>
{
    public GetAllTimestampsCommandValidator()
    {

    }
}

public record GetAllLocationsComand() : IRequest<GetAllLocationsResponse>;
public record GetAllLocationsResponse(IEnumerable<Location> Locations);
public class GetAllLocationsResponseValidator : AbstractValidator<GetAllLocationsComand>
{
    public GetAllLocationsResponseValidator()
    {

    }
}

public record GetAllMeasurementTypesResponse(IEnumerable<DefaultMeasurementTypeDTO> MeasurementTypes);
public record GetAllMeasurementTypesCommand() : IRequest<GetAllMeasurementTypesResponse>;
public class GetAllMeasurementTypesCommandValidator : AbstractValidator<GetAllMeasurementTypesCommand>
{
    public GetAllMeasurementTypesCommandValidator()
    {

    }
}