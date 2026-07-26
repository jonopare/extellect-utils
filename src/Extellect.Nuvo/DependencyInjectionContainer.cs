using Microsoft.Extensions.DependencyInjection;

namespace Extellect.Nuvo
{
    /// <summary>
    /// Internally developed Adapter for AspNetCore's container so that our service objects
    /// can create new lifetime scopes.
    /// </summary>
    public class DependencyInjectionContainer : IDependencyInjectionContainer
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public DependencyInjectionContainer(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public IDependencyInjectionScope BeginLifetimeScope(IRequestContext parentRequestContext)
        {
            return new DependencyInjectionScope(_serviceScopeFactory.CreateScope(), parentRequestContext);
        }
    }
}
