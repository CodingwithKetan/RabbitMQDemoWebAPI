namespace RabbitMQWebAPI.RabbitMQ;

public interface IRabbitMQConsumer
{
    void ConsumeOrder(CancellationToken cancellationToken);
}