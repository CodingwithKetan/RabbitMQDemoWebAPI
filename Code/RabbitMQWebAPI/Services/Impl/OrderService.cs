using RabbitMQWebAPI.Models;
using RabbitMQWebAPI.RabbitMQ;
using RabbitMQWebAPI.Services.Contracts;

namespace RabbitMQWebAPI.Services.Impl;

public class OrderService : IOrderService
{
    private readonly IRabbitMQPublisher _rabbitMQPublisher;

    public OrderService(IRabbitMQPublisher rabbitMQPublisher)
    {
        _rabbitMQPublisher = rabbitMQPublisher;
    }

   
    public void PlaceOrder(Order order)
    {
       _rabbitMQPublisher.PublishOrder(order);
    }
}