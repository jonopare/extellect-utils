namespace Extellect.Nuvo
{
    public class VersionGenerator : IVersionGenerator
    {
        private DateTime Base { get; }

        public VersionGenerator(DateTime @base)
        {
            Base = @base;
        }

        public VersionGenerator(int year)
        {
            Base = new DateTime(year, 1, 1);
        }

        public Version Generate(short major, short minor)
        {
            return Generate(major, minor, DateTime.UtcNow);
        }

        public Version Generate(short major, short minor, DateTime utcNow)
        {
            if (utcNow < Base)
            {
                utcNow = Base;
            }
            return Generate(major, minor, Build(utcNow), Revision(utcNow));
        }

        public Version Generate(short major, short minor, short build, short revision)
        {
            return new Version(major, minor, build, revision);
        }

        /// <summary>
        /// This numbering scheme will be safe until the end of 32 years after the base year.
        /// </summary>
        public short Build(DateTime utcNow)
        {
            return (short)(((utcNow.Year - Base.Year) * 1000) + utcNow.DayOfYear);
        }

        public short Revision(DateTime utcNow)
        {
            return (short)(utcNow.Hour * 100 + utcNow.Minute);
        }
    }
}
