using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace EmailWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            var rabbitHostName = Environment.GetEnvironmentVariable("RABBIT_HOSTNAME");
            _logger.LogInformation(rabbitHostName);

            _connectionFactory = new ConnectionFactory
            {
                HostName = rabbitHostName ?? "localhost",
                Port = 5672,
                UserName = "ops0",
                Password = "ops0",
                DispatchConsumersAsync = true
            };
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            stoppingToken.ThrowIfCancellationRequested();

            const string queueName = "emailworker";
            _channel.QueueDeclarePassive(queueName);
            _logger.LogInformation($"Queue [{queueName}] is waiting for messages.");

            var messageCount = _channel.MessageCount(queueName);
            if (messageCount > 0)
            {
                _logger.LogInformation($"\tDetected {messageCount} message(s).");
            }

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (bc, ea) =>
            {
                if (ea.BasicProperties.UserId != "ops0")
                {
                    _logger.LogInformation($"\tIgnored a message sent by [{ea.BasicProperties.UserId}].");
                    return;
                }

                try
                {
                    var t = DateTimeOffset.FromUnixTimeMilliseconds(ea.BasicProperties.Timestamp.UnixTime);
                    _logger.LogInformation($"{t.LocalDateTime:O} ID=[{ea.BasicProperties.MessageId}]");
                    var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                    _logger.LogInformation($"\tProcessing order: '{message}'.");

                    await Task.Delay(new Random().Next(1, 3) * 1000, stoppingToken);
                    var order = JsonSerializer.Deserialize<Order>(message);
                    _logger.LogInformation($"\tOrder #[{order.Id}] confirmation sent to [{order.Email}].");

                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (AlreadyClosedException)
                {
                    _logger.LogInformation("RabbitMQ is closed!");
                }
            };
            _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

            await Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
            _connection.Close();
        }
    }

    public class Order
    {
        public int Id { get; set; }
        public string Email { get; set; }
    }
}
