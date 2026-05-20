using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCRWorker.Interfaces
{
    public interface IPdfToImageConverter
    {
        List<Bitmap> Convert(Stream stream);
    }
}
