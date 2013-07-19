using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace Extellect.Utilities.Collections
{
    /// <summary>
    /// This class encapsulates a pool of objects. Items can be acquired from the pool
    /// but once acquired they are exclusively "owned" until they are released.
    /// It is intended to provide an efficient mechanism for sharing reusable 
    /// objects whose creation is costly.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Pool<T> : IDisposable
    {
        private readonly Func<T> activate;
        private readonly Action<T> passivate;
        private readonly int capacity;
        private readonly TimeSpan keepAlive;

        private readonly List<T> items;
        private readonly List<bool> leases;

        private int available;

        public Pool(Func<T> activate, Action<T> passivate, int capacity, TimeSpan keepAlive)
        {
            this.activate = activate;
            this.passivate = passivate;
            this.capacity = capacity;
            this.keepAlive = keepAlive;

            items = new List<T>(capacity);
            leases = new List<bool>(capacity);

            available = capacity;
        }

        public int Capacity
        {
            get { return capacity; }
        }

        /// <summary>
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool TryAcquire(TimeSpan timeout, out T item)
        {
            DateTime end = DateTime.UtcNow + timeout;
            if (Monitor.TryEnter(leases, timeout))
            {
                try
                {
                    if (available == 0)
                    {
                        do
                        {
                            timeout = end - DateTime.UtcNow;
                            if (timeout < TimeSpan.Zero || !Monitor.Wait(leases, timeout))
                            {
                                item = default(T);
                                return false;
                            }
                        } while (available == 0);
                    }

                    for (int i = 0; i < leases.Count; i++)
                    {
                        if (!leases[i])
                        {
                            leases[i] = true;
                            item = items[i];
                            available--;
                            return true;
                        }
                    }

                    item = activate();
                    items.Add(item);
                    leases.Add(true);
                    available--;
                    return true;
                }
                finally
                {
                    Monitor.Exit(leases);
                }
            }
            else
            {
                item = default(T);
                return false;
            }
        }

        public void Release(T item)
        {
            lock (leases)
            {
                var released = false;
                for (int i = 0; i < leases.Count; i++)
                {
                    if (object.Equals(items[i], item))
                    {
                        if (leases[i])
                        {
                            leases[i] = false;
                            released = true;
                            available++;
                            Monitor.PulseAll(leases);
                            break;
                        }
                        else
                        {
                            throw new InvalidOperationException();
                        }
                    }
                }
                if (!released)
                {
                    throw new InvalidOperationException(@"Item not found");
                }
            }
        }

        public void Dispose()
        {
            lock (leases)
            {
                for (int i = leases.Count - 1; i >= 0; i--)
                {
                    passivate(items[i]);
                    items.RemoveAt(i);

                    if (leases[i])
                    {
                        Debug.Print(@"Encountered an outstanding lease while disposing a pool.");
                    }
                    leases.RemoveAt(i);
                }
            }
        }
    }
}
