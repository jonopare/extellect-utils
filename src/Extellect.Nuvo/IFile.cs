namespace Extellect.Nuvo
{
    public interface IFile
    {
        public string FileName { get; }
        public string ContentType { get; }
        public byte[] Content { get; }
    }
}
