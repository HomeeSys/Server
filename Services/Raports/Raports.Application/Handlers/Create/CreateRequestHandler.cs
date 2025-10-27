namespace Raports.Application.Handlers.Create;

public class CreateRequestHandler(RaportsDBContext dBContext) : IRequestHandler<CreateRequestCommand, ReadRequestResponse>
{
    public async Task<ReadRequestResponse> Handle(CreateRequestCommand request, CancellationToken cancellationToken)
    {
        var createDTO = request.CreateDTO;

        var periodEntity = await dBContext.Periods.FirstOrDefaultAsync(x => x.Name == createDTO.PeriodName, cancellationToken);
        if (periodEntity is null)
        {
            throw new EntityNotFoundException(nameof(Period), createDTO.PeriodName);
        }

        var statusEntity = await dBContext.RequestStatuses.FirstOrDefaultAsync(x => x.Name == "Pending", cancellationToken);
        if (statusEntity is null)
        {
            throw new EntityNotFoundException(nameof(RequestStatus), "Pending");
        }

        //  Remove everyting after hours.
        DateTime endDate = new DateTime(
            createDTO.EndDate.Year,
            createDTO.EndDate.Month,
            createDTO.EndDate.Day,
            createDTO.EndDate.Hour,
            0,
            0);

        DateTime startDate = new DateTime(
            createDTO.StartDate.Year,
            createDTO.StartDate.Month,
            createDTO.StartDate.Day,
            createDTO.StartDate.Hour,
            0,
            0);


        //  Calculate period
        int estimatedHours = (int)(endDate - startDate).Duration().TotalHours;

        if (periodEntity.Hours != estimatedHours)
        {
            throw new EntityNotFoundException(nameof(RequestStatus), $"Estimated hours: '{estimatedHours}'");
        }

        var requestEntity = await dBContext.Requests
            .Include(x => x.Period)
            .FirstOrDefaultAsync(x => x.StartDate == createDTO.StartDate && x.EndDate == createDTO.EndDate && x.Period.Name == createDTO.PeriodName);
        if (requestEntity is not null)
        {
            throw new DuplicateEntityException(nameof(Request), request.CreateDTO);
        }

        Request newReqeust = new Request()
        {
            RequestCreationDate = DateTime.Now,
            StartDate = startDate,
            EndDate = endDate,
            PeriodID = periodEntity.ID,
            StatusID = statusEntity.ID
        };

        await dBContext.Requests.AddAsync(newReqeust, cancellationToken);
        await dBContext.SaveChangesAsync();

        var dto = newReqeust.Adapt<DefaultRequestDTO>();

        var response = new ReadRequestResponse(dto);

        return response;
    }
}
