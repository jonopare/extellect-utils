#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extellect.Utilities.Caching
{
    public interface ICacheEntry<T>
    {
        DateTime? Expires { get; }

        bool IsExpired { get; }

        T Item { get; }
    }
}
