using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extellect.Utilities.Polling
{
    [TestClass]
    public class BackOffTests
    {
        [TestMethod]
        public void Wait_WithDefaultWaitAction_ActuallyWaits()
        {
            TimeSpan expected = TimeSpan.FromMilliseconds(500);
            BackOff backOff = new BackOff(expected, TimeSpan.MaxValue);

            DateTime start = DateTime.Now;
            backOff.Wait();
            DateTime finish = DateTime.Now;
            
            Assert.IsTrue(finish - start > expected);
        }

        [TestMethod]
        public void Wait_WithMockWaitAction_Waits()
        {
            TimeSpan expectedWait = TimeSpan.Zero;
            TimeSpan initialWait = TimeSpan.FromMilliseconds(250);
            TimeSpan maxWait = TimeSpan.FromMilliseconds(550);
            BackOff backOff = new BackOff(initialWait, maxWait, currentWait => Assert.AreEqual(expectedWait, currentWait));

            // wait once
            expectedWait = initialWait;
            backOff.Wait();

            // wait a second time
            expectedWait = initialWait + initialWait;
            backOff.Wait();

            // this wait would exceed the specified max wait, so only wait the max
            expectedWait = maxWait;
            backOff.Wait();

            // wait the max again
            expectedWait = maxWait;
            backOff.Wait();

            // reset to initial
            backOff.Reset();

            // repeat the above sequence
            expectedWait = initialWait;
            backOff.Wait();

            expectedWait = initialWait + initialWait;
            backOff.Wait();

            expectedWait = maxWait;
            backOff.Wait();

            expectedWait = maxWait;
            backOff.Wait();
        }
    }
}
