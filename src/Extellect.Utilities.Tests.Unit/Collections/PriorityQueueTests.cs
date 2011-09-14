using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Extellect.Utilities.Collections;

namespace Extellect.Utilities.Tests.Unit.Collections
{
    /// <summary>
    /// Summary description for PriorityQueueTests
    /// </summary>
    [TestClass]
    public class PriorityQueueTests
    {
        public PriorityQueueTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void Queue()
        {
            PriorityQueue<string> queue = CreatePriorityQueue();

            Assert.IsTrue(
                new string[] { 
                    "High-a", "High-b", "High-c",
                    "Medium-a", "Medium-b", "Medium-c",
                    "Low-a", "Low-b", "Low-c",
                }.SequenceEqual(new string[] {
                    queue.Dequeue(),
                    queue.Dequeue(),
                    queue.Dequeue(),
                    queue.Dequeue(),
                    queue.Dequeue(),
                    queue.Dequeue(),
                    queue.Dequeue(),
                    queue.Dequeue(),
                    queue.Dequeue(),
                }));

            // each call to dequeue removes an element
            Assert.AreEqual(0, queue.Count);
        }

        [TestMethod]
        public void Queue2()
        {
            PriorityQueue<string> queue = CreatePriorityQueue();

            Assert.IsTrue(
                new string[] { 
                    "High-a", "High-b", "High-c",
                    "Medium-a", "Medium-b", "Medium-c",
                    "Low-a", "Low-b", "Low-c",
                }.SequenceEqual(queue));

            // enumerating the queue in order doesn't remove anything from the 
            // queue (same behaviour as queue)
            Assert.AreEqual(9, queue.Count);
        }

        private static PriorityQueue<string> CreatePriorityQueue()
        {
            PriorityQueue<string> queue = new PriorityQueue<string>();

            queue.Enqueue("Low-a", 1);
            queue.Enqueue("High-a", 3);
            queue.Enqueue("Medium-a", 2);

            queue.Enqueue("Low-b", 1);
            queue.Enqueue("High-b", 3);
            queue.Enqueue("Medium-b", 2);

            queue.Enqueue("Low-c", 1);
            queue.Enqueue("High-c", 3);
            queue.Enqueue("Medium-c", 2);
            return queue;
        }
    }
}
