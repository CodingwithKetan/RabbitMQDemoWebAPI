using RabbitMQWebAPI.RabbitMQ;

namespace RabbitMQWebAPI.Services;


public class RabbitMQConsumerHostedService : BackgroundService
{
    private readonly IRabbitMQConsumer _rabbitMQConsumer;
    private readonly int _numberOfConsumers;
    private readonly List<Task> _consumerTasks;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public RabbitMQConsumerHostedService(IRabbitMQConsumer rabbitMQConsumer, int numberOfConsumers = 5)
    {
        _rabbitMQConsumer = rabbitMQConsumer;
        _numberOfConsumers = numberOfConsumers;
        _consumerTasks = new List<Task>();
        _cancellationTokenSource = new CancellationTokenSource();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() => StartConsumers(stoppingToken));
    }

    private void StartConsumers(CancellationToken stoppingToken)
    {
        for (int i = 0; i < _numberOfConsumers; i++)
        {
            var task = Task.Run(() =>
            {
                _rabbitMQConsumer.ConsumeOrder(stoppingToken);
            }, stoppingToken);

            _consumerTasks.Add(task);
        }

        Console.WriteLine($"{_numberOfConsumers} RabbitMQ consumers started.");
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Stopping RabbitMQ consumers...");
        _cancellationTokenSource.Cancel();

        await Task.WhenAll(_consumerTasks);
        Console.WriteLine("RabbitMQ consumers stopped.");
    }
}