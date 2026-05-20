using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OCRWorker.Contracts;

namespace OCRWorker.Interfaces
{
    public interface IQueueClient
    {
        Task InitializeAsync();
        Task<JobMessage?> ReceiveAsync(CancellationToken token);
        Task CompleteAsync (JobMessage message, CancellationToken token);
        Task AbandonAsync(JobMessage message, CancellationToken token);
        Task<Stream> GetPdfStreamAsync(JobMessage message, CancellationToken token);
    }
}
