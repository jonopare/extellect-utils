#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extellect.Utilities.Caching
{
    public class CacheEntry<T> : ICacheEntry<T>
    {
        private readonly IClock _clock;

        public DateTime? Expires { get; }

        public bool IsExpired => Expires.HasValue && _clock.UtcNow >= Expires.Value;

        public T Item { get; }

        public CacheEntry(IClock clock, T item, DateTime? expires = null)
        {
            _clock = clock;
            Item = item;
            Expires = expires;
        }
    }
}
