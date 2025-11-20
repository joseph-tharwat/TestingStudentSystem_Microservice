using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using TestManagment.Shared.Dtos;
using Microsoft.AspNetCore.Connections;
using TestManagment.Domain.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TestManagment.ApplicationLayer.Interfaces.Messaging;

namespace TestManagment.Infrastructure.RabbitMQ
{
    public class RabbitMqService: IEventPublisher, IDisposable 
    {
        private IConnection connection { get; set; }
        private readonly IOptions<RabbitMqSetings> config;

        public RabbitMqService(IOptions<RabbitMqSetings> config) 
        {
            this.config = config;

            var factory = new ConnectionFactory() { Uri = new Uri(config.Value.Uri) };
            connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        }

        public async Task PublishQuestionCreatedAsync<T>(T objectToSend, string oneOrManyHeaderValue)
        {
            await using var channel = await connection.CreateChannelAsync();
            await channel.QueueDeclareAsync(queue: config.Value.QueueName, exclusive: false);

            var msgBody = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(objectToSend));

            var props = new BasicProperties()
            {
                Headers = new Dictionary<string, object?>
                {
                    { config.Value.HeaderKey, oneOrManyHeaderValue }
                }
            };

            await channel.BasicPublishAsync(
                exchange: config.Value.ExchangeName,
                routingKey: config.Value.QueueName,
                true,
                basicProperties: props,
                body: msgBody);
        }

        public async Task PublishOneQuestionCreatedAsync(OneQuestionCreatedEvent question)
        {
            await PublishQuestionCreatedAsync(question, config.Value.HeaderValueOne);
        }
        public async Task PublishManyQuestionsCreatedAsync(ManyQuestionsCreatedEvent questions)
        {
            await PublishQuestionCreatedAsync(questions, config.Value.HeaderValueMany);
        }

        public void Dispose()
        {
            connection?.Dispose();
        }
    }
}
