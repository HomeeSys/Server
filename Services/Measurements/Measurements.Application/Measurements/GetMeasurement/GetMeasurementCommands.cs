namespace Measurements.Application.Measurements.GetMeasurement;

/// <summary>
/// Get single measurement by its ID
/// </summary>
/// <param name="MeasurementID"></param>
public record GetMeasurementCommand(Guid MeasurementID) : IRequest<GetMeasurementResponse>;
public record GetMeasurementResponse(DefaultMeasurementDTO Measurement);
public class GetMeasurementCommandValidator : AbstractValidator<GetMeasurementCommand>
{
    public GetMeasurementCommandValidator()
    {
        RuleFor(x => x.MeasurementID).NotNull();
    }
}

/// <summary>
/// Get page of measurements with provided parameters.
/// </summary>
/// <param name="Page"></param>
/// <param name="PageSize"></param>
/// <param name="SortOrder"></param>
/// <param name="DeviceNumber"></param>
/// <param name="DateStart"></param>
/// <param name="DateEnd"></param>
/// <param name="LocationID"></param>
public record GetAllMeasurementCommand(int Page, int PageSize, string? SortOrder, Guid? DeviceNumber, DateTime? DateStart, DateTime? DateEnd, int? LocationID) : IRequest<GetAllMeasurementResponse>;
public record GetAllMeasurementResponse(PaginatedList<DefaultMeasurementDTO> PaginatedMeasurements);
public class GetAllMeasurementCommandValidator : AbstractValidator<GetAllMeasurementCommand>
{
    public GetAllMeasurementCommandValidator()
    {
    }
}

public record GetAllCombinedMeasurementCommand(int Page, int PageSize, string? SortOrder, Guid? DeviceNumber, DateTime? DateStart, DateTime? DateEnd, int? LocationID) : IRequest<GetAllCombinedMeasurementResponse>;
public record GetAllCombinedMeasurementResponse(PaginatedList<CombinedMeasurementDTO> PaginatedMeasurements);
public class GetAllCombinedMeasurementCommandValidator : AbstractValidator<GetAllCombinedMeasurementCommand>
{
    public GetAllCombinedMeasurementCommandValidator()
    {
    }
}
