using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Extellect.Utilities.Testing;

namespace Extellect.Utilities.Execution
{
    
    public class MapReduceFixture
    {
        private class LetterCount : MapReduce<string, char, int>
        {
            protected override IEnumerable<KeyValuePair<char, int>> Map(string input)
            {
                return input.Select(x => new KeyValuePair<char, int>(x, 1));
            }

            protected override KeyValuePair<char, int> Reduce(KeyValuePair<char, IEnumerable<int>> intermediate)
            {
                return new KeyValuePair<char, int>(intermediate.Key, intermediate.Value.Sum());
            }
        }

        private readonly string[] _letterCountInput = new[] { "the", "quick", "brown", "fox", "jumps", "over", "the", "lazy", "dog" };

        private readonly Dictionary<char, int> _expected = new Dictionary<char, int>()
            {
                { 'a', 1 },
                { 'b', 1 },
                { 'c', 1 },
                { 'd', 1 },
                { 'e', 3 },
                { 'f', 1 },
                { 'g', 1 },
                { 'h', 2 },
                { 'i', 1 },
                { 'j', 1 },
                { 'k', 1 },
                { 'l', 1 },
                { 'm', 1 },
                { 'n', 1 },
                { 'o', 4 },
                { 'p', 1 },
                { 'q', 1 },
                { 'r', 2 },
                { 's', 1 },
                { 't', 2 },
                { 'u', 2 },
                { 'v', 1 },
                { 'w', 1 },
                { 'x', 1 },
                { 'y', 1 },
                { 'z', 1 },
            };

        [Fact]
        public void SanityCheck()
        {
            Dictionary<char, int> actual = Enumerable.Range('a', 26).ToDictionary(x => (char)x, x => 0);
            foreach (var letter in _letterCountInput.SelectMany(x => x))
            {
                actual[letter]++;
            }

            AssertionHelper.AreSequencesEqual(_expected.OrderBy(x => x.Key), actual.OrderBy(x => x.Key));
        }

        [Fact]
        public void MapReduce_Run_LetterCount()
        {
            var actual = new Dictionary<char, int>();

            var letterCount = new LetterCount();
            letterCount.Run(
                _letterCountInput,
                x => actual.Add(x.Key, x.Value)
                ).Wait();

            AssertionHelper.AreSequencesEqual(_expected.OrderBy(x => x.Key), actual.OrderBy(x => x.Key));
        }
    }
}
