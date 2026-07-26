namespace Extellect.Nuvo
{
    public class RequestContext : IRequestContext
    {
        private readonly Worm<User?> _user;
        private readonly Worm<Guid?> _tenancyID;

        public RequestContext()
        {
            _user = new Worm<User?>();
            _tenancyID = new Worm<Guid?>();
        }

        public User? User
        {
            get { return _user; }
            set { _user.Value = value; }
        }

        public Guid? TenancyID
        {
            get { return _tenancyID; }
            set { _tenancyID.Value = value; }
        }
    }
}
