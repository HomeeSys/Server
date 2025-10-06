using Devices.Application.Messages;
using MassTransit;

namespace Devices.Application.MqConsumers
{
    public class DeviceStatusMessageConsumer : IConsumer<DeviceStatusChangedMessage>
    {
        public Task Consume(ConsumeContext<DeviceStatusChangedMessage> context)
        {
            return Task.CompletedTask;
        }
    }
}
