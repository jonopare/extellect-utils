using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Extellect.Utilities.Collections
{
    /// <summary>
    /// Implements a thread safe queue that blocks if attempting to read when 
    /// empty or write when full.
    /// </summary>
    public class BlockingQueue<T>
    {
        private readonly int capacity;
        private readonly object mutex;
        private readonly Queue<T> queue;

        /// <summary>
        /// Constructs a new queue with a capacity of int.MaxValue.
        /// </summary>
        public BlockingQueue()
            : this(int.MaxValue)
        {
        }

        /// <summary>
        /// Constructs a new queue with a specific capacity.
        /// </summary>
        public BlockingQueue(int capacity)
        {
            this.capacity = capacity;
            mutex = new object();
            queue = new Queue<T>();
        }

        /// <summary>
        /// Writes an item onto the queue. If the queue has reached capacity, this
        /// operation will block.
        /// </summary>
        public void Enqueue(T item)
        {
            lock (mutex)
            {
                while (queue.Count == capacity)
                {
                    Monitor.Wait(mutex);
                }
                queue.Enqueue(item);
                if (queue.Count == 1)
                {
                    Monitor.PulseAll(mutex);
                }
            }
        }

        /// <summary>
        /// Reads an item off the queue. If there are no items on the queue, this
        /// operation will block.
        /// </summary>
        public T Dequeue()
        {
            lock (mutex)
            {
                while (queue.Count == 0)
                {
                    Monitor.Wait(mutex);
                }
                T item = queue.Dequeue();
                if (queue.Count == capacity - 1)
                {
                    Monitor.PulseAll(mutex);
                }
                return item;
            }
        }
    }
}
