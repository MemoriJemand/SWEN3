using ImageMagick;
using OCRWorker.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCRWorker.Services
{
    public class PdfToImageConverter : IPdfToImageConverter
    {
        public List<Bitmap> Convert(Stream pdfStream)
        {
            var images = new List<Bitmap>();
            using var ms = new MemoryStream();
            pdfStream.CopyTo(ms);
            using var collection = new MagickImageCollection(ms.ToArray());
            foreach(var page in collection)
            {
                using var img = page.Clone();
                img.Format = MagickFormat.Png;
                using var stream = new MemoryStream();
                img.Write(stream);
                images.Add(new Bitmap(stream));
            }
            return images;
        }
    }
}
