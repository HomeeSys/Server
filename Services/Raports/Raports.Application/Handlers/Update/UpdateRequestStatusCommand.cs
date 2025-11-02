namespace Raports.Application.Handlers.Update;

public record UpdateRequestStatusCommand(int RequestID, UpdateRequestStatusDTO UpdateDTO) : IRequest<ReadRequestResponse>;
public class UpdateRequestStatusCommandValidator : AbstractValidator<UpdateRequestStatusCommand>
{
    public UpdateRequestStatusCommandValidator() { }
}

public record UpdateRequestCommand(int RequestID, UpdateRequestDTO UpdateDTO) : IRequest<ReadRequestResponse>;
public class UpdateRequestCommandValidator : AbstractValidator<UpdateRequestCommand>
{
    public UpdateRequestCommandValidator()
    {
        RuleFor(x => x)
            .Must(x =>
                (!x.UpdateDTO.UpdatedStartDate.HasValue && !x.UpdateDTO.UpdatedEndDate.HasValue) ||
                (x.UpdateDTO.UpdatedStartDate.HasValue && x.UpdateDTO.UpdatedEndDate.HasValue))
            .WithMessage("Both UpdatedStartDate and UpdatedEndDate must be either null or defined.");

        RuleFor(x => x)
            .Must(x =>
                !x.UpdateDTO.UpdatedStartDate.HasValue ||
                !x.UpdateDTO.UpdatedEndDate.HasValue ||
                x.UpdateDTO.UpdatedStartDate <= x.UpdateDTO.UpdatedEndDate)
            .WithMessage("UpdatedStartDate must be earlier than or equal to UpdatedEndDate.");

        RuleFor(x => x)
            .Must(x =>
                x.UpdateDTO.UpdatedStartDate.HasValue ||
                x.UpdateDTO.UpdatedEndDate.HasValue ||
                x.UpdateDTO.UpdatedPeriodID.HasValue ||
                x.UpdateDTO.UpdatedRaportID.HasValue)
            .WithMessage("At least one field must be provided.");
    }
}

public record UpdateRequestRaportCommand(int RequestID, UpdateRequestRaportDTO UpdateDTO) : IRequest<ReadRequestResponse>;
public class UpdateRequestRaportCommandValidator : AbstractValidator<UpdateRequestRaportCommand>
{
    public UpdateRequestRaportCommandValidator() { }
}