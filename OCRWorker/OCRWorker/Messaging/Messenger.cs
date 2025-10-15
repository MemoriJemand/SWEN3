using RabbitMQ.Client;
using System.Threading.Channels;

namespace OCRWorker.Messaging
{
    public class Messenger
    {
        private ConnectionFactory Factory = new();
        private IConnection connection;
        public Sender Sender = new();
        public Receiver Receiver = new();
        
        public async void ConnectMQ()
        {
            Factory.ClientProvidedName = "app:DMM component:OCR";
            connection = await Factory.CreateConnectionAsync();
            ChannelMQ();
        }

        public async void ChannelMQ()
        {
            IChannel channel = await connection.CreateChannelAsync();
            Sender.Channel = channel;
            Receiver.Channel = channel;
        }

        public async void CloseMQ()
        {
            await Sender.Channel.CloseAsync();
            await Receiver.Channel.CloseAsync();
            await connection.CloseAsync();
            await Sender.Channel.DisposeAsync();
            await Receiver.Channel.DisposeAsync();
            await connection.DisposeAsync();
        }
    }
}
