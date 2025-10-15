using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messaging.Common.Options;
using RabbitMQ.Client;

namespace Messaging.Common.Topology
{
    public static class RabbitTopology
    {
        public static void EnsureAll(IChannel channel, RabbitMqOptions rabbitMqOptions)
        {
            channel.ExchangeDeclareAsync(
                exchange: rabbitMqOptions.ExchangeName,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false);

            // Declare the Dead Letter Exchange (DLX) if configured
            //    - Used for failed/rejected messages (safety net)
            if (!string.IsNullOrWhiteSpace(rabbitMqOptions.DlxExchangeName))
            {
                channel.ExchangeDeclareAsync(
                    exchange: rabbitMqOptions.DlxExchangeName!,
                    type: ExchangeType.Fanout,   // Fanout: send dead letters to all bound queues
                    durable: true,
                    autoDelete: false);

                // Declare Dead Letter Queue if provided
                if (!string.IsNullOrWhiteSpace(rabbitMqOptions.DlxQueueName))
                {
                    channel.QueueDeclareAsync(
                        queue: rabbitMqOptions.DlxQueueName!,
                        durable: true,      // survive broker restarts
                        exclusive: false,   // can be consumed by multiple consumers
                        autoDelete: false,  // not auto-deleted when last consumer disconnects
                        arguments: null);

                    // Bind DLQ to DLX (routingKey irrelevant for fanout exchange)
                    channel.QueueBindAsync(rabbitMqOptions.DlxQueueName, rabbitMqOptions.DlxExchangeName!, routingKey: "");
                }
            }

            // Common queue arguments (applied to business queues)
            //    - Add DLX binding if one exists, so rejected messages are routed safely
            var args = new Dictionary<string, object>();
            if (!string.IsNullOrWhiteSpace(rabbitMqOptions.DlxExchangeName))
                args["x-dead-letter-exchange"] = rabbitMqOptions.DlxExchangeName;


            channel.QueueDeclareAsync(
                queue: rabbitMqOptions.DocumentQueue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: args);


            channel.QueueDeclareAsync(
                queue: rabbitMqOptions.InformationQueue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: args); 


            channel.QueueBindAsync(
                queue: rabbitMqOptions.DocumentQueue,
                exchange: rabbitMqOptions.ExchangeName,
                routingKey: "document_add");

            channel.QueueBindAsync(
                queue: rabbitMqOptions.InformationQueue,
                exchange: rabbitMqOptions.ExchangeName,
                routingKey: "document_read");
        }
    }
}
