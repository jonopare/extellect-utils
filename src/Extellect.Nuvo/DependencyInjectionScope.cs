#nullable enable
using Microsoft.Extensions.DependencyInjection;

namespace Extellect.Nuvo
{
    /// <summary>
    /// Internally developed Adapter for AspNetCore's lifetime scope so that our root service objects
    /// can resolve other services within specific scopes that are not tied to the HTTP request
    /// scopes that may have been used to create the root service objects.
    /// </summary>
    public class DependencyInjectionScope : IDependencyInjectionScope
    {
        private readonly IServiceScope _serviceScope;
        private readonly IRequestContext _parentRequestContext;

        public DependencyInjectionScope(IServiceScope serviceScope, IRequestContext parentRequestContext)
        {
            _serviceScope = serviceScope;
            _parentRequestContext = parentRequestContext;
        }

        public void Dispose() => _serviceScope.Dispose();

        public TService Resolve<TService>() where TService : notnull
        {
            return _serviceScope.ServiceProvider.GetRequiredService<TService>();
        }

        public IRequestContext InitializeRequestContext(Guid? tenancyID = null, User? user = null)
        {
            var requestContext = Resolve<IRequestContext>();
            requestContext.TenancyID = tenancyID ?? _parentRequestContext.TenancyID;
            requestContext.User = user ?? _parentRequestContext.User;
            return requestContext;
        }
    }
}
