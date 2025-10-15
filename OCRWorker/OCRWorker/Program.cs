// See https://aka.ms/new-console-template for more information

using OCRWorker.Messaging;

Messenger _messenger = new();

while (true)
{
    await _messenger.Receiver.ReceiveDocument();
    //do something
    _messenger.Sender.SendDocument("documentSent");
}