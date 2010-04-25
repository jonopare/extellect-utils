#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Text;
using log4net;

namespace Extellect.Utilities.Leasing
{
    public class Lease : ILease
    {
        //private readonly ILog log = LogManager.GetLogger(typeof(Lease));
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
            expires = DateTime.UtcNow.Add(amount);
        }

        public bool IsExpired
        {
            get { return expires < DateTime.UtcNow; }
        }
    }
}
