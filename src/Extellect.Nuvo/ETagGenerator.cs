namespace Extellect.Nuvo
{
    public class ETagGenerator : IETagGenerator
    {
        public static int ETagLength { get; set; } = 8;

        private readonly ISecureRandom _secureRandom;

        public ETagGenerator(ISecureRandom secureRandom)
        {
            _secureRandom = secureRandom;
        }

        public string NewETag()
        {
            var bytes = new byte[ETagLength];
            _secureRandom.Fill(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}
