using Confluent.Kafka;
using Microsoft.Extensions.Options;
using ServiceSchedule.Configurations;

namespace ServiceSchedule.Helpers.Producer
{
    public class ProducerHelper : IProducerHelper
    {
        private readonly KafkaProducerConfig _kafkaProducerConfig;
        public ProducerHelper(IOptions<KafkaProducerConfig> kafkaProducerConfig)
        {
            _kafkaProducerConfig = kafkaProducerConfig.Value;
        }
        public async Task<DeliveryResult<string, string>> ProduceAsync(string topic, Message<string, string> message, ConfigurationBase configurationBase, CancellationToken cancellationToken = default)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = configurationBase.BootstrapServers,
                ClientId = configurationBase.ClientId,
            };

            using (var producer = new ProducerBuilder<string, string>(config).Build())
            {
                var deliveryReport = await producer.ProduceAsync(topic, message);
                return deliveryReport;
            }
        }
    }
}