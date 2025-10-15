using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

namespace OCRWorker.Messaging
{
    public class Receiver
    {
        public IChannel Channel { get; set; }
        string message;

        public async Task<String> ReceiveDocument()
        {
            await Channel.QueueDeclareAsync("documents", true, false, false, null);
            var consumer = new AsyncEventingBasicConsumer(Channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                message = Encoding.UTF8.GetString(body);
            };

            await Channel.BasicConsumeAsync("documents", autoAck: true, consumer: consumer);

            return message;
        }
    }
}
