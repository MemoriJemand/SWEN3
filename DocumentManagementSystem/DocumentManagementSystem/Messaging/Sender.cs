using RabbitMQ.Client;
using System.Text;
using System.Threading.Channels;

namespace DocumentManagementSystem.Messaging
{
    public class Sender
    {
        public IChannel Channel { get; set; }
        
        public async void SendDocument(string document)
        {
            await Channel.QueueDeclareAsync("documents", true, false, false, null);
            var body = Encoding.UTF8.GetBytes(document);
            await Channel.BasicPublishAsync(exchange: string.Empty, routingKey: "documents", body: body);
        }
    }
}
