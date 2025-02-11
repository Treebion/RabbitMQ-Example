using MassTransit;
using RabbitMQ.Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Consumer.Consumers
{
    /// <summary>
    /// Consumer class for handling notification messages.
    /// </summary>
    public sealed class NotificationConsumer : IConsumer<NotificationMessage>
    {
        /// <summary>
        /// Consumes the notification message and processes it.
        /// </summary>
        /// <param name="context">The context containing the notification message.</param>
        /// <returns>A task that represents the asynchronous consume operation.</returns>
        public Task Consume(ConsumeContext<NotificationMessage> context)
        {
            var notification = context.Message;
            StringBuilder display = new();
            Console.WriteLine($"Received: {notification.Id}");
            Console.WriteLine($"Type: {notification.Type}");
            Console.WriteLine($"To: {notification.Recipient}");
            Console.WriteLine($"Message: {notification.Message}");
            Console.WriteLine();

            return Task.CompletedTask;
        }
    }
}
