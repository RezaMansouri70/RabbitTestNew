using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factore = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
                Port = 5672
            };

            var connection = factore.CreateConnection();
            var channel = connection.CreateModel();

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (ch, ea) =>
             {
                 var body = ea.Body.ToArray();
                 var result = Encoding.UTF8.GetString(body);
                 channel.BasicAck(ea.DeliveryTag, false);
                 Console.WriteLine(result);
             };

            var consumerTag = channel.BasicConsume("test_queue", false, "", false, false, null, consumer);
            Console.ReadLine();
        }
    }
}
