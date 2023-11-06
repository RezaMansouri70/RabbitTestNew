using RabbitMQ.Client;
using System;
using System.Text;

namespace RabbitTest
{
    class Program
    {
        static void Main(string[] args)
        {

            while (true)
            {
                Console.Write("Inter Message : ");
                var messageString = Console.ReadLine();

                SampleMessage  sampleMessage = new SampleMessage
                {
                    Creationtime = DateTime.Now,
                    MessageId = Guid.NewGuid(),
                    SampleText = messageString,
                };
                RabbitMQMessageBus.SendMessage(sampleMessage, "", "test_queue");

            }

        }
    }
}
