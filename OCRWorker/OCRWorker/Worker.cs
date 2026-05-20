using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OCRWorker.Interfaces;
using Nest;
using OCRWorker.Contracts;
using OCRWorker.Model;

namespace OCRWorker
{
    public class Worker : BackgroundService
    {
        private readonly IQueueClient _queueClient;
        private readonly IOcrEngine _ocrEngine;
        private readonly ILogger<Worker> _logger;
        private readonly IElasticClient _elasticClient;
        public Worker(IQueueClient queueClient, IOcrEngine ocrEngine, ILogger<Worker> logger, IElasticClient elasticClient)
        {
            _queueClient = queueClient;
            _ocrEngine = ocrEngine;
            _logger = logger;
            _elasticClient = elasticClient;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Initializing Queue Client");
            await _queueClient.InitializeAsync();
            _logger.LogInformation("OCR Worker started");
            while (!stoppingToken.IsCancellationRequested)
            {
                var msg = await _queueClient.ReceiveAsync(stoppingToken);
                if (msg == null)
                {
                    await Task.Delay(500, stoppingToken);
                    continue;
                }
                try
                {
                    using var pdfStream = await _queueClient.GetPdfStreamAsync(msg, stoppingToken);
                    var text = await _ocrEngine.ExtractTextFromPdfAsync(pdfStream, stoppingToken);
                    _logger.LogInformation($"OCR completed for {msg.DocumentId}");
                    await _queueClient.CompleteAsync(msg, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing OCR job");
                    await _queueClient.AbandonAsync(msg, stoppingToken);
                }
            }
        }
        public async Task ProcessMessageAsyc(JobMessage msg, CancellationToken cancellationToken)
        {
            await using var pdfStream = await _queueClient.GetPdfStreamAsync(msg, cancellationToken);
            var text = await _ocrEngine.ExtractTextFromPdfAsync(pdfStream, cancellationToken);
            var doc = new DocIndex
            {
                DocID = msg.DocumentId,
                Content = text,
                Timestamp = DateTime.UtcNow
            };
            await _elasticClient.IndexDocumentAsync(doc, cancellationToken);
        }
    }
}
