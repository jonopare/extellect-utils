#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Text;

namespace Extellect.Leasing
{
    public class Lease : ILease
    {
        private DateTime expires;

        public Lease()
            : this(TimeSpan.Zero)
        {
        }

        public Lease(TimeSpan initial)
        {
            Renew(initial);
        }

        public void Renew(TimeSpan amount)
        {
            expires = Clock.UtcNow.Add(amount);
        }

        public bool IsExpired
        {
            get { return expires < Clock.UtcNow; }
        }
    }
}
