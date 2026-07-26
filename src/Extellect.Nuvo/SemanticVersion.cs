using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace Extellect.Nuvo
{
    public sealed class SemanticVersion : IEquatable<SemanticVersion>, IComparable<SemanticVersion>
    {
        public int Major { get; }
        public int Minor { get; }
        public int Patch { get; }

        public static SemanticVersion Parse(string text)
        {
            var regex = new Regex(@"^(?<major>\d+)\.(?<minor>\d+)\.(?<patch>\d+)$");
            var match = regex.Match(text);
            if (!match.Success)
                throw new FormatException();

            return new SemanticVersion(int.Parse(match.Groups["major"].Value),
                int.Parse(match.Groups["minor"].Value),
                int.Parse(match.Groups["patch"].Value));
        }

        public SemanticVersion(int major, int minor, int patch)
        {
            if (major < 0)
                throw new ArgumentOutOfRangeException(nameof(major));
            if (minor < 0)
                throw new ArgumentOutOfRangeException(nameof(minor));
            if (patch < 0)
                throw new ArgumentOutOfRangeException(nameof(patch));

            Major = major;
            Minor = minor;
            Patch = patch;
        }

        public SemanticVersion Bump(SemanticVersionParts parts)
        {
            switch (parts)
            {
                case SemanticVersionParts.Patch:
                    return new SemanticVersion(Major, Minor, checked(Patch + 1));
                case SemanticVersionParts.Minor:
                    return new SemanticVersion(Major, checked(Minor + 1), 0);
                case SemanticVersionParts.Major:
                    return new SemanticVersion(checked(Major + 1), 0, 0);
                default:
                    throw new NotSupportedException();
            }
        }

        public override string ToString()
        {
            return $"{Major}.{Minor}.{Patch}";
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Major, Minor, Patch);
        }

        public override bool Equals(object? obj)
        {
            if (obj is SemanticVersion other)
            {
                return Equals(other);
            }
            return false;
        }

        public bool Equals(SemanticVersion? other)
        {
            return other != null
                && Major == other.Major
                && Minor == other.Minor
                && Patch == other.Patch;
        }

        public int CompareTo(SemanticVersion? other)
        {
            if (other == null)
                return -1;

            var selectors = new Func<SemanticVersion, IComparable>[]
            {
                x => x.Major,
                x => x.Minor,
                x => x.Patch,
            };

            foreach (var selector in selectors)
            {
                var result = selector(this).CompareTo(selector(other));
                if (result != 0)
                {
                    return result;
                }
            }

            return 0;
        }

        public static implicit operator Version(SemanticVersion value)
        {
            return new Version(value.Major, value.Minor, value.Patch);
        }

        public static explicit operator SemanticVersion(Version value)
        {
            return new SemanticVersion(value.Major, value.Minor, value.Build);
        }
    }
}
