#pragma warning disable 1591
using System;
using System.Collections.Generic;

namespace Extellect.Sequencing
{
    public class EnumerableIterable<T> : IIterable<T>, IDisposable
    {
        private IEnumerator<T> _enumerator;

        public EnumerableIterable(IEnumerable<T> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException();
            }
            _enumerator = items.GetEnumerator();
        }

        public T Next()
        {
            if (_enumerator == null || !_enumerator.MoveNext())
            {
                throw new InvalidOperationException();
            }
            return _enumerator.Current;
        }

        public void Dispose()
        {
            if (_enumerator != null)
            {
                _enumerator.Dispose();
                _enumerator = null;
            }
        }
    }

}
