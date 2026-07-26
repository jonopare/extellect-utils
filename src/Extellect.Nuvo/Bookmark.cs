using System.Diagnostics.CodeAnalysis;

namespace Extellect.Nuvo
{
    public static class Bookmark
    {
        public static Bookmark<TKey, TValue> Create<TKey, TValue>(IList<TValue> values, Func<TValue, TKey> keySelector) where TKey : notnull
        {
            return Bookmark<TKey, TValue>.Create(values, keySelector);
        }
    }

    public class Bookmark<TKey, TValue> where TKey : notnull
    {
        private readonly Dictionary<TKey, int> _lookup;
        private readonly IList<TValue> _values;

        private Bookmark(Dictionary<TKey, int> lookup, IList<TValue> values)
        {
            _lookup = lookup;
            _values = values;
        }

        public static Bookmark<TKey, TValue> Create(IList<TValue> values, Func<TValue, TKey> keySelector)
        {
            var lookup = new Dictionary<TKey, int>(values.Count);
            for (var i = 0; i < values.Count; i++)
            {
                lookup.Add(keySelector(values[i]), i);
            }
            return new Bookmark<TKey, TValue>(lookup, values);
        }

        public bool TryGetIndex(TKey key, [MaybeNullWhen(false)] out int index)
        {
            return _lookup.TryGetValue(key, out index);
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            if (_lookup.TryGetValue(key, out var index))
            {
                value = _values[index];
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }
    }
}
