namespace CommonServiceLibrary.GRPC.Client;

public class AiResponseClientGRPC
{
    private readonly GrpcChannel _channel;
    private readonly IConfiguration _configuration;
    private readonly AiResponser.AiResponserClient _client;

    public AiResponseClientGRPC(IConfiguration configuration)
    {
        _configuration = configuration;

        var httpClient = new HttpClient
        {
            Timeout = Timeout.InfiniteTimeSpan
        };

        _channel = GrpcChannel.ForAddress(_configuration.GetValue<string>("GRPC:AiAssistant")!, new GrpcChannelOptions
        {
            HttpClient = httpClient
        });

        _client = new AiResponser.AiResponserClient(_channel);

        TypeAdapterConfig<MeasurementDescriptionResponse, AiResponseGRPC>
            .NewConfig()
            .Map(dest => dest.Description, src => src.Description);
    }

    public async Task<AiResponseGRPC> GenerateDescriptionForDailyRaport(string input)
    {
        MeasurementDescriptionResponse? response = await _client.GenerateDailyRaportDescriptionAsync(new MeasurementDescriptionRequest() { Input = input });

        AiResponseGRPC modelGrpc = response.Adapt<AiResponseGRPC>();

        return modelGrpc;
    }
}
