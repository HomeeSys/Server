namespace Raports.Application.Handlers.Update;

public record UpdateRaportStatusCommand(int RaportID, int StatusID) : IRequest<ReadRaportResponse>;
public class UpdateRaportStatusCommandValidator : AbstractValidator<UpdateRaportStatusCommand>
{
    public UpdateRaportStatusCommandValidator() { }
}

public record UpdateRaportCommand(int RaportID, DateTime? UpdatedStartDate, DateTime? UpdatedEndDate, int? UpdatedPeriodID) : IRequest<ReadRaportResponse>;
public class UpdateRaportCommandValidator : AbstractValidator<UpdateRaportCommand>
{
    public UpdateRaportCommandValidator()
    {
        RuleFor(x => x)
        .Must(x =>
            (!x.UpdatedStartDate.HasValue && !x.UpdatedEndDate.HasValue) ||
            (x.UpdatedStartDate.HasValue && x.UpdatedEndDate.HasValue))
        .WithMessage("Both UpdatedStartDate and UpdatedEndDate must be either null or defined.");

        RuleFor(x => x)
            .Must(x =>
                !x.UpdatedStartDate.HasValue ||
                !x.UpdatedEndDate.HasValue ||
                x.UpdatedStartDate <= x.UpdatedEndDate)
            .WithMessage("UpdatedStartDate must be earlier than or equal to UpdatedEndDate.");

        RuleFor(x => x)
            .Must(x =>
                x.UpdatedStartDate.HasValue ||
                x.UpdatedEndDate.HasValue ||
                x.UpdatedPeriodID.HasValue)
            .WithMessage("At least one field must be provided.");
    }
}