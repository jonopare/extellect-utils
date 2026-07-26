namespace Extellect.Nuvo
{
    public interface IVersionGenerator
    {
        Version Generate(short major, short minor);
        Version Generate(short major, short minor, DateTime utcNow);
        Version Generate(short major, short minor, short build, short revision);
        short Build(DateTime utcNow);
        short Revision(DateTime utcNow);
    }
}
