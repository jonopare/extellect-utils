using System.Numerics;
using System.Text;

namespace Extellect.Nuvo
{
    public class NumericalStringComparer : IComparer<string>
    {
        private readonly StringComparer _stringComparer;

        public static NumericalStringComparer OrdinalIgnoreCase { get; } = new NumericalStringComparer(StringComparer.OrdinalIgnoreCase);

        private class CharacterRuns
        {
            private readonly string[] _nonDigits;
            private readonly BigInteger[] _digits;

            public static CharacterRuns Parse(string text)
            {
                var nonDigits = new List<string>();
                var digits = new List<string>();

                var run = new StringBuilder();
                bool wasDigit = false;
                for (var i = 0; i < text.Length; i++)
                {
                    if (wasDigit)
                    {
                        if (char.IsDigit(text[i]))
                        {
                            // continue digit
                            run.Append(text[i]);
                        }
                        else
                        {
                            // start new non digit
                            digits.Add(run.ToString());
                            run.Clear();
                            run.Append(text[i]);
                            wasDigit = false;
                        }
                    }
                    else
                    {
                        if (char.IsDigit(text[i]))
                        {
                            // start new digit
                            nonDigits.Add(run.ToString());
                            run.Clear();
                            run.Append(text[i]);
                            wasDigit = true;
                        }
                        else
                        {
                            // continue non digit
                            run.Append(text[i]);
                        }
                    }
                }
                if (wasDigit)
                {
                    digits.Add(run.ToString());
                }
                else
                {
                    nonDigits.Add(run.ToString());
                }
                return new CharacterRuns(nonDigits.ToArray(), digits.Select(BigInteger.Parse).ToArray());
            }

            private CharacterRuns(string[] nonDigits, BigInteger[] digits)
            {
                _nonDigits = nonDigits;
                _digits = digits;
            }

            public int Length => _nonDigits.Length + _digits.Length;

            public int CompareTo(CharacterRuns other, StringComparer stringComparer)
            {
                var m = Math.Min(Length, other.Length);
                for (var i = 0; i < m; i++)
                {
                    var j = i / 2;
                    if (i % 2 == 0)
                    {
                        var result = stringComparer.Compare(_nonDigits[j], other._nonDigits[j]);
                        if (result != 0)
                        {
                            return result;
                        }
                    }
                    else
                    {
                        var result = _digits[j].CompareTo(other._digits[j]);
                        if (result != 0)
                        {
                            return result;
                        }
                    }
                }
                return Length.CompareTo(other.Length);
            }
        }

        public NumericalStringComparer(StringComparer stringComparer)
        {
            _stringComparer = stringComparer;
        }

        public int Compare(string? x, string? y)
        {
            if (x == null)
            {
                return y == null ? 0 : -1;
            }
            else if (y == null)
            {
                return 1;
            }
            else
            {
                return CharacterRuns.Parse(x).CompareTo(CharacterRuns.Parse(y), _stringComparer);
            }
        }
    }
}
