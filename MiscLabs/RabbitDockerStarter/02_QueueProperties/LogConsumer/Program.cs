using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace LogConsumer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var factory = new ConnectionFactory
            {
                UserName = "ops1",
                Password = "ops1"
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.ExchangeDeclarePassive("logs");

            var queueName = args.Length == 0 || string.IsNullOrWhiteSpace(args[0]) ? "mytest" : args[0];
            channel.QueueDeclarePassive(queueName);
            Console.WriteLine($"Queue [{queueName}] is waiting for messages.");

            var messageCount = channel.MessageCount(queueName);
            if (messageCount > 0)
            {
                Console.WriteLine($"\tDetected {messageCount} message(s).");
            }

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (bc, ea) =>
            {
                var t = DateTimeOffset.FromUnixTimeMilliseconds(ea.BasicProperties.Timestamp.UnixTime);
                Console.WriteLine($"{t.LocalDateTime:O} ID=[{ea.BasicProperties.MessageId}]");
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine($"\tProcessing message: '{message}'.");

                if (ea.BasicProperties.UserId != "ops0")
                {
                    Console.WriteLine($"\tIgnored a message sent by [{ea.BasicProperties.UserId}].");
                    return;
                }

                try
                {
                    Thread.Sleep((new Random().Next(1, 3)) * 1000);
                    var model = ((IBasicConsumer)bc).Model;
                    model.BasicAck(ea.DeliveryTag, false);
                }
                catch (AlreadyClosedException)
                {
                    Console.WriteLine("RabbitMQ is closed!");
                }
            };
            channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

            Console.WriteLine("Press [enter] to exit.");
            Console.ReadLine();
            connection.Close();
        }
    }
}
