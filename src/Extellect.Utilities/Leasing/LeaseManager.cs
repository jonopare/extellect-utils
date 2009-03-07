using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using log4net;

namespace Extellect.Utilities.Leasing
{
    public class LeaseManager<TKey, TValue> : IDisposable
    {
        private readonly ILog log = LogManager.GetLogger(typeof(LeaseManager<TKey, TValue>));
        private readonly TimeSpan renewal;
        private readonly TimeSpan cleanUpInterval;
        private readonly Dictionary<TKey, Leasable<TValue>> items;
        private System.Threading.Timer timer;

        public LeaseManager(TimeSpan renewal, TimeSpan cleanUpInterval)
        {
            items = new Dictionary<TKey, Leasable<TValue>>();
            this.renewal = renewal;
            this.cleanUpInterval = cleanUpInterval;
        }

        public void Add(TKey key, TValue value)
        {
            Add(key, value, null);
        }

        public void Add(TKey key, TValue value, ExpiryAction<TValue> expiry)
        {
            lock (items)
            {
                log.Debug("Adding new leasable item...");
                items.Add(key, new Leasable<TValue>(value, new Lease(renewal), expiry));
                if (timer == null)
                {
                    log.Debug("Creating new clean up timer...");
                    timer = new System.Threading.Timer(Expire, null, cleanUpInterval, cleanUpInterval);
                }
            }
        }

        public bool TryGet(TKey key, out TValue value)
        {
            lock (items)
            {
                if (items.ContainsKey(key))
                {
                    items[key].Renew(renewal);
                    value = items[key].Item;
                    return true;
                }
            }
            log.Debug("Leasable item not found");
            value = default(TValue);
            return false;
        }

        public bool Remove(TKey key)
        {
            return Remove(key, true);
        }

        public bool Remove(TKey key, bool expire)
        {
            lock (items)
            {
                if (expire && items.ContainsKey(key))
                {
                    items[key].Expire();
                }
                bool removed = items.Remove(key);
                if (items.Count == 0)
                {
                    if (timer != null)
                    {
                        log.Debug("Disposing and removing reference to clean up timer...");
                        timer.Dispose();
                        timer = null;
                    }
                }
                return removed;
            }
        }

        public void Expire(bool force)
        {
            lock (items)
            {
                // iterate over all items, marking ones that are expired 
                List<TKey> expired = new List<TKey>();
                foreach (KeyValuePair<TKey, Leasable<TValue>> pair in items)
                {
                    if (force || pair.Value.IsExpired)
                    {
                        expired.Add(pair.Key);
                    }
                }

                // remove expired items
                foreach (TKey key in expired)
                {
                    Remove(key, true);
                }
            }
        }

        /// <summary>
        /// Called internally by cleaner thread.
        /// </summary>
        private void Expire(object state)
        {
            try
            {
                Expire(false);
            }
            catch (InvalidOperationException exception)
            {
                log.Error(exception);
            }
        }

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            Expire(true);
        }

        #endregion
    }
}
