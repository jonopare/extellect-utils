namespace Extellect.Nuvo
{
    public interface ISimilarity
    {
        Type ComparandType { get; }
        bool AreSimilar(object x, object y);
    }
}
