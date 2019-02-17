using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Extellect.Collections;

namespace Extellect.Collections
{
    /// <summary>
    /// Summary description for PriorityQueueTests
    /// </summary>
    
    public class PriorityQueueTests
    {

        [Fact]
        public void Queue()
        {
            PriorityQueue<string> queue = CreatePriorityQueue();

            Assert.True(
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
            Assert.Equal(0, queue.Count);
        }

        [Fact]
        public void Queue2()
        {
            PriorityQueue<string> queue = CreatePriorityQueue();

            Assert.True(
                new string[] { 
                    "High-a", "High-b", "High-c",
                    "Medium-a", "Medium-b", "Medium-c",
                    "Low-a", "Low-b", "Low-c",
                }.SequenceEqual(queue));

            // enumerating the queue in order doesn't remove anything from the 
            // queue (same behaviour as queue)
            Assert.Equal(9, queue.Count);
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
