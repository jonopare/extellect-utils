namespace Extellect.Nuvo
{
    public class MemoryFile : IFile
    {
        public string FileName { get; }
        public string ContentType { get; }
        public byte[] Content { get; }

        public MemoryFile(string fileName, string contentType, byte[] content)
        {
            FileName = fileName;
            ContentType = contentType;
            Content = content;
        }
    }
}
