using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Messaging.Common.Consuming
{
    public abstract class BaseConsumer<T>
    {
        private readonly IChannel _channel;
        private readonly string _queue;

        protected BaseConsumer(IChannel channel, string queue)
        {
            _channel = channel;
            _queue = queue;
        }

        public void Consume()
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);


            consumer.ReceivedAsync += async (model, ea) =>
            {

                try
                {
                    var body = Encoding.UTF8.GetString(ea.Body.ToArray());

                    await HandleMessage(body!, ea.BasicProperties.CorrelationId);

                    await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);

                }
                catch (Exception ex)
                {

                   await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false);

                }
            };
            _channel.BasicConsumeAsync(queue: _queue, autoAck: false, consumer: consumer);
        }
        protected abstract Task HandleMessage(string message, string correlationId);
    }
}