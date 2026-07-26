namespace Extellect.Nuvo
{
    public sealed class Some<TValue> : Maybe<TValue>
    {
        public override bool HasValue => true;
        public override TValue Value { get; }

        public Some(TValue value)
        {
            Value = value;
        }

        public override string? ToString()
        {
            return Value?.ToString();
        }

        public static implicit operator Some<TValue>(TValue value)
        {
            return new Some<TValue>(value);
        }

        public static implicit operator TValue(Some<TValue> some)
        {
            return some.Value;
        }
    }
}
