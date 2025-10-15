using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Messaging.Common.Publishing
{
    public class Publisher
    {
        private readonly IChannel _channel;
        public Publisher(IChannel channel)
        {
            _channel = channel;
        }
        public void Publish<T>(string exchange, string routingKey, string message, string? correlationId = null)
        {
            var body = Encoding.UTF8.GetBytes(message);
            var props = new BasicProperties();
            props.Persistent = true;
            _channel.BasicPublishAsync(exchange, routingKey, true, props, body);
        }
    }
}
