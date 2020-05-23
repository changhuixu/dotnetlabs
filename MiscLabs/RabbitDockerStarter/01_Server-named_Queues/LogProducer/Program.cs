using System;
using System.Text;
using RabbitMQ.Client;

namespace LogProducer
{
    internal class Program
    {
        private static void Main()
        {
            var factory = new ConnectionFactory();
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

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
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "logs",
                    routingKey: "",
                    basicProperties: null,
                    body: body);
                Console.WriteLine($"\t[x] Sent {message}");
            }
        }
    }
}