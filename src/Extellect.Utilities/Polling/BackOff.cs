using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Extellect.Utilities.Polling
{
    /// <summary>
    /// Frequently when polling, I find the need to back off because a resource isn't ready.
    /// For example, if I have a process that polls for the TOP(N) unprocessed items in a 
    /// SQL Server database (essentially dealing with them in batches,) I would prefere to 
    /// avoid hammering the database when there are no available items. For that purpose, I
    /// would contruct a <see cref="BackOff"/> object with some initial wait time, and a max
    /// wait time. The first time that the process found 0 available items, it would call 
    /// <see cref="Wait"/> and would wait for the initial time. If, on the subsequent call, it
    /// still found 0 available items, it would call <see cref="Wait"/> again, but this time
    /// it would wait twice as long. The wait increases exponentially until it reaches the
    /// specified max value. As soon as the process finds 1 or more available items, it
    /// should call <see cref="Reset"/>. This resets the back off algorithm to start at
    /// the initial value the next time the process finds 0 available items.
    /// </summary>
    public class BackOff
    {
        private readonly Action<TimeSpan> wait;
        private readonly TimeSpan initialWait;
        private readonly TimeSpan maxWait;
        private TimeSpan currentWait;

        /// <summary>
        /// Constructs a new BackOff object.
        /// </summary>
        public BackOff(TimeSpan initialWait, TimeSpan maxWait)
            : this (initialWait, maxWait, currentWait => Thread.Sleep(currentWait))
        {   
        }

        /// <summary>
        /// Constructs a new BackOff object, with a customised implementation of the 
        /// wait function. This constructor is used for unit tests, but it might also
        /// be useful if you don't want to use Thread.Sleep, which is the default 
        /// implementation.
        /// </summary>
        public BackOff(TimeSpan initialWait, TimeSpan maxWait, Action<TimeSpan> wait)
        {
            this.initialWait = initialWait;
            this.maxWait = maxWait;
            this.wait = wait;
            Reset();
        }

        /// <summary>
        /// Resets this object to its initial state.
        /// </summary>
        public void Reset()
        {
            currentWait = initialWait;
        }

        /// <summary>
        /// Calls the wait function and sets up the next wait duration in the back off sequence.
        /// </summary>
        public void Wait()
        {
            wait(currentWait);
            if (currentWait < maxWait)
            {
                currentWait += currentWait;
                if (currentWait > maxWait)
                {
                    currentWait = maxWait;
                }
            }
        }
    }
}
