using RabbitMQWebAPI.Models;

namespace RabbitMQWebAPI.Helpers;

public static class ConfigHelper
{
    public static RabbitMQConfig GetRabbitMQConfig()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("AppSettings/config.json")
            .Build();

        var rabbitConfig = new RabbitMQConfig();
        config.GetSection("RabbitMQ").Bind(rabbitConfig);
        return rabbitConfig;
    }
}