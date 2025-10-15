using Messaging.Common.Options;
using Messaging.Common.Topology;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;

namespace DocumentManagementSystem.Messaging
{
    public sealed class NewDocumentPublisher : INewDocumentPublisher
    {
        private readonly IChannel _channel;      
        private readonly RabbitMqOptions _opt;  
        public NewDocumentPublisher(IChannel channel, IOptions<RabbitMqOptions> opt)
        {
            _channel = channel;
            _opt = opt.Value;
            RabbitTopology.EnsureAll(_channel, _opt);
        }
        public Task PublishNewDocumentAsync(string documentStr)
        {
            var body = Encoding.UTF8.GetBytes(documentStr);
            var props = new BasicProperties();
            props.Persistent = true;
            _channel.BasicPublishAsync(_opt.ExchangeName, "document_add", true, props, body);
            return Task.CompletedTask;
        }
    }
}
