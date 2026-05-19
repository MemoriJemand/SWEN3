using Messaging.Common.Options;
using Messaging.Common.Topology;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace DocumentManagementSystem.Messaging
{
    public sealed class NewDocumentReceiver : INewDocumentReceiver
    {
        private readonly IChannel _channel;      
        private readonly RabbitMqOptions _opt;  
        public NewDocumentReceiver(IChannel channel, IOptions<RabbitMqOptions> opt)
        {
            _channel = channel;
            _opt = opt.Value;
            RabbitTopology.EnsureAll(_channel, _opt);
        }
        

        public async Task<String> ReceiveDocumentText()
        {
            string message = "";
            await _channel.QueueDeclareAsync("documents_processed", true, false, false, null);
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                message = Encoding.UTF8.GetString(body);
            };

            await _channel.BasicConsumeAsync("documents_processed", autoAck: true, consumer: consumer);

            return message;
        }
    }
}
