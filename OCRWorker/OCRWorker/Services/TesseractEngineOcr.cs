using OCRWorker.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace OCRWorker.Services
{
    public class TesseractEngineOcr : IOcrEngine
    {
        private readonly IPdfToImageConverter _converter;
        private readonly ITesseractEngineWrapper _engine;
        public TesseractEngineOcr(IPdfToImageConverter converter, ITesseractEngineWrapper engine)
        {
            _converter = converter;
            _engine = engine;
        }
        public async Task<string> ExtractTextFromPdfAsync(Stream pdfStream, CancellationToken token)
        {
            var images = _converter.Convert(pdfStream);
            var sb = new StringBuilder();
            foreach (var image in images)
            {
                sb.AppendLine(_engine.Process(image));
            }
            return sb.ToString();
        }
    }
}
