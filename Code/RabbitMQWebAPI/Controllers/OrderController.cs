using Microsoft.AspNetCore.Mvc;
using RabbitMQWebAPI.Models;
using RabbitMQWebAPI.Services.Contracts;

namespace RabbitMQWebAPI.Controllers;

[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost("place-order")]
    public IActionResult PlaceOrder([FromBody] Order order)
    {
        _orderService.PlaceOrder(order);
        return Ok(new { message = "Order placed and sent to RabbitMQ", order });
    }
}