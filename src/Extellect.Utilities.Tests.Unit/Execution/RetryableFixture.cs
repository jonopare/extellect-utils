using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extellect.Utilities.Execution
{
    [TestClass]
    public class RetryableFixture
    {
        [TestMethod]
        public void Retry_ThrowsTwiceSucceedsThirdAttempt()
        {
            var retried = new Dictionary<int, Exception>();
            var n = 0;
            string m = null;

            Retryable.Retry(() => { if (n++ < 2) { throw new Exception("No"); } else { m = "Yes"; } }, 3, (e, i) => { retried.Add(i, e); }, e => e.Message == "No");

            Assert.AreEqual(3, n);
            Assert.AreEqual(2, retried.Count);
            Assert.AreEqual("Yes", m);
        }
    }
}
