using System;
using System.Text;
using RabbitMQ.Client;

namespace LogProducer
{
    internal class Program
    {
        private static void Main()
        {
            var factory = new ConnectionFactory
            {
                UserName = "ops0",
                Password = "ops0"
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.ExchangeDeclarePassive("logs");

            var props = channel.CreateBasicProperties();
            props.AppId = "MyLogs";
            props.Persistent = true;
            props.UserId = "ops0";

            Console.WriteLine("Please type your message.");
            Console.WriteLine("Type 'exit' to exit.");

            while (true)
            {
                var message = Console.ReadLine();
                if (message == "exit")
                {
                    connection.Close();
                    break;
                }

                props.MessageId = Guid.NewGuid().ToString("N");
                props.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "logs",
                    routingKey: "order.created",
                    basicProperties: props,
                    body: body);
                Console.WriteLine($"\t[x] Sent {message}");
            }
        }
    }
}