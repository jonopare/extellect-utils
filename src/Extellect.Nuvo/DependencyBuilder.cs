namespace Extellect.Nuvo
{
    public class DependencyBuilder<T>
    {
        private readonly T _node;

        public DependencyBuilder(T node)
        {
            _node = node;
        }

        public Dependency<T> DependsOn(T independent) => new Dependency<T>(independent, _node);

        public Dependency<T> IsDependencyOf(T dependent) => new Dependency<T>(_node, dependent);
    }
}
