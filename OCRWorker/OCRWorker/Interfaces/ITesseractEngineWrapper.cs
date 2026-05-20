using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCRWorker.Interfaces
{
    public interface ITesseractEngineWrapper
    {
        string Process(Bitmap bitmap);
    }
}
