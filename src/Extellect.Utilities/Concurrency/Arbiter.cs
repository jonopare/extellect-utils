using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Extellect.Utilities.Concurrency
{
    /// <summary>
    /// A very basic means of slicing up concurrent code so that two threads
    /// can run in parallel in a controlled (defined) order.
    /// </summary>
    public static class Arbiter
    {
        [ThreadStatic]
        private static int id;
        
        private static int firstToRun;
        private static int count;
        private static int finished;
        private static ManualResetEvent main;

        [ThreadStaticAttribute]
        private static ManualResetEvent sync;

        private static List<ManualResetEvent> all;

        private readonly static string StartGate = "__START";
        private readonly static string FinishGate = "__FINISH";

        /// <summary>
        /// Registers a callback with the arbiter. Wraps the specified action with 
        /// control calls to synchronise with the arbiter.
        /// </summary>
        /// <param name="action">The action to be performed.</param>
        /// <returns>The actual callback that should be run</returns>
        public static WaitCallback RegisterCallback(WaitCallback action)
        {
            int id = Interlocked.Increment(ref count);
            WaitCallback result = delegate(object state)
            {
                Arbiter.sync = new ManualResetEvent(false);
                all.Add(Arbiter.sync);
                Arbiter.id = id;
                Gate(StartGate);
                try
                {   
                    action(state);
                }
                finally
                {
                    Gate(FinishGate);
                    int x = Interlocked.Increment(ref finished);
                    if (x >= count)
                    {
                        foreach (ManualResetEvent e in all)
                        {
                            e.Set();
                        }
                        main.Set();
                    }
                }
            };
            return result;
        }

        /// <summary>
        /// Yields from the current thread to the other thread.
        /// </summary>
        public static void Yield()
        {
            Yield(true);
        }

        private static void Gate(string name)
        {
            if (name == StartGate)
            {
                if (Arbiter.id != firstToRun)
                {
                    Yield(true);
                }
                else
                {
                    sync.WaitOne();
                }
            }
            else if (name == FinishGate)
            {
                Yield(false);
            }
            else
            {
                Yield(true);
            }
        }

        private static void Yield(bool wait)
        {
            foreach (ManualResetEvent other in all.Where(s => s != sync))
            {
                other.Set();
            }
            if (wait)
            {
                sync.Reset();
                sync.WaitOne();
            }
        }

        /// <summary>
        /// Set up the arbiter to run.
        /// </summary>
        /// <param name="firstToRun">Either 1 or 2; whichever of the two functions you want to start first.</param>
        public static void Setup(int firstToRun)
        {
            count = 0;
            main = new ManualResetEvent(false);
            all = new List<ManualResetEvent>();
            Arbiter.firstToRun = firstToRun;
        }

        /// <summary>
        /// Waits for the threads to finish their work.
        /// </summary>
        public static void WaitUntilFinished()
        {
            main.WaitOne();
            sync = null;
            all = null;
        }
    }
}
