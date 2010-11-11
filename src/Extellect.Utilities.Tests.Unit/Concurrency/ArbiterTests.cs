using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extellect.Utilities.Concurrency
{
    [TestClass]
    public class ArbiterTests
    {
        [TestMethod]
        public void A()
        {
            WaitCallback f1 = delegate(object state)
            {
                Console.WriteLine("a");
                Arbiter.Yield();
                Console.WriteLine("b");
                Arbiter.Yield();
                Console.WriteLine("c");
                Arbiter.Yield();
                Console.WriteLine("d");
            };

            WaitCallback f2 = delegate(object state)
            {
                Console.WriteLine("A");
                Arbiter.Yield();
                Console.WriteLine("B");
                Arbiter.Yield();
                Console.WriteLine("C");
                Arbiter.Yield();
                Console.WriteLine("D");
            };

            Arbiter.Setup(1); // starts with f1

            f1 = Arbiter.RegisterCallback(f1);
            f2 = Arbiter.RegisterCallback(f2);

            ThreadPool.QueueUserWorkItem(f1, null);
            ThreadPool.QueueUserWorkItem(f2, null);

            // writes: a A b B c C d D 

            Arbiter.WaitUntilFinished();
        }
    }
}
