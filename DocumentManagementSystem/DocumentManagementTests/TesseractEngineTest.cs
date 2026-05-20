using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using OCRWorker.Interfaces;
using OCRWorker.Services;
using Xunit;
using FluentAssertions;

namespace DocumentManagementTests
{
    public class TesseractEngineTest
    {
        [Fact]
        public async Task ExtractTextFromPdf()
        {
            var converter = new Mock<IPdfToImageConverter>();
            var testBitmap = new Bitmap(100, 100);

            converter.Setup(c => c.Convert(It.IsAny<Stream>()))
                     .Returns(new List<Bitmap> { testBitmap });

            var tesseract = new Mock<ITesseractEngineWrapper>();
            tesseract.Setup(t => t.Process(It.IsAny<Bitmap>())).Returns("TestText");
            var engine = new TesseractEngineOcr(converter.Object, tesseract.Object);

            // Act
            var result = await engine.ExtractTextFromPdfAsync(new MemoryStream(), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
