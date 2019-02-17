#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extellect.Caching
{
    public interface ICacheEntryFactory<T>
    {
        ICacheEntry<T> NewEntry(T item, DateTime? expires);
        ICacheEntry<T> NewEntry(T item, TimeSpan expires);
        ICacheEntry<T> NewEntry(T item);
    }
}
