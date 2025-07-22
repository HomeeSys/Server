namespace AIAssistant.GRPC.Services
{
    public class AiResponseService : AiResponser.AiResponserBase
    {
        private readonly ILogger<AiResponseService> _logger;
        private readonly OllamaRaportChat _olamaChat;
        public AiResponseService(ILogger<AiResponseService> logger, OllamaRaportChat olamaChat)
        {
            _olamaChat = olamaChat;
            _logger = logger;
        }

        public override async Task<MeasurementDescriptionResponse> GenerateDailyRaportDescription(MeasurementDescriptionRequest request, ServerCallContext context)
        {
            string result = await _olamaChat.GenerateDescription(request.Input);

            MeasurementDescriptionResponse response = new MeasurementDescriptionResponse() { Description = result };

            return response;
        }
    }
}
