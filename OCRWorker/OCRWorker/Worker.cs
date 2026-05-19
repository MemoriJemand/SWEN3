using OCRWorker.Messaging;

namespace OCRWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private Messenger _messenger = new();

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
                while (true)
                {
                    var message = await _messenger.Receiver.ReceiveDocument();
                    message = message.Trim();
                    _messenger.Sender.SendDocument(message);
                }
            }
        }
    }
}
