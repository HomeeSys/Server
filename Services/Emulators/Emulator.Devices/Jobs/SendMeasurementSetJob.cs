namespace Emulator.Devices.Jobs;

internal class SendMeasurementSetJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        //try
        //{
        //    var deviceObject = context.JobDetail.JobDataMap[nameof(DeviceModel)];
        //    var measurementHttpClientObject = context.JobDetail.JobDataMap[nameof(HttpClient)];
        //    var endpointObject = context.JobDetail.JobDataMap["Endpoint"];

        //    if (deviceObject is DeviceModel deviceModel && measurementHttpClientObject is HttpClient httpClient && endpointObject is string endpoint)
        //    {
        //        var data = deviceModel.GenerateMeasurementSet();
        //        object dataDTO = null;
        //        var json = JsonConvert.SerializeObject(dataDTO);
        //        var payload = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        //        var postResponse = httpClient.PostAsync(endpoint, payload);

        //        var response = postResponse.Result;

        //        if (response.IsSuccessStatusCode == false)
        //        {
        //            Console.WriteLine("Failed to send new measurement!");
        //            throw new Exception();
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine("Failed to retrievie device!");
        //        throw new Exception();
        //    }
        //}
        //catch (Exception ex)
        //{
        //    throw new JobExecutionException(ex, true);
        //}
    }
}
