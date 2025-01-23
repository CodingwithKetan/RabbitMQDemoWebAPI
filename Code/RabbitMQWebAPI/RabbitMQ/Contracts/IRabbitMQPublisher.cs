using RabbitMQWebAPI.Models;

namespace RabbitMQWebAPI.RabbitMQ;

public interface IRabbitMQPublisher
{
    void PublishOrder(Order order);
}