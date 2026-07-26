namespace Extellect.Nuvo
{
    public interface IRequestContext
    {
        public User? User { get; set; }
        public Guid? TenancyID { get; set; }
    }
}
