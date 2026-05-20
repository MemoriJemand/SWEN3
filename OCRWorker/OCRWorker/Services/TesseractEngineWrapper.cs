using OCRWorker.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace OCRWorker.Services
{
    public class TesseractEngineWrapper : ITesseractEngineWrapper
    {
        public string Process(Bitmap bitmap)
        {
            using var ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Position = 0;

            using var pix = Pix.LoadFromMemory(ms.ToArray());
            using var engine = new TesseractEngine("./tessdata", "eng");
            using var page = engine.Process(pix);

            return page.GetText();
        }
    }
}
