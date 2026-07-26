using System.Diagnostics.CodeAnalysis;

namespace Extellect.Nuvo
{
    public static class Maybe
    {
        public static Maybe<T> None<T>()
        {
            return Maybe<T>.None;
        }

        public static Maybe<T> Some<T>(T value)
        {
            return Maybe<T>.Some(value);
        }
    }

    public abstract class Maybe<T>
    {
        public abstract bool HasValue { get; }
        public abstract T Value { get; } // Never null - None will throw an exception

        public static None<T> None { get; } = None<T>.Instance;
        public static Some<T> Some(T value) => new (value);

        public void Deconstruct(out bool hasValue, [MaybeNull] out T value)
        {
            hasValue = HasValue;
            value = hasValue ? Value : default;
        }
    }
}
