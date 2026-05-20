using Microsoft.Extensions.Logging;
using Moq;
using Nest;
using OCRWorker;
using OCRWorker.Contracts;
using OCRWorker.Interfaces;
using OCRWorker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DocumentManagementTests
{
    public class ElasticSearchTest
    {
        [Fact]
        public async Task ElasticSearchinOcr()
        {
            var queue = new Mock<IQueueClient>();
            var ocr = new Mock<IOcrEngine>();
            var elastic = new Mock<IElasticClient>();
            var logger = new Mock<ILogger<Worker>>();
            var msg = new JobMessage
            {
                DocumentId = "doc-123",
                Bucket = "docs",
                Key = "file.pdf"
            };
            var pdfStream = new MemoryStream(new byte[] { 1, 2, 3 });
            queue.Setup(q => q.GetPdfStreamAsync(msg, It.IsAny<CancellationToken>())).ReturnsAsync(pdfStream);
            ocr.Setup(o => o.ExtractTextFromPdfAsync(pdfStream, It.IsAny<CancellationToken>())).ReturnsAsync("OCR Working");
            elastic.Setup(e => e.IndexDocumentAsync(
                It.IsAny<DocIndex>(),
                It.IsAny<CancellationToken>()
                )
            ).ReturnsAsync(new IndexResponse());
            var worker = new Worker(queue.Object, ocr.Object, logger.Object, elastic.Object );

            await worker.ProcessMessageAsyc(msg, CancellationToken.None);
            elastic.Verify(e => e.IndexDocumentAsync(
                    It.Is<DocIndex>(d =>
                        d.DocID == "doc-123" &&
                        d.Content == "OCR Working"
                    ),
                    It.IsAny<CancellationToken>()
                ),
                Times.Once
            );
        }
    }
}
