namespace Extellect.Nuvo
{
    public sealed class None<TValue> : Maybe<TValue>
    {
        public override bool HasValue => false;
        public override TValue Value => throw new InvalidOperationException();

        private None()
        {
            
        }

        public static None<TValue> Instance { get; } = new None<TValue>();
    }
}
