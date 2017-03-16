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
        private readonly IBlockable _blockable;
        private readonly TimeSpan _maxSleepDuration;

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public ManyBlock(IBlockable blockable, TimeSpan maxSleepDuration)
        {
            _blockable = blockable;
            _maxSleepDuration = maxSleepDuration;
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
                _blockable.Block(Min(slice, _maxSleepDuration));
            }
        }

        private static TimeSpan Min(TimeSpan one, TimeSpan two)
        {
            return one < two ? one : two;
        }
    }
}
