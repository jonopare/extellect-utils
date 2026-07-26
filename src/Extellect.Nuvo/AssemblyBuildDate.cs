namespace Extellect.Nuvo
{
    [AttributeUsage(validOn: AttributeTargets.Assembly)]
    public class AssemblyBuildDateAttribute : Attribute
    {
        public long Ticks { get; }

        public AssemblyBuildDateAttribute(long ticks)
        {
            Ticks = ticks;
        }
    }
}
