namespace Extellect.Nuvo
{
    public class NumericalStringComparerTests
    {
        [Theory]
        [InlineData(null, null, 0)]
        [InlineData(null, "", -1)]
        [InlineData("", null, 1)]
        [InlineData("", "", 0)]
        [InlineData("aB", "Ab", 0)]
        [InlineData("Ab", "aB", 0)]
        [InlineData("AB", "AB", 0)]
        [InlineData("ab", "ab", 0)]
        [InlineData("abc", "ab", 1)]
        [InlineData("ab", "abc", -1)]
        [InlineData("ITEM 1", "ITEM 17", -1)]
        [InlineData("ITEM 2", "ITEM 17", -1)]
        [InlineData("ITEM 17", "ITEM 20", -1)]
        [InlineData("ITEM 17", "ITEM 1", 1)]
        [InlineData("ITEM 17", "ITEM 2", 1)]
        [InlineData("ITEM 20", "ITEM 17", 1)]
        [InlineData("ITEM 39 PART 9 TRANCHE 1", "ITEM 39 PART 9 TRANCHE 1", 0)]
        [InlineData("ITEM 39 PART 9 TRANCHE 1", "ITEM 39 PART 9 TRANCHE 2", -1)]
        [InlineData("ITEM 39 PART 9 TRANCHE 1", "ITEM 39 PART 10 TRANCHE 1", -1)]
        [InlineData("ITEM 39 PART 9 TRANCHE 1", "ITEM 39 PART 10 TRANCHE 2", -1)]
        public void Compare(string? x, string? y, int expected)
        {
            var actual = NumericalStringComparer.OrdinalIgnoreCase.Compare(x, y);

            Assert.Equal(expected, actual);
        }
    }
}