namespace Extellect.Nuvo
{
    public class Dependency
    {
        public static DependencyBuilder<T> Create<T>(T node) => new DependencyBuilder<T>(node);
    }

    public class Dependency<T>
    {
        public T Independent { get; }
        public T Dependent { get; }

        public Dependency(T independent, T dependent)
        {
            Independent = independent;
            Dependent = dependent;
        }
    }
}
