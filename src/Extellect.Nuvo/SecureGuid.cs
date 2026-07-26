namespace Extellect.Nuvo
{
    public class SecureGuid : ISecureGuid
    {
        private readonly ISecureRandom _secureRandom;

        public SecureGuid(ISecureRandom secureRandom)
        {
            _secureRandom = secureRandom;
        }

        public Guid NewGuid()
        {
            var bytes = new byte[16];
            _secureRandom.Fill(bytes);
            return new Guid(bytes);
        }
    }
}
