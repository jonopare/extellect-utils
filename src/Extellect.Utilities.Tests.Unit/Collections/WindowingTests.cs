using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Extellect.Utilities.Collections;
using System.IO;

namespace Extellect.Utilities.Collections
{
    
    public class WindowingTests
    {
        [Fact]
        public void Find_ZeroLengthPattern()
        {
            List<byte> pattern = new List<byte> { };
            List<byte> sequence = new List<byte> { 1, 2, 3, 2, 1 };
            List<int> expected = new List<int> { 0, 1, 2, 3, 4 };
            Assert.True(expected.SequenceEqual(sequence.Find(pattern, false)));
        }

        [Fact]
        public void Find_ZeroLengthSequence()
        {
            List<byte> pattern = new List<byte> { 1 };
            List<byte> sequence = new List<byte> { };
            List<int> expected = new List<int> { };
            Assert.True(expected.SequenceEqual(sequence.Find(pattern, false)));
        }

        [Fact]
        public void Find1a()
        {
            List<byte> pattern = new List<byte> { 1 };
            List<byte> sequence = new List<byte> { 1, 2, 3, 2, 1 };
            List<int> expected = new List<int> { 0, 4 };
            Assert.True(expected.SequenceEqual(sequence.Find(pattern, false)));
        }

        [Fact]
        public void Find1b()
        {
            List<byte> pattern = new List<byte> { 1 };
            List<byte> sequence = new List<byte> { 1, 1, 1, 1, 1 };
            List<int> expected = new List<int> { 0, 1, 2, 3, 4 };
            Assert.True(expected.SequenceEqual(sequence.Find(pattern, false)));
        }

        [Fact]
        public void Find1c()
        {
            List<byte> pattern = new List<byte> { 1 };
            List<byte> sequence = new List<byte> { 2, 1, 1, 1, 3 };
            List<int> expected = new List<int> { 1, 2, 3 };
            Assert.True(expected.SequenceEqual(sequence.Find(pattern, false)));
        }

        [Fact]
        public void Find2a()
        {
            List<byte> pattern = new List<byte> { 1, 2 };
            List<byte> sequence = new List<byte> { 1, 2, 3, 2, 1 };
            List<int> expected = new List<int> { 0 };
            Assert.True(expected.SequenceEqual(sequence.Find(pattern, false)));
        }

        [Fact]
        public void Find2b()
        {
            List<byte> pattern = new List<byte> { 2, 1 };
            List<byte> sequence = new List<byte> { 1, 2, 3, 2, 1 };
            List<int> expected = new List<int> { 3 };
            Assert.True(expected.SequenceEqual(sequence.Find(pattern, false)));
        }

        [Fact]
        public void Find2c()
        {
            List<byte> pattern = new List<byte> { 2, 3 };
            List<byte> sequence = new List<byte> { 1, 2, 3, 2, 3 };
            List<int> expected = new List<int> { 1, 3 };
            Assert.True(expected.SequenceEqual(sequence.Find(pattern, false)));
        }

        [Fact]
        public void FindPatternLongerThanSequence()
        {
            List<byte> pattern = new List<byte> { 1, 2, 3, 2, 3 };
            List<byte> sequence = new List<byte> { 2, 3 };
            List<int> expected = new List<int> { };
            Assert.True(expected.SequenceEqual(sequence.Find(pattern, false)));
        }

        [Fact]
        public void FindPatternEqualsSequence()
        {
            List<byte> pattern = new List<byte> { 1, 2, 3, 4, 5 };
            List<byte> sequence = new List<byte> { 1, 2, 3, 4, 5 };
            List<int> expected = new List<int> { 0 };
            Assert.True(expected.SequenceEqual(sequence.Find(pattern, false)));
        }

        [Fact]
        public void Find2aOverlap()
        {
            List<byte> pattern = new List<byte> { 1, 1 };
            List<byte> sequence = new List<byte> { 2, 1, 1, 1, 3 };
            List<int> expected = new List<int> { 1, 2 };
            Assert.True(expected.SequenceEqual(sequence.Find(pattern, true)));
        }

        [Fact]
        public void FindEmptyPatternOverlap()
        {
            List<byte> pattern = new List<byte> { };
            List<byte> sequence = new List<byte> { 2, 1, 1, 1, 3 };
            List<int> expected = new List<int> { 0, 1, 2, 3, 4 };
            Assert.True(expected.SequenceEqual(sequence.Find(pattern, true)));
        }

        [Fact]
        public void FindInStream()
        {
            List<byte> pattern = Encoding.UTF8.GetBytes("Paré").ToList();
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes("Jono Paré wrote this test. Who wrote it? Jono Paré did.").ToArray(), false))
            {
                List<int> expected = new List<int> { 5, 47 };
                Assert.True(expected.SequenceEqual(stream.Find(pattern, true)));
            }
        }
    }
}
