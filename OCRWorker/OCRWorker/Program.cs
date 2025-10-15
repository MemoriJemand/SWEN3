// See https://aka.ms/new-console-template for more information

using OCRWorker.Messaging;

Messenger _messenger = new();

while (true)
{
    var message = await _messenger.Receiver.ReceiveDocument();
    message = message.Trim();
    _messenger.Sender.SendDocument(message);
}