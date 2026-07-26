namespace Extellect.Nuvo
{
    public class BinaryTests
    {
        [Theory]
        [InlineData("A", "A", "B", true)]
        [InlineData("B", "A", "B", false)]
        [InlineData("a", "A", "B", true)]
        [InlineData("b", "A", "B", false)]
        public void Parse(string value, string trueValue, string falseValue, bool expected)
        {
            var actual = Binary.Parse(value, trueValue, falseValue);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Parse_NullValue()
        {
            Assert.Throws<ArgumentNullException>(() => Binary.Parse(null, "true", "false"));
        }

        [Fact]
        public void Parse_MissingValue()
        {
            Assert.Throws<FormatException>(() => Binary.Parse("maybe", "true", "false"));
        }
    }
}