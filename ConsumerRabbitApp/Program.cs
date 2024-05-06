
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory() { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "Inbox",
    durable: false, exclusive: false,
    autoDelete: false,
    arguments: null);

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (sender, e) =>
{

    var message = Encoding.UTF8.GetString(e.Body.ToArray());
    Console.WriteLine(message);

   
    SendMessageToQueueWithTTL("AnotherQueue", message, 10000);
};

channel.BasicConsume("Inbox", autoAck: true, consumer: consumer);
Console.ReadLine();

void SendMessageToQueueWithTTL(string queueName, string message, int ttlMilliseconds)
{
    using var sendingChannel = connection.CreateModel();
    sendingChannel.QueueDeclare(queue: queueName,
        durable: false, exclusive: false,
        autoDelete: false,
        arguments: new Dictionary<string, object>
        {
            {"x-message-ttl", ttlMilliseconds}
        });

    var body = Encoding.UTF8.GetBytes(message);
    sendingChannel.BasicPublish(exchange: "",
                                routingKey: queueName,
                                basicProperties: null,
                                body: body);
    Console.WriteLine($"پیام به صف {queueName} با مدت زمان TTL {ttlMilliseconds} میلی‌ثانیه ارسال شد.");
}
