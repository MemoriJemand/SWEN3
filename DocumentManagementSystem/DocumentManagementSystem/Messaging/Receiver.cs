using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

namespace DocumentManagementSystem.Messaging
{
    public class Receiver
    {
        public IChannel Channel { get; set; }
        string message;

        public async Task<String> ReceiveInfo()
        {
            await Channel.QueueDeclareAsync("ocrResult", true, false, false, null);
            var consumer = new AsyncEventingBasicConsumer(Channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                message = Encoding.UTF8.GetString(body);
            };
            await Channel.BasicConsumeAsync("ocrResult", autoAck: true, consumer: consumer);

            return message;
        }
    }
}
