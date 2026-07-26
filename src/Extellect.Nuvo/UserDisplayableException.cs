namespace Extellect.Nuvo
{
    [Serializable]
    public class UserDisplayableException : ApplicationException
    {
        private static (Guid, string) UnexpectedFailureMessage(ISecureRandom? secureRandom = null, string? operation = null)
        {
            Guid reference;
            if (secureRandom == null)
            {
                reference = Guid.NewGuid();
            }
            else
            {
                var buffer = new byte[16];
                secureRandom.Fill(buffer);
                reference = new Guid(buffer);
            }
            if (operation == null)
            {
                operation = "fulfilling your request";
            }
            return (reference, $"An unexpected error prevented us from {operation}. Please wait and try again. If the problem persists, please quote this reference to a member of our support team [{reference}]");
        }

        public static UserDisplayableException UnexpectedFailure(Exception inner, ISecureRandom? secureRandom = null, string? operation = null)
        {
            if (inner is UserDisplayableException ude)
            {
                return ude;
            }
            var ufm = UnexpectedFailureMessage(secureRandom, operation);
            return new UserDisplayableException(ufm.Item1, ufm.Item2, inner);
        }

        public bool IsUnexpected => SupportReferenceID != null;
        public Guid? SupportReferenceID { get; private set; }

        public UserDisplayableException() { }
        public UserDisplayableException(string message) : this(null, message) { }
        public UserDisplayableException(string message, Exception inner) : this(null, message, inner) { }
        private UserDisplayableException(Guid? supportReferenceID, string message) : base(message) { SupportReferenceID = supportReferenceID; }
        private UserDisplayableException(Guid? supportReferenceID, string message, Exception inner) : base(message, inner) { SupportReferenceID = supportReferenceID; }

        [Obsolete(DiagnosticId = "SYSLIB0051")]
        protected UserDisplayableException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

}
