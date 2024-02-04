using Confluent.Kafka;
using ServiceSchedule.Configurations;

namespace ServiceSchedule.Helpers.Producer
{
    public interface IProducerHelper
    {
        Task<DeliveryResult<string, string>> ProduceAsync(string topic, Message<string, string> message, ConfigurationBase configurationBase, CancellationToken cancellationToken = default(CancellationToken));
    }
}
