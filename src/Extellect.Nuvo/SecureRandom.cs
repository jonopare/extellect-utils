using System.Security.Cryptography;

namespace Extellect.Nuvo
{
    public sealed class SecureRandom : ISecureRandom, IDisposable
    {
        private readonly RandomNumberGenerator _random;

        public SecureRandom()
        {
            _random = RandomNumberGenerator.Create();
        }

        public void Fill(byte[] data)
        {
            lock (_random)
            {
                _random.GetNonZeroBytes(data);
            }
        }

        public void Dispose()
        {
            _random.Dispose();
        }
    }
}
