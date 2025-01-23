using RabbitMQWebAPI.RabbitMQ;
using RabbitMQWebAPI.RabbitMQ.Impl;
using RabbitMQWebAPI.Services;
using RabbitMQWebAPI.Services.Contracts;
using RabbitMQWebAPI.Services.Impl;

 public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureServices(services =>
                    {
                        // Register services and controllers
                        services.AddControllers();

                        // Add Swagger generation
                        services.AddSwaggerGen();

                        services.AddSingleton<IRabbitMQPublisher, RabbitMQPublisher>();
                        services.AddSingleton<IRabbitMQConsumer, RabbitMQConsumer>();
                        services.AddSingleton<IOrderService, OrderService>();
                        services.AddHostedService<RabbitMQConsumerHostedService>();
                    });

                    webBuilder.Configure(app =>
                    {
                        app.UseRouting();

                        app.UseSwagger();
                        app.UseSwaggerUI();

                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapControllers();  // Map controllers
                        });
                    });
                });
    }

