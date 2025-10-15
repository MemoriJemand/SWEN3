using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Common.Options
{
    public sealed class RabbitMqOptions
    {
        // Connection
        public string HostName { get; set; } = "rabbitmq";
        public int Port { get; set; } = 5672;
        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";
        public string VirtualHost { get; set; } = ConnectionFactory.DefaultVHost;

        // Exchange(s)
        public string ExchangeName { get; set; } = "DocumentManager";

        // Dead-lettering (optional but recommended)
        public string? DlxExchangeName { get; set; } = "document_queue.dlx";
        public string? DlxQueueName { get; set; } = "document_queue.dlq";

        // Queues we care about for this feature set
        public string DocumentQueue { get; set; } = "documents_new";
        public string InformationQueue { get; set; } = "documents_processed";
    }
}
