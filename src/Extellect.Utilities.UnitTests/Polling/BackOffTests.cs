using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Extellect.Utilities.Testing;

namespace Extellect.Utilities.Polling
{
    
    public class BackOffTests
    {
        [Fact]
        public void Wait_WithDefaultWaitAction_ActuallyWaits()
        {
            TimeSpan expected = TimeSpan.FromMilliseconds(500);
            BackOff backOff = new BackOff(expected, TimeSpan.MaxValue);

            DateTime start = DateTime.Now;
            backOff.Wait();
            DateTime finish = DateTime.Now;
            
            Assert.True(finish - start > expected);
        }

        [Fact]
        public void Wait_WithFakeDelay_Waits()
        {
            TimeSpan expectedWait = TimeSpan.Zero;
            var fakeBlock = new TestBlock();

            TimeSpan initialWait = TimeSpan.FromMilliseconds(250);
            TimeSpan maxWait = TimeSpan.FromMilliseconds(550);
            BackOff backOff = new BackOff(initialWait, maxWait, fakeBlock);

            var expectedWaits = new List<TimeSpan>();

            // wait once
            expectedWaits.Add(initialWait);
            backOff.Wait();

            // wait a second time
            expectedWaits.Add(initialWait + initialWait);
            backOff.Wait();

            // this wait would exceed the specified max wait, so only wait the max
            expectedWaits.Add(maxWait);
            backOff.Wait();

            // wait the max again
            expectedWaits.Add(maxWait);
            backOff.Wait();

            // reset to initial
            backOff.Reset();

            // repeat the above sequence
            expectedWaits.Add(initialWait);
            backOff.Wait();

            expectedWaits.Add(initialWait + initialWait);
            backOff.Wait();

            expectedWaits.Add(maxWait);
            backOff.Wait();

            expectedWaits.Add(maxWait);
            backOff.Wait();

            AssertionHelper.AreSequencesEqual(fakeBlock.ActualWaits, expectedWaits);
        }
    }
}
