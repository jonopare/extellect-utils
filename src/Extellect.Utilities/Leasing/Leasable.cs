#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Text;

namespace Extellect.Leasing
{
    public class Leasable<T> : ILease
    {
        private Lease lease;
        private readonly Action<T> expiry;

        public Leasable(T item, Lease lease, Action<T> expiry)
        {
            Item = item;
            this.lease = lease;
            this.expiry = expiry;
        }

        public void Expire()
        {
            expiry?.Invoke(Item);
        }

        public T Item { get; }

        #region ILease Members

        public bool IsExpired
        {
            get { return lease.IsExpired; }
        }

        public void Renew(TimeSpan amount)
        {
            lease.Renew(amount);
        }

        #endregion
    }
}
