namespace Extellect.Nuvo
{
    public static class DependencyInjection
    {
        public static IDependencyInjectionContainer? Container { get; private set; }

        public static void Initialize(IDependencyInjectionContainer container)
        {
            Container = container;
        }
    }
}
