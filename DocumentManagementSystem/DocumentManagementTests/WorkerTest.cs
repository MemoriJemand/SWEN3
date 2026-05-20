using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Xunit;
using OCRWorker;
using OCRWorker.Interfaces;
using Microsoft.Extensions.Logging;
using OCRWorker.Contracts;

namespace DocumentManagementTests
{
    public class WorkerTest
    {
        [Fact]
        public async Task WorkerProcessOcr()
        {
            var queue = new Mock<IQueueClient>();
            var ocr = new Mock<IOcrEngine>();
            var logger = new Mock<ILogger<Worker>>();
            var msg = new JobMessage
            {
                DocumentId = "123",
                Bucket = "docs",
                Key = "test.pdf",
                DeliveryTag = 1
            };

            queue.Setup(q => q.InitializeAsync()).Returns(Task.CompletedTask);
            queue.SetupSequence(q => q.ReceiveAsync(It.IsAny<CancellationToken>())).ReturnsAsync(msg).ReturnsAsync((JobMessage?)null);
            queue.Setup(q => q.GetPdfStreamAsync(msg, It.IsAny<CancellationToken>())).ReturnsAsync(new  MemoryStream(new byte[] {1,2,3}));
            ocr.Setup(o => o.ExtractTextFromPdfAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>())).ReturnsAsync("test text");
            var worker = new Worker(queue.Object, ocr.Object, logger.Object);

            var testToken = new CancellationTokenSource(200);
            await worker.StartAsync(testToken.Token);
            queue.Verify(q => q.CompleteAsync(msg, It.IsAny<CancellationToken>()), Times.Once);

        }
    }
}
