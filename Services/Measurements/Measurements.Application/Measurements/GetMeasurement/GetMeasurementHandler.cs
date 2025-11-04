using CommonServiceLibrary.GRPC.Client;
using Services.Helpers;

namespace Measurements.Application.Measurements.GetMeasurement;

public class GetMeasurementsInfoHandler(MeasurementsDBContext context) : IRequestHandler<GetMeasurementsInfoCommand, GetMeasurementsInfoResponse>
{
    public async Task<GetMeasurementsInfoResponse> Handle(GetMeasurementsInfoCommand request, CancellationToken cancellationToken)
    {
        var min = await context.GetMinimalDate();
        if (min == null)
        {
            min = DateTime.Now;
        }

        var max = await context.GetMaximalDate();
        if (max == null)
        {
            max = DateTime.Now;
        }

        var response = new GetMeasurementsInfoResponse(new MeasurementsInfo()
        {
            MinDate = ((DateTime)min).ToLocalTime(),
            MaxDate = ((DateTime)max).ToLocalTime()
        });

        return response;
    }
}

public class GetAllMeasurementsHandler(MeasurementsDBContext context) : IRequestHandler<GetMeasurementSetsCommand, GetAllMeasurementSetsResponse>
{
    public async Task<GetAllMeasurementSetsResponse> Handle(GetMeasurementSetsCommand request, CancellationToken cancellationToken)
    {
        var result = await context.GetMeasurements();

        var response = new GetAllMeasurementSetsResponse(result);

        return response;
    }
}

public class GetMeasurementsQueryHandler(MeasurementsDBContext context, DevicesClientGRPC devicesGRPC) : IRequestHandler<GetMeasurementsQueryCommand, PaginatedList<QueryableMeasurementSet>>
{
    public async Task<PaginatedList<QueryableMeasurementSet>> Handle(GetMeasurementsQueryCommand request, CancellationToken cancellationToken)
    {
        List<Guid> filteredDeviceNumbers = null;

        var devices = devicesGRPC.GetAllDevices().Result.AsQueryable();

        if (!string.IsNullOrEmpty(request.Search))
        {
            devices = devices.Where(x => x.DeviceNumber.ToString().Contains(request.Search) || x.Location.Name.Contains(request.Search) || x.Name.Contains(request.Search));
            filteredDeviceNumbers = devices.Select(x => x.DeviceNumber).ToList();
        }

        var solidDevices = devices.AsEnumerable();

        int absoluteCount = await context.GetAbsoluteCount();

        IQueryable<MeasurementSet>? measurementsQuery = await context.GetMeasurementSetsQuery(filteredDeviceNumbers, request.DateFrom, request.DateTo, request.SortOrder, request.Page, request.PageSize);

        var measurementsCombined = measurementsQuery.Select(x => new QueryableMeasurementSet()
        {
            ID = x.Id,
            DeviceNumber = x.DeviceNumber,
            DeviceName = "",
            Location = "",
            RegisterDate = x.RegisterDate,
            Temperature = x.Temperature,
            Humidity = x.Humidity,
            CO2 = x.CO2,
            VOC = x.VOC,
            ParticulateMatter1 = x.ParticulateMatter1,
            ParticulateMatter2v5 = x.ParticulateMatter2v5,
            ParticulateMatter10 = x.ParticulateMatter10,
            Formaldehyde = x.Formaldehyde,
            CO = x.CO,
            O3 = x.O3,
            Ammonia = x.Ammonia,
            Airflow = x.Airflow,
            AirIonizationLevel = x.AirIonizationLevel,
            O2 = x.O2,
            Radon = x.Radon,
            Illuminance = x.Illuminance,
            SoundLevel = x.SoundLevel
        });

        var measurements = await PaginatedList<QueryableMeasurementSet>.Create(measurementsCombined, request.Page, request.PageSize, absoluteCount);

        measurements.Items.ForEach(x =>
        {
            x.DeviceName = solidDevices.FirstOrDefault(y => y.DeviceNumber == x.DeviceNumber)!.Name;
            x.Location = solidDevices.FirstOrDefault(y => y.DeviceNumber == x.DeviceNumber)!.Location.Name;
        });

        return measurements;
    }
}

public class GetAllMeasurementSetsFromDeviceByDayHandler(MeasurementsDBContext context) : IRequestHandler<GetAllMeasurementSetsFromDeviceByDayCommand, GetAllMeasurementSetsResponse>
{
    public async Task<GetAllMeasurementSetsResponse> Handle(GetAllMeasurementSetsFromDeviceByDayCommand request, CancellationToken cancellationToken)
    {
        var result = await context.GetMeasurementsFromDay(request.DeviceNumber, request.Day);

        var response = new GetAllMeasurementSetsResponse(result);

        return response;
    }
}

public class GetAllMeasurementSetsFromDeviceByWeekHandler(MeasurementsDBContext context) : IRequestHandler<GetAllMeasurementSetsFromDeviceByWeekCommand, GetAllMeasurementSetsResponse>
{
    public async Task<GetAllMeasurementSetsResponse> Handle(GetAllMeasurementSetsFromDeviceByWeekCommand request, CancellationToken cancellationToken)
    {
        var result = await context.GetMeasurementsFromWeek(request.DeviceNumber, request.Week);

        var response = new GetAllMeasurementSetsResponse(result);

        return response;
    }
}

public class GetAllMeasurementSetsFromDeviceByMonthHandler(MeasurementsDBContext context) : IRequestHandler<GetAllMeasurementSetsFromDeviceByMonthCommand, GetAllMeasurementSetsResponse>
{
    public async Task<GetAllMeasurementSetsResponse> Handle(GetAllMeasurementSetsFromDeviceByMonthCommand request, CancellationToken cancellationToken)
    {
        var result = await context.GetMeasurementsFromMonth(request.DeviceNumber, request.Month);

        var response = new GetAllMeasurementSetsResponse(result);

        return response;
    }
}

public class GetAllMeasurementsFromDeviceHandler(MeasurementsDBContext context) : IRequestHandler<GetMeasurementSetsFromDeviceCommand, GetAllMeasurementSetsResponse>
{
    public async Task<GetAllMeasurementSetsResponse> Handle(GetMeasurementSetsFromDeviceCommand request, CancellationToken cancellationToken)
    {
        var result = await context.GetMeasurements(request.DeviceNumber);

        var response = new GetAllMeasurementSetsResponse(result);

        return response;
    }
}

public class GetMeasurementByIDHandler(MeasurementsDBContext context) : IRequestHandler<GetMeasurementSetCommand, GetMeasurementSetResponse>
{
    public async Task<GetMeasurementSetResponse> Handle(GetMeasurementSetCommand request, CancellationToken cancellationToken)
    {
        var result = await context.GetMeasurement(request.ID);

        var dto = result.Adapt<MeasurementSetDTO>();

        var response = new GetMeasurementSetResponse(dto);

        return response;
    }
}