
using Elastic.Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using System;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using IConnection = RabbitMQ.Client.IConnection;
using Elastic.Application.Constants;
using Elastic.Domain.Entities;
using Elastic.Application.Services;

namespace Elastic.Infrastructure.Consumer
{
    public class RabbitMqConsumerService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly RabbitMqSettings _rabbitMqSetting;
        private IConnection _connection;
        private IModel _channel;
        private readonly ILogger<RabbitMqConsumerService> _logger;

        public RabbitMqConsumerService(IOptions<RabbitMqSettings> rabbitMqSetting, IServiceProvider serviceProvider, ILogger<RabbitMqConsumerService> logger)
        {
            _rabbitMqSetting = rabbitMqSetting.Value;
            _serviceProvider = serviceProvider;
            _logger= logger;


            var factory = new ConnectionFactory
            {
                HostName = _rabbitMqSetting.HostName,
                UserName = _rabbitMqSetting.UserName,
                Password = _rabbitMqSetting.Password
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            StartConsuming(MessageQueueNames.NewsQueue, stoppingToken);
            await Task.CompletedTask;
        }

        private void StartConsuming(string queueName, CancellationToken cancellationToken)
        {
            if (_channel == null) 
                throw new InvalidOperationException("Channel is not initialized.");

            _channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);



                bool processedSuccessfully = false;
                try
                {
                    processedSuccessfully = await ProcessMessageAsync(message);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Haber işlenirken bir hata oluştu {queueName}: {ex}");
                }

                if (processedSuccessfully)// başarısızsa bir kez daha deniyor
                {
                    _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
                else
                {
                    _channel.BasicReject(deliveryTag: ea.DeliveryTag, requeue: true);
                }

            };

            _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
        }

        private async Task<bool> ProcessMessageAsync(string news)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var newEntity = JsonConvert.DeserializeObject<NewEntity>(news);

                    if (string.IsNullOrEmpty(newEntity?.Title) || newEntity == null)
                    {
                        _logger.LogWarning("Gelen Haber boş ya da başlıksız: {news}", news);
                        return true;
                    }

                    var newsService = scope.ServiceProvider.GetRequiredService<IElasticService>();
                    await newsService.SaveAsync(newEntity);
                    _logger.LogInformation("Haber başarıyla kaydedildi: {Title}", newEntity.Title);
                    return true;

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error {ex.Message}");
                return false;
            }
        }
     
        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
