using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQWebAPI.Helpers;
using RabbitMQWebAPI.Models;

namespace RabbitMQWebAPI.RabbitMQ.Impl;


public class RabbitMQPublisher : IRabbitMQPublisher
{
    private readonly string _host;
    private readonly string _queueName;
    private readonly string _username;
    private readonly string _password;
    private readonly int _portNumber;

    public RabbitMQPublisher()
    {
        var config = ConfigHelper.GetRabbitMQConfig();
        Console.WriteLine(config.ToString());
        _host = config.Host;
        _queueName = config.QueueName;
        _username = config.Username;
        _password = config.Password;
        _portNumber = config.Port;
        
    }

    public void PublishOrder(Order order)
    {
        var factory = new ConnectionFactory()
        {
            HostName = _host,
            UserName = _username,
            Password = _password,
            Port = _portNumber
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

        var message = JsonConvert.SerializeObject(order);
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);
        Console.WriteLine(" [x] Sent {0}", message);
    }
}
