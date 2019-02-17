using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Extellect.Polling
{
    /// <summary>
    /// Class that allows for a blocking type delay to be inserted into the execution flow.
    /// </summary>
    public class ThreadSleepBlock : IBlockable
    {
        /// <summary>
        /// A single call to Thread.Sleep
        /// </summary>
        public void Block(TimeSpan duration)
        {
            Thread.Sleep(duration);
        }
    }
}
