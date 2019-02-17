using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extellect.Collections
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ListStringInternPool : IInternPool, IDisposable
    {
        private List<string> _values;

        /// <summary>
        /// 
        /// </summary>
        public ListStringInternPool()
        {
            _values = new List<string>();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            _values = null;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Intern(string value)
        {
            var index = _values.BinarySearch(value);
            if (index < 0)
            {
                _values.Insert(~index, value);
                return value;
            }
            return _values[index];
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsInterned(string value)
        {
            var index = _values.BinarySearch(value);
            return index >= 0 && ReferenceEquals(value, _values[index]);
        }

        /// <summary>
        /// 
        /// </summary>
        public int Count => _values.Count;
    }
}
