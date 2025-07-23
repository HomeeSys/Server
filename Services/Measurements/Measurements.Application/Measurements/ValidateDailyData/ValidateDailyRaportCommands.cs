namespace Measurements.Application.Measurements.ValidateDailyData
{
    public record ValidateDailyDataCommand(DateTime Date) : IRequest<ValidateDailyDataResponse>;
    public record ValidateDailyDataResponse(bool Success);
    public class ValidateDailyDataCommandValidator : AbstractValidator<ValidateDailyDataCommand>
    {
        public ValidateDailyDataCommandValidator()
        {

        }
    }
}
