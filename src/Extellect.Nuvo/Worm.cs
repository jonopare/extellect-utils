namespace Extellect.Nuvo
{
    /// <summary>
    /// Write-once, read-many.
    /// </summary>
    public class Worm<T>
    {
        private T? _value;
        private bool _written;

        public T Value
        {
            get
            {
                return _value!;
            }
            set
            {
                if (_written)
                    throw new InvalidOperationException("Immutable value has already been written once.");
                _value = value;
                _written = true;
            }
        }

        public static implicit operator T(Worm<T> worm) => worm.Value;

        public bool HasValue => _written;
    }
}
