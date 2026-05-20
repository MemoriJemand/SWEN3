using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCRWorker.Contracts
{
    public class JobMessage
    {
        public string DocumentId { get; set; } = string.Empty;
        public string Bucket { get; set; } = string.Empty;  
        public string Key { get; set; } = string.Empty;
        public ulong DeliveryTag { get; set; }
    }
}
