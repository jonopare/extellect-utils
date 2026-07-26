namespace Extellect.Nuvo
{
    [Flags]
    public enum DeepPropertyComparisonOptions
    {
        None = 0,
        Enumerables = 1,
        Primitives = 2,
        All = Enumerables | Primitives,
    }
}
