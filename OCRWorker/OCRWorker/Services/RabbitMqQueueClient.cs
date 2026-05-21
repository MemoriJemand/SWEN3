using OCRWorker.Contracts;
using OCRWorker.Interfaces;
using RabbitMQ.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OCRWorker.Services
{
    public class RabbitMqQueueClient : IQueueClient
    {
        private IConnection? _connection;
        private IChannel? _channel;
        public async Task InitializeAsync()
        {
            var factory = new ConnectionFactory
            {
                HostName = "rabbitmq",
                UserName = "ocr",
                Password = "password"
            };
            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            await _channel.QueueDeclareAsync(
                queue: "documents_new",
                durable: true,
                exclusive: false,
                autoDelete: false
            );
        }
        public Task AbandonAsync(JobMessage message, CancellationToken token)
        {
            return _channel!.BasicNackAsync(message.DeliveryTag, multiple: false, requeue: true).AsTask();
        }

        public Task CompleteAsync(JobMessage message, CancellationToken token)
        {
            return _channel!.BasicAckAsync(message.DeliveryTag, multiple: false).AsTask();
        }

        public Task<Stream> GetPdfStreamAsync(JobMessage message, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public async Task<JobMessage?> ReceiveAsync(CancellationToken token)
        {
            var result = await _channel!.BasicGetAsync("documents_new", autoAck: false);
            if (result == null)
            {
                return null;
            }
            var json = Encoding.UTF8.GetString(result.Body.ToArray());
            var msg = JsonSerializer.Deserialize<JobMessage>(json);
            msg!.DeliveryTag = result.DeliveryTag;
            return msg;
        }
    }
}
