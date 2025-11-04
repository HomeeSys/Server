namespace Measurements.Application.Measurements.ValidateDailyData
{
    public class ValidateDailyReportHandler(MeasurementsDBContext dbContext, IPublishEndpoint publisher) : IRequestHandler<ValidateDailyDataCommand, ValidateDailyDataResponse>
    {
        public async Task<ValidateDailyDataResponse> Handle(ValidateDailyDataCommand request, CancellationToken cancellationToken)
        {
            var items = await dbContext.GetAllMeasurementsFromDay(request.Date);

            bool areValid = AreValid(items);
            if (areValid == false)
            {
                //  What to do?
                return new ValidateDailyDataResponse(areValid);
            }

            var message = new GenerateDailyReportMessage() { RaportDate = request.Date };

            await publisher.Publish(message, cancellationToken);

            return new ValidateDailyDataResponse(areValid);
        }

        private bool AreValid(IEnumerable<MeasurementSet> measurementSets)
        {
            //  Thise measurement set should have data for whole day, so 1 per hour -> 24 measurements per device.
            //  Each measuremetn should be concluded in unique hour.

            if (measurementSets == null || !measurementSets.Any())
            {
                return false;
            }

            //  All devices
            var distinceDeviceNumbers = measurementSets.DistinctBy(x => x.DeviceNumber).Select(x => x.DeviceNumber);

            //  Ammount is not valid
            if (measurementSets.Count() != distinceDeviceNumbers.Count() * 24)
            {
                return false;
            }

            foreach (var deviceNumber in distinceDeviceNumbers)
            {
                var measurementsPerDevice = measurementSets.Where(x => x.DeviceNumber == deviceNumber);

                //  Every device should have 24 measurements
                if (measurementsPerDevice == null || measurementsPerDevice.Count() != 24)
                {
                    return false;
                }

                //  Measurement should have distince 24 hours
                var distinctHourlyMeasurements = measurementsPerDevice.DistinctBy(x => x.RegisterDate.ToString("HH"));
                if (distinctHourlyMeasurements == null || distinctHourlyMeasurements.Count() != 24)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
