using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extellect.Utilities.Collections
{
    public interface IIndex<TKey, TValue>
    {
        int Count { get; }

        TValue this[TKey key] { get; }

        bool Contains(TKey key);
    }


}
