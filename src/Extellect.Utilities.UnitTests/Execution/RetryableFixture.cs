using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Extellect.Utilities.Execution
{
    
    public class RetryableFixture
    {
        [Fact]
        public void Retry_ThrowsTwiceSucceedsThirdAttempt()
        {
            var retried = new Dictionary<int, Exception>();
            var n = 0;
            string m = null;

            Retryable.Retry(() => { if (n++ < 2) { throw new Exception("No"); } else { m = "Yes"; } }, 3, (e, i) => { retried.Add(i, e); }, e => e.Message == "No");

            Assert.Equal(3, n);
            Assert.Equal(2, retried.Count);
            Assert.Equal("Yes", m);
        }
    }
}
