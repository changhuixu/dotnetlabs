using System;
using System.Text;
using RabbitMQ.Client;

namespace OrderApi.RabbitMQ
{
    public class RabbitMqClient
    {
        private readonly ConnectionFactory _connectionFactory;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqClient()
        {
            var rabbitHostName = Environment.GetEnvironmentVariable("RABBIT_HOSTNAME");
            _connectionFactory = new ConnectionFactory
            {
                HostName = rabbitHostName ?? "localhost",
                Port = 5672,
                UserName = "ops0",
                Password = "ops0"
            };
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void Publish(string exchange, string routingKey, string payload)
        {
            var props = _channel.CreateBasicProperties();
            props.AppId = "OrderApi";
            props.Persistent = true;
            props.UserId = _connectionFactory.UserName;
            props.MessageId = Guid.NewGuid().ToString("N");
            props.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
            var body = Encoding.UTF8.GetBytes(payload);
            _channel.BasicPublish(exchange, routingKey, props, body);
        }

        public void Close()
        {
            _connection.Close();
        }
    }
}
