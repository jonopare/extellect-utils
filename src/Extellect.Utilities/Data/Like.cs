using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Extellect.Utilities.Data
{
    /// <summary>
    /// Implementation of the LIKE operator as used in SQL
    /// </summary>
    public class Like
    {
        /// <summary>
        /// Stack frame used by the IsMatch method
        /// </summary>
        private class LikeStackFrame
        {
            public int P { get; private set; }
            public int V { get; private set; }

            public LikeStackFrame(int p, int v)
            {
                P = p;
                V = v;
            }
        }

        private readonly string _pattern;

        /// <summary>
        /// Creates a new LIKE object with the specified pattern.
        /// </summary>
        public Like(string pattern)
        {
            if (pattern == null)
            {
                throw new ArgumentNullException("pattern");
            }

            _pattern = Normalize(pattern);
        }

        private static string Normalize(string pattern)
        {
            var regex = new Regex("(?<wildcard>[_%]+)|(?<text>[^_%]+)");
            var reduced = new StringBuilder();
            foreach (Match match in regex.Matches(pattern))
            {
                var wildcard = match.Groups["wildcard"];
                if (wildcard.Length > 0)
                {
                    Reduce(wildcard.Value, reduced);
                }
                else
                {
                    reduced.Append(match.Groups["text"].Value);
                }
            }
            return reduced.ToString();
        }

        private static void Reduce(string wildcards, StringBuilder reduced)
        {
            var isUnbounded = false;
            foreach (var wildcard in wildcards)
            {
                switch (wildcard)
                {
                    case '_':
                        reduced.Append("_");
                        break;
                    case '%':
                        isUnbounded = true;
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }
            if (isUnbounded)
            {
                reduced.Append("%");
            }
        }

        /// <summary>
        /// Returns true when the specified value matches this object's pattern.
        /// </summary>
        public bool IsMatch(string value)
        {
            var stack = new Stack<LikeStackFrame>();
            var v = 0;
            for (var p = 0; p < _pattern.Length; p++)
            {
                if (v == value.Length)
                {
                    if (stack.Any())
                    {
                        var frame = stack.Pop();
                        p = frame.P - 1;
                        v = frame.V + 1;
                        continue; // will bump p
                    }
                    else
                    {
                        return false;
                    }
                }

                switch (_pattern[p])
                {
                    case '_':
                        v++;
                        break;
                    case '%':
                        stack.Push(new LikeStackFrame(p, v));
                        break;
                    default:
                        if (_pattern[p] == value[v])
                        {
                            v++;
                        }
                        else
                        {
                            if (stack.Any())
                            {
                                var frame = stack.Pop();
                                p = frame.P - 1;
                                v = frame.V + 1;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        break;
                }

                if (p == _pattern.Length - 1 && _pattern[_pattern.Length - 1] != '%' && v != value.Length)
                {
                    if (stack.Any())
                    {
                        var frame = stack.Pop();
                        p = frame.P - 1;
                        v = frame.V + 1;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return v == value.Length || (_pattern.Length > 0 && _pattern[_pattern.Length - 1] == '%');
        }
    }
}
