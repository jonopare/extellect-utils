#pragma warning disable 1591
using System;

namespace Extellect
{
    public struct Range<T>
        where T : IComparable<T>
    {
        private T _minValue;
        private T _maxValue;
        public Range(T minValue, T maxValue)
        {
            if (minValue.CompareTo(maxValue) > 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            _minValue = minValue;
            _maxValue = maxValue;
        }
        public T MinValue { get { return _minValue; } }
        public T MaxValue { get { return _maxValue; } }
    }
}
