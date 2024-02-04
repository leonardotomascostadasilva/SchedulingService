using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceSchedule.Configurations;
using ServiceSchedule.Helpers.Producer;
using ServiceSchedule.Infrastructure;
using ServiceSchedule.Infrastructure.Entities;
using ServiceSchedule.Infrastructure.Repository;
using System.Net;

namespace ServiceSchedule.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScheduleController : ControllerBase
    {

        private readonly ILogger<ScheduleController> _logger;
        private readonly KafkaProducerConfig _kafkaProducerConfig;
        private readonly KafkaProducerAccountConfiguration _kafkaProducerAccountConfiguration;
        private readonly ApplicationDbContext _dbContext;
        private readonly IProducerHelper _producerHelper;
        private readonly IRepository _repository;

        public ScheduleController(ILogger<ScheduleController> logger, IOptions<KafkaProducerConfig> kafkaProducerConfig, IProducerHelper producerHelper, IOptions<KafkaProducerAccountConfiguration> kafkaProducerAccountConfiguration, ApplicationDbContext dbContext, IRepository repository)
        {
            _logger = logger;
            _kafkaProducerConfig = kafkaProducerConfig.Value;
            _producerHelper = producerHelper;
            _kafkaProducerAccountConfiguration = kafkaProducerAccountConfiguration.Value;
            _dbContext = dbContext;
            _repository = repository;
        }

        [HttpPost]
        [ProducesResponseType(typeof(DeliveryResult<string, string>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> PostSchedule()
        {
            string content = @"{
                    ""account"": ""12345"",
                    ""id"": 12345678,
                    ""origin"": ""web""
                }";
            var data = new Message<string, string> { Value = content, Key = "123456" };

            var result = await _producerHelper.ProduceAsync("process-schedule", data, _kafkaProducerConfig);

            return Ok(result);
        }

        [HttpPost("account")]
        [ProducesResponseType(typeof(DeliveryResult<string, string>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> PostScheduleAccount([FromBody] Message message)
        {
            _dbContext.Messages.Add(message);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("account")]
        [ProducesResponseType(typeof(DeliveryResult<string, string>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetScheduleAccount(string topic)
        {
            
            var results = await _repository.GetAsync<Message>(e => e.Topic == topic);

            foreach (var item in results)
            {
                var data = new Message<string, string> { Value = item.Data,  Key = item.Key };

                var result = await _producerHelper.ProduceAsync(item.Topic, data, _kafkaProducerConfig);
            }
       
            return Ok(results);
        }
    }
}