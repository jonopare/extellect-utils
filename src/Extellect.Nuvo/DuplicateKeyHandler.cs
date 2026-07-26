namespace Extellect.Nuvo
{
    public class DuplicateKeyHandler<TKey>
    {
        private readonly Action<string, TKey> _onDuplicateKey;

        public string Name { get; }

        public DuplicateKeyHandler(string name, Action<string, TKey> onDuplicateKey)
        {
            Name = name;
            _onDuplicateKey = onDuplicateKey;
        }

        public void Handle(TKey duplicateKey)
        {
            _onDuplicateKey(Name, duplicateKey);
        }
    }
}
