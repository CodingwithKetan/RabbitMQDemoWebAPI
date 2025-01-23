using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQWebAPI.Helpers;
using RabbitMQWebAPI.Models;

namespace RabbitMQWebAPI.RabbitMQ.Impl;

public class RabbitMQConsumer : IRabbitMQConsumer
{
    private readonly string _host;
    private readonly string _queueName;
    private readonly string _username;
    private readonly string _password;
    private readonly int _port;

    public RabbitMQConsumer()
    {
        var config = ConfigHelper.GetRabbitMQConfig();
        _host = config.Host;
        _queueName = config.QueueName;
        _username = config.Username;
        _password = config.Password;
        _port = config.Port;
    }

    public void ConsumeOrder(CancellationToken cancellationToken)
    {
        var factory = new ConnectionFactory()
        {
            HostName = _host,
            UserName = _username,
            Password = _password,
            Port = _port
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var order = JsonConvert.DeserializeObject<Order>(message);
            Console.WriteLine(" [x] Received order: {0}, Customer: {1}", order.OrderId, order.CustomerName);
        };

        channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
        
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Task.Delay(1000, cancellationToken).Wait();
            }
        }
        catch (OperationCanceledException)
        {
            // Graceful shutdown
            Console.WriteLine(" [*] Consumer stopped.");
        }
    }
}