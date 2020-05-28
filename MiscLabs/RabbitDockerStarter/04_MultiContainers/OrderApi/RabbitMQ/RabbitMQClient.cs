using System;
using System.Text;
using RabbitMQ.Client;

namespace OrderApi.RabbitMQ
{
    public class RabbitMQClient
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQClient(IConnection connection)
        {
            _connection = connection;
            _channel = _connection.CreateModel();
        }

        public void Publish(string exchange, string routingKey, string payload)
        {
            var props = _channel.CreateBasicProperties();
            props.AppId = "OrderApi";
            props.Persistent = true;
            props.UserId = "ops0";
            props.MessageId = Guid.NewGuid().ToString("N");
            props.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
            var body = Encoding.UTF8.GetBytes(payload);
            _channel.BasicPublish(exchange, routingKey, props, body);
        }

        public void CloseConnection()
        {
            _connection.Close();
        }
    }
}
