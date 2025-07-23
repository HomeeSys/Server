using Measurements.Application.Measurements.ValidateDailyData;
using Microsoft.Extensions.Logging;

namespace Measurements.Application.MqConsumers;

public class EnququedDailyRaportConsumer(ILogger<EnququedDailyRaportConsumer> logger, ISender sender) : IConsumer<EnqueueDailyRaportGenerationMessage>
{
    public async Task Consume(ConsumeContext<EnqueueDailyRaportGenerationMessage> context)
    {
        logger.LogInformation($"Recieved message: '{context.Message.ID}'");

        ValidateDailyDataCommand command = new ValidateDailyDataCommand(context.Message.RaportDate);

        await sender.Send(command);
    }
}
