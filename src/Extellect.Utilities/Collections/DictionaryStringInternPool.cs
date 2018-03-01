﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extellect.Utilities.Collections
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class DictionaryStringInternPool : IInternPool, IDisposable
    {
        private Dictionary<string, string> _values;

        /// <summary>
        /// 
        /// </summary>
        public DictionaryStringInternPool()
        {
            _values = new Dictionary<string, string>();
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
            string result;
            if (_values.TryGetValue(value, out result))
            {
                return result;
            }
            _values.Add(value, value);
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsInterned(string value)
        {
            string result;
            if (_values.TryGetValue(value, out result))
            {
                return ReferenceEquals(value, result);
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Count => _values.Count;
    }
}
