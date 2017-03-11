using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extellect.Utilities.Polling
{
    internal class TestBlock : IBlockable
    {
        internal TestClock Clock { get; set; }

        public List<TimeSpan> ActualWaits { get; private set; }

        public TestBlock()
        {
            ActualWaits = new List<TimeSpan>();
        }

        public void Block(TimeSpan actualWait)
        {
            ActualWaits.Add(actualWait);
            if (Clock != null)
                Clock.UtcNow += actualWait; // tick!
        }
    }
}
