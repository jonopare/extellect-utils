namespace Extellect.Nuvo
{
    public interface IDependencyInjectionScope : IDisposable
    {
        TService Resolve<TService>() where TService : notnull;
        IRequestContext InitializeRequestContext(Guid? tenancyID = null, User? user = null);
    }
}
