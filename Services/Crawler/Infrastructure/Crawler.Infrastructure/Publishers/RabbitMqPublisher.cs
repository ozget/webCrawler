using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crawler.Application.IServices;
using Crawler.Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;



namespace Crawler.Infrastructure.Publishers
{

    public class RabbitMQPublisher<T> : INewsPublisher<T>
    {
        private readonly RabbitMqSettings _rabbitMqSetting;

        public RabbitMQPublisher(IOptions<RabbitMqSettings> rabbitMqSetting)
        {
            _rabbitMqSetting = rabbitMqSetting.Value;

        }

        public async Task PublishMessageAsync(T message, string queueName)
        {

            var factory = new ConnectionFactory
            {
                HostName = _rabbitMqSetting.HostName,
                UserName = _rabbitMqSetting.UserName,
                Password = _rabbitMqSetting.Password
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var messageJson = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(messageJson);


            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;// bu şekilde rabbitmq retartında bile mesaj silinmez
            
            await Task.Run(() =>
                channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: properties, body: body)
                );
        }

    }
}
