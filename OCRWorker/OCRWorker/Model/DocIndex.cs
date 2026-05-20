using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCRWorker.Model
{
    public class DocIndex
    {
        public string DocID { get; set; } = default!;
        public string Content { get; set; } = default!;
        public DateTime Timestamp { get; set; }
    }
}
