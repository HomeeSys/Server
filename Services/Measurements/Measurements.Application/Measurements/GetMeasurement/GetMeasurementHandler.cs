using CommonServiceLibrary.GRPC.Types.Devices;

namespace Measurements.Application.Measurements.GetMeasurement;

public class GetMeasurementByIDHandler(Container cosmosContainer) : IRequestHandler<GetMeasurementCommand, GetMeasurementResponse>
{
    public async Task<GetMeasurementResponse> Handle(GetMeasurementCommand request, CancellationToken cancellationToken)
    {
        var result = cosmosContainer
            .GetItemLinqQueryable<Measurement>(allowSynchronousQueryExecution: true)
            .Where(x => x.ID == request.MeasurementID)
            .AsEnumerable()
            .FirstOrDefault();

        if (result is null)
        {
            throw new EntityNotFoundException(nameof(Measurement), request.MeasurementID);
        }

        var dto = result.Adapt<DefaultMeasurementDTO>();

        var response = new GetMeasurementResponse(dto);

        await Task.CompletedTask;

        return response;
    }
}

public class GetAllMeasurementHandler(Container cosmosContainer) : IRequestHandler<GetAllMeasurementCommand, GetAllMeasurementResponse>
{
    public async Task<GetAllMeasurementResponse> Handle(GetAllMeasurementCommand request, CancellationToken cancellationToken)
    {
        var orederedQuery = cosmosContainer.GetItemLinqQueryable<Measurement>(allowSynchronousQueryExecution: true);
        var query = orederedQuery.AsQueryable();

        //  Filter
        if (request.DeviceNumber is not null)
        {
            query = query.Where(x => x.DeviceNumber == request.DeviceNumber);
        }

        if (request.DateStart is not null)
        {
            query = query.Where(x => x.MeasurementCaptureDate >= request.DateStart);
        }

        if (request.DateEnd is not null)
        {
            query = query.Where(x => x.MeasurementCaptureDate <= request.DateEnd);
        }

        if (request.LocationHash is not null)
        {
            query = query.Where(x => x.LocationHash == request.LocationHash);
        }

        //  Order
        if (request.SortOrder is not null)
        {
            if (request.SortOrder == "asc")
            {
                query = query.OrderBy(x => x.MeasurementCaptureDate);
            }
            else
            {
                query = query.OrderByDescending(x => x.MeasurementCaptureDate);
            }
        }

        //  Paginated list
        int absoluteCount = orederedQuery.Count();
        var paginatedModels = await PaginatedList<Measurement>.Create(query, request.Page, request.PageSize, absoluteCount);

        var paginatedDtos = await PaginatedDTOList<Measurement, DefaultMeasurementDTO>.Create(query, request.Page, request.PageSize, absoluteCount);

        var response = new GetAllMeasurementResponse(paginatedDtos);
        return response;
    }
}

public class GetAllCombinedMeasurementHandler(Container cosmosContainer, DevicesService.DevicesServiceClient devicesGrpc) : IRequestHandler<GetAllCombinedMeasurementCommand, GetAllCombinedMeasurementResponse>
{
    public async Task<GetAllCombinedMeasurementResponse> Handle(GetAllCombinedMeasurementCommand request, CancellationToken cancellationToken)
    {
        var orederedQuery = cosmosContainer.GetItemLinqQueryable<Measurement>(allowSynchronousQueryExecution: true);
        var query = orederedQuery.AsQueryable();

        var devices = new List<DeviceGRPC>();
        using (var call = devicesGrpc.GetAllDevices(new DeviceAllRequest()))
        {
            while (await call.ResponseStream.MoveNext(cancellationToken))
            {
                var current = call.ResponseStream.Current;
                var dev = current.Adapt<DeviceGRPC>();
                devices.Add(dev);
            }
        }

        var locations = new List<LocationGRPC>();
        using (var call = devicesGrpc.GetAllLocations(new LocationAllRequest()))
        {
            while (await call.ResponseStream.MoveNext(cancellationToken))
            {
                var current = call.ResponseStream.Current;
                var loc = current.Adapt<LocationGRPC>();
                locations.Add(loc);
            }
        }

        //  Filter
        if (request.DeviceNumber is not null)
        {
            query = query.Where(x => x.DeviceNumber == request.DeviceNumber);
        }

        if (request.DateStart is not null)
        {
            query = query.Where(x => x.MeasurementCaptureDate >= request.DateStart);
        }

        if (request.DateEnd is not null)
        {
            query = query.Where(x => x.MeasurementCaptureDate <= request.DateEnd);
        }

        if (request.LocationHash is not null)
        {
            query = query.Where(x => x.LocationHash == request.LocationHash);
        }

        //  Order
        if (request.SortOrder is not null)
        {
            if (request.SortOrder == "asc")
            {
                query = query.OrderBy(x => x.MeasurementCaptureDate);
            }
            else
            {
                query = query.OrderByDescending(x => x.MeasurementCaptureDate);
            }
        }

        //  Paginated list
        int absoluteCount = orederedQuery.Count();

        var totalCount = query.Count();
        var measurementCombinedDtos = query.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToList().Select(x =>
        {
            var device = devices.FirstOrDefault(y => y.DeviceNumber == x.DeviceNumber);
            var location = locations.FirstOrDefault(y => y.Hash == x.LocationHash);

            return new CombinedMeasurementDTO(
                x.ID,
                x.MeasurementCaptureDate,
                (device != null) ? device.ID : -1,
                (device != null) ? device.Name : "",
                x.DeviceNumber,
                x.LocationHash,
                (location != null) ? location.Name : "",
                x.Temperature,
                x.Humidity,
                x.CarbonDioxide,
                x.VolatileOrganicCompounds,
                x.ParticulateMatter1,
                x.ParticulateMatter2v5,
                x.ParticulateMatter10,
                x.Formaldehyde,
                x.CarbonMonoxide,
                x.Ozone,
                x.Ammonia,
                x.Airflow,
                x.AirIonizationLevel,
                x.Oxygen,
                x.Radon,
                x.Illuminance,
                x.SoundLevel
                );
        }).ToList();

        var paginatedDtos = new PaginatedList<CombinedMeasurementDTO>(measurementCombinedDtos, request.Page, request.PageSize, totalCount, absoluteCount);

        var response = new GetAllCombinedMeasurementResponse(paginatedDtos);

        return response;
    }
}
