using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCRWorker.Interfaces
{
    public interface IOcrEngine
    {
        Task<string> ExtractTextFromPdfAsync(Stream pdfStream, CancellationToken token);
    }
}
