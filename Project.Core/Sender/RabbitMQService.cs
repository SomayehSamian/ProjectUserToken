using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Core.Contract;
using RabbitMQ.Client;
using Newtonsoft.Json;

namespace Project.Core.Sender
{
    public sealed class RabbitMQService: IRabbitMQService
    {
        private readonly string _rabbitMQHost;
        private readonly string _queueName;

        public RabbitMQService(string rabbitMQHost, string queueName)
        {
            _rabbitMQHost = rabbitMQHost;
            _queueName = queueName;
        }

        public void SendMessage<T>(T message)
        {
            var factory = new ConnectionFactory() { HostName = _rabbitMQHost };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);


            var jsonMessage = JsonConvert.SerializeObject(message);

            var body = Encoding.UTF8.GetBytes(jsonMessage);

            channel.BasicPublish(exchange: "",
                routingKey: _queueName,
                body: body);
           




        }

    }
}
