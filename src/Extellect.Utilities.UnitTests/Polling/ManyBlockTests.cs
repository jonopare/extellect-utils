using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Extellect.Utilities.Testing;

namespace Extellect.Utilities.Polling
{
    
    public class ManyBlockTests
    {
        [Fact]
        public void Block_DelayLessThanMaxDelay_BlocksOnce()
        {
            var fake = new TestBlock();
            var many = new ManyBlock(fake, TimeSpan.FromSeconds(1));

            var testClock = new TestClock();
            Clock.Current = testClock;
            fake.Clock = testClock;

            testClock.UtcNow = DateTime.UtcNow;
            many.Block(TimeSpan.FromSeconds(0.5));
            AssertionHelper.AreSequencesEqual(new[] { TimeSpan.FromSeconds(0.5) }, fake.ActualWaits);
        }

        [Fact]
        public void Block_DelayGreaterThanMaxDelay_BlocksManyTimes()
        {
            var fake = new TestBlock();
            var many = new ManyBlock(fake, TimeSpan.FromSeconds(1));

            var testClock = new TestClock();
            Clock.Current = testClock;
            fake.Clock = testClock;

            testClock.UtcNow = DateTime.UtcNow;
            many.Block(TimeSpan.FromSeconds(2.5));
            AssertionHelper.AreSequencesEqual(new[] { TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(0.5) }, fake.ActualWaits);
        }
    }
}
