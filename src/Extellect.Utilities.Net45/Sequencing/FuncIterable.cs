#pragma warning disable 1591
using System;

namespace Extellect.Utilities.Sequencing
{
    public class FuncIterable<T> : IIterable<T>
    {
        private readonly Func<T> _source;

        public FuncIterable(Func<T> source)
        {
            _source = source;
        }

        public T Next()
        {
            return _source();
        }
    }
}
