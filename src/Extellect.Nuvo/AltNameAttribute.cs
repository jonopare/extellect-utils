namespace Extellect.Nuvo
{
    [AttributeUsage(AttributeTargets.Field)]
    public class AltNameAttribute : Attribute
    {
        public string[] AltNames { get; set; }

        public AltNameAttribute(params string[] altNames)
        {
            AltNames = altNames;
        }
    }
}
