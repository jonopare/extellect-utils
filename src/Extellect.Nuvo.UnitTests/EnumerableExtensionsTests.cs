namespace Extellect.Nuvo
{
    public class EnumerableExtensionsTests
    {
        //[Fact]
        //public void EqualsIEnumerableT_Null_Null_IsTrue()
        //{
        //    Assert.True(((IEnumerable<string>)null).SetEquals(null));
        //}

        //[Fact]
        //public void EqualsIEnumerableT_Null_Empty_IsTrue()
        //{
        //    Assert.True(((IEnumerable<string>)null).SetEquals(Enumerable.Empty<string>()));
        //}

        //[Fact]
        //public void EqualsIEnumerableT_Empty_Null_IsTrue()
        //{
        //    Assert.True(Enumerable.Empty<string>().SetEquals(null));
        //}

        [Fact]
        public void EqualsIEnumerableT_Empty_Empty_IsTrue()
        {
            Assert.True(Enumerable.Empty<string>().SetEquals(Enumerable.Empty<string>()));
        }

        [Theory]
        [InlineData(1, 5, 1, 5, true)]
        [InlineData(1, 5, 1, 6, false)]
        [InlineData(1, 6, 1, 5, false)]
        public void EqualIEnumerableT(int leftStart, int leftCount, int rightStart, int rightCount, bool expected)
        {
            var left = Enumerable.Range(leftStart, leftCount);
            var right = Enumerable.Range(rightStart, rightCount).OrderByDescending(x => x);

            Assert.Equal(expected, left.SetEquals(right));
        }

        [Theory]
        [InlineData("abcdefghijklmnopqrstuvwxyz", 1, "a|b|c|d|e|f|g|h|i|j|k|l|m|n|o|p|q|r|s|t|u|v|w|x|y|z")]
        [InlineData("abcdefghijklmnopqrstuvwxyz", 2, "ab|cd|ef|gh|ij|kl|mn|op|qr|st|uv|wx|yz")]
        [InlineData("abcdefghijklmnopqrstuvwxyz", 3, "abc|def|ghi|jkl|mno|pqr|stu|vwx|yz")]
        [InlineData("abcdefghijklmnopqrstuvwxyz", 26, "abcdefghijklmnopqrstuvwxyz")]
        [InlineData("abcdefghijklmnopqrstuvwxyz", 27, "abcdefghijklmnopqrstuvwxyz")]
        public void Batch(string input, int batchSize, string expected)
        {
            var actual = string.Join("|", input.Batch(batchSize).Select(x => new string(x)));
            Assert.Equal(expected, actual);
        }
    }
}