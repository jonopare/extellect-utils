namespace Extellect.Nuvo
{
    public interface IDependencyInjectionContainer
    {
        IDependencyInjectionScope BeginLifetimeScope(IRequestContext parentRequestContext);
    }
}
