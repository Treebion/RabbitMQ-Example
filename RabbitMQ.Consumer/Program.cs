using MassTransit;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Consumer.Consumers;
using RabbitMQ.Shared;

namespace RabbitMQ.Consumer;

/// <summary>
/// The main entry point for the RabbitMQ consumer application.
/// </summary>
public class Program
{
    /// <summary>
    /// The main method to start the RabbitMQ consumer.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    public static async Task Main(string[] args)
    {
        RabbitMQConfig rabbitMQConfig = Configure();

        var busControl = Bus.Factory.CreateUsingRabbitMq(c =>
        {
            c.Host(new Uri($"rabbitmq://{rabbitMQConfig.Host}:{rabbitMQConfig.Port}"),
                h =>
                {
                    h.Username(rabbitMQConfig.Username);
                    h.Password(rabbitMQConfig.Password);
                });
            c.ReceiveEndpoint("NotificationQueue", e =>
            {
                e.Consumer<NotificationConsumer>();
            });
        });

        await busControl.StartAsync();
        try
        {
            Console.WriteLine("Consumer started...");
            Console.WriteLine("Press Enter to stop...");
            Console.ReadLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            await busControl.StopAsync();
        }
    }

    /// <summary>
    /// Configures the RabbitMQ settings from the appsettings.json file.
    /// </summary>
    /// <returns>The RabbitMQ configuration settings.</returns>
    private static RabbitMQConfig Configure()
    {
        var config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

        var rabbitMQConfig = config.GetSection("RabbitMQ").Get<RabbitMQConfig>()!;
        return rabbitMQConfig;
    }
}
