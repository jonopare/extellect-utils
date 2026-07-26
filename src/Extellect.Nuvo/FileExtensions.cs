using System.Text.RegularExpressions;
using System.Text;

namespace Extellect.Nuvo
{
    public static class FileExtensions
    {
        private readonly static char[] _invalidChars = Path.GetInvalidFileNameChars()
            .Concat(Path.GetInvalidPathChars())
            .Distinct()
            .ToArray();

        private readonly static Regex _multiDotRegex = new Regex(@"(?<dots>\.\.+)");

        public static string Declaw(this string partialFileName)
        {
            if (partialFileName.IndexOfAny(_invalidChars) >= 0)
            {
                var temp = new StringBuilder(partialFileName);
                foreach (var invalidChar in _invalidChars)
                {
                    temp.Replace(invalidChar, '_');
                }
                partialFileName = temp.ToString();
            }

            // The exact behaviour shouldn't matter but we just don't 
            // want to enable some kind of directory traversal with
            // double dots.
            string removeConsecutiveDots(Match match)
            {
                var length = match.Groups["dots"].Value.Length;
                if (length % 3 == 0)
                    return new string('…', length / 3);
                else if (length % 2 == 0)
                    return new string('‥', length / 2);
                else
                    return new string('‥', length / 2) + ".";
            }

            return _multiDotRegex.Replace(partialFileName, removeConsecutiveDots);
        }
    }
}
