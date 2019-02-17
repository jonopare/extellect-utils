#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Extellect
{
    public class MultiDimensionCounter
    {
        private Range<int>[] _ranges;
        private int[] _values;
        public MultiDimensionCounter(params Range<int>[] ranges)
        {
            _ranges = ranges;
            _values = new int[_ranges.Length];
            Reset();
        }

        public void Reset()
        {
            for (int i = 0; i < _ranges.Length; i++)
            {
                _values[i] = _ranges[i].MinValue;
            }
        }

        public bool Increment()
        {
            for (int i = _values.Length - 1; i >= 0; i--)
            {
                _values[i] += 1;
                if (_values[i] > _ranges[i].MaxValue)
                {
                    if (i == 0)
                    {
                        Reset();
                        return false;
                    }
                    _values[i] = _ranges[i].MinValue;
                }
                else
                {
                    break;
                }
            }
            return true;
        }

        public int[] Current => _values;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _values.Length; i++)
            {
                if (i != 0)
                {
                    sb.Append(',');
                }
                sb.Append(_values[i]);
            }
            return sb.ToString();
        }

        public string ToString(string formatString)
        {
            Type[] parameterTypes = new Type[] { typeof(string), typeof(object[])};
            MethodInfo method = typeof(string).GetMethod("Format", parameterTypes);
            
            object[] objectArray = new object[_values.Length];
            Array.Copy(_values, objectArray, _values.Length);

            object[] parameters = new object[] { formatString, objectArray };
            
            return (string)method.Invoke(null, parameters);
        }
    }
}
