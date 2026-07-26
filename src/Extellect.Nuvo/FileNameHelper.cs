namespace Extellect.Nuvo
{
    public class FileNameHelper : IFileNameHelper
    {
        private readonly IClock _clock;

        public FileNameHelper(IClock clock)
        {
            _clock = clock;
        }

        public string FileName(string prefix, string extension)
        {
            var utcNow = _clock.UtcNow;
            return $"{prefix.Declaw()}-{utcNow:yyyyMMdd}-{utcNow:HHmmss}-{utcNow:fff}{extension.Declaw()}";
        }
    }
}
