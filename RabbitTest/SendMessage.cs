using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitTest
{

    public class SampleMessage : BaseMessage
    {
        public string SampleText { get; set; }

    }


    public class BaseMessage
    {
        public Guid MessageId { get; set; } = Guid.NewGuid();
        public DateTime Creationtime { get; set; } = DateTime.Now;
    }
    public static class RabbitMQMessageBus
    {


        public static void SendMessage(BaseMessage message, string exchange, string queueName = "")
        {


            IConnection _connection;

            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };
            _connection = factory.CreateConnection();


            using (var channel = _connection.CreateModel())
            {

                var json = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(json);
                var Properties = channel.CreateBasicProperties();
                Properties.Persistent = true;

                if (!string.IsNullOrEmpty(queueName))
                {
                    channel.QueueDeclare(queue: queueName,
                  durable: true, exclusive: false, autoDelete: false,
                  arguments: null);


                    channel.BasicPublish(exchange: "", routingKey: queueName
                        , basicProperties: Properties, body: body);
                }
                else
                {

                    channel.ExchangeDeclare(exchange, ExchangeType.Fanout, true, false, null);

                    channel.BasicPublish(exchange: exchange, routingKey: "", basicProperties: Properties, body: body);
                }


            }

        }






    }


}
