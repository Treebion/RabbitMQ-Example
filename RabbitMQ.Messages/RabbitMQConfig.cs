using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Shared
{
    /// <summary>
    /// Represents the configuration settings for RabbitMQ.
    /// </summary>
    public sealed class RabbitMQConfig
    {
        /// <summary>
        /// Gets or sets the RabbitMQ host address.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the RabbitMQ port number.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the RabbitMQ username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the RabbitMQ password.
        /// </summary>
        public string Password { get; set; }
    }
}
