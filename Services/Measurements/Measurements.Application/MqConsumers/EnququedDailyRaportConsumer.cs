namespace Measurements.Application.MqConsumers;

public class EnququedDailyRaportConsumer(ILogger<EnququedDailyRaportConsumer> logger, ISender sender) : IConsumer<EnqueueDailyRaportGenerationMessage>
{
    public async Task Consume(ConsumeContext<EnqueueDailyRaportGenerationMessage> context)
    {
        logger.LogInformation($"Recieved message: '{context.Message.RaportDate}'");

        ValidateDailyDataCommand command = new ValidateDailyDataCommand(context.Message.RaportDate);

        await sender.Send(command);
    }
}