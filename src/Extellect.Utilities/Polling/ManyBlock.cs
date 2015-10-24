using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Extellect.Utilities.Polling
{
    /// <summary>
    /// Class that allows for a composite blocking type delay to be inserted into the execution flow.
    /// </summary>
    public class ManyBlock : IBlockable
    {
        private readonly IBlockable blockable;
        private readonly TimeSpan maxSleepDuration;

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public ManyBlock(IBlockable blockable, TimeSpan maxSleepDuration)
        {
            this.blockable = blockable;
            this.maxSleepDuration = maxSleepDuration;
        }

        /// <summary>
        /// Multiple calls to Thread.Sleep (each no longer than maxSleepDuration) until the complete 
        /// duration has expired.
        /// </summary>
        public void Block(TimeSpan duration)
        {
            var awaken = Clock.UtcNow.Add(duration);
            TimeSpan slice;
            while ((slice = awaken - Clock.UtcNow) > TimeSpan.Zero)
            {
                blockable.Block(Min(slice, maxSleepDuration));
            }
        }

        private static TimeSpan Min(TimeSpan one, TimeSpan two)
        {
            return one < two ? one : two;
        }
    }
}
