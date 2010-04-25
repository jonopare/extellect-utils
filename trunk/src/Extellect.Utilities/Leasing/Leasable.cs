using System;
using System.Collections.Generic;
using System.Text;
using log4net;

namespace Extellect.Utilities.Leasing
{
    public class Leasable<T> : ILease
    {
        //private readonly ILog log = LogManager.GetLogger(typeof(Leasable<T>));
        private T item;
        private Lease lease;
        private Action<T> expiry;

        public Leasable(T item, Lease lease, Action<T> expiry)
        {
            this.item = item;
            this.lease = lease;
            this.expiry = expiry;
        }

        public void Expire()
        {
            if (expiry != null)
            {
                expiry(item);
            }
        }

        public T Item
        {
            get { return item; }
        }

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
