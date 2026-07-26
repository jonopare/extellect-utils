namespace Extellect.Nuvo
{
    public class NullETagGenerator : IETagGenerator
    {
        public string NewETag() => string.Empty;
    }
}
