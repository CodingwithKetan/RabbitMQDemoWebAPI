using RabbitMQWebAPI.Models;

namespace RabbitMQWebAPI.Services.Contracts;

public interface IOrderService
{
    void PlaceOrder(Order order);
}