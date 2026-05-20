using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using OCRWorker.Contracts;
using OCRWorker.Interfaces;
using Xunit;

namespace DocumentManagementTests
{
    public class QueueClientTest
    {
        [Fact]
        public async Task ReceiveAsyncReturnNull()
        {
            var queue = new Mock<IQueueClient>();
            queue.Setup(q => q.ReceiveAsync(It.IsAny<CancellationToken>())).ReturnsAsync((JobMessage?)null);

            var result = await queue.Object.ReceiveAsync(CancellationToken.None);

            Assert.Null(result);
        }
    }
}
