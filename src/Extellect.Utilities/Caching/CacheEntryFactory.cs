#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extellect.Utilities.Caching
{
    public class CacheEntryFactory<T> : ICacheEntryFactory<T>
    {
        private readonly IClock _clock;

        public CacheEntryFactory(IClock clock)
        {
            _clock = clock;
        }

        public ICacheEntry<T> NewEntry(T item, DateTime? expires)
        {
            return new CacheEntry<T>(_clock, item, expires);
        }

        public ICacheEntry<T> NewEntry(T item, TimeSpan expires)
        {
            return new CacheEntry<T>(_clock, item, _clock.UtcNow.Add(expires));
        }

        public ICacheEntry<T> NewEntry(T item)
        {
            return new CacheEntry<T>(_clock, item);
        }
    }
}
