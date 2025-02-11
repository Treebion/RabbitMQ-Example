using MassTransit;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Shared;
using RabbitMQ.Shared.Messages;

namespace RabbitMQ.Producer
{
    /// <summary>
    /// The main entry point for the RabbitMQ producer application.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main method to start the RabbitMQ producer.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        public static async Task Main(string[] args)
        {
            var rabbitMQConfig = Configure();

            var busControl = Bus.Factory.CreateUsingRabbitMq(c =>
            {
                c.Host(new Uri($"rabbitmq://{rabbitMQConfig.Host}:{rabbitMQConfig.Port}"),
                h =>
                {
                    h.Username(rabbitMQConfig.Username);
                    h.Password(rabbitMQConfig.Password);
                });
            });

            await busControl.StartAsync();
            try
            {
                while (await SendMessage(busControl) != "quit")
                {
                    Console.WriteLine();
                };
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
        /// Sends a notification message to the RabbitMQ queue.
        /// </summary>
        /// <param name="busControl">The bus control to send the message.</param>
        /// <returns>A task that represents the asynchronous send operation.</returns>
        private static async Task<string> SendMessage(IBusControl busControl)
        {
            Console.WriteLine("Enter recipient:");
            string recipient = Console.ReadLine()!;

            Console.WriteLine("Enter message:");
            string message = Console.ReadLine()!;

            MessageType? type = null!;

            while (!type.HasValue)
            {
                Console.WriteLine("Enter type:");
                Console.WriteLine("A:\tEmail");
                Console.WriteLine("B:\tSMS");
                Console.WriteLine("C:\tPush");
                string typeAsString = Console.ReadLine()!;

                switch (typeAsString.ToLower())
                {
                    case "a": type = MessageType.Email; break;
                    case "b": type = MessageType.SMS; break;
                    case "c": type = MessageType.Push; break;
                    default: break;
                }
            }

            var notification = new NotificationMessage
            {
                Id = Guid.NewGuid(),
                Recipient = recipient,
                Message = message,
                Type = type.Value
            };

            var endpoint = await busControl.GetSendEndpoint(new Uri($"queue:NotificationQueue"));
            await endpoint.Send(notification);

            Console.WriteLine($"Sent message: {notification.Id}");

            return Console.ReadLine()!;
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
}
