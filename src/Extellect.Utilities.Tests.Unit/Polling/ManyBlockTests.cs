using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extellect.Utilities.Polling
{
    [TestClass]
    public class ManyBlockTests
    {
        [TestMethod]
        public void Block_DelayLessThanMaxDelay_BlocksOnce()
        {
            var fake = new TestBlock();
            var many = new ManyBlock(fake, TimeSpan.FromSeconds(1));

            var testClock = new TestClock();
            Clock.Current = testClock;
            fake.Clock = testClock;

            testClock.UtcNow = DateTime.UtcNow;
            many.Block(TimeSpan.FromSeconds(0.5));
            Assertion.AreSequenceEqual(fake.ActualWaits, new[] { TimeSpan.FromSeconds(0.5) }).Assert();
        }

        [TestMethod]
        public void Block_DelayGreaterThanMaxDelay_BlocksManyTimes()
        {
            var fake = new TestBlock();
            var many = new ManyBlock(fake, TimeSpan.FromSeconds(1));

            var testClock = new TestClock();
            Clock.Current = testClock;
            fake.Clock = testClock;

            testClock.UtcNow = DateTime.UtcNow;
            many.Block(TimeSpan.FromSeconds(2.5));
            Assertion.AreSequenceEqual(fake.ActualWaits, new[] { TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(0.5) }).Assert();
        }
    }
}
