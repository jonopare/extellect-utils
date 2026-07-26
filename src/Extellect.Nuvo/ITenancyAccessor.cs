namespace Extellect.Nuvo
{
    public interface ITenancyAccessor
    {
        Maybe<Guid> GetTenancyIdentifier();
    }
}
