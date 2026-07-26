namespace Extellect.Nuvo
{
    public class StringExtensionsTests
    {
        [Theory]
        [InlineData("COFFEE_IS_GOOD", "CoffeeIsGood")]
        public void ToCamelCase(string value, string expected)
        {
            var actual = value.ToPascalCase();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("CoffeeIsGood", "Coffee Is Good")]
        public void FromCamelCase(string value, string expected)
        {
            var actual = value.FromPascalCase();
            Assert.Equal(expected, string.Join(" ", actual));
        }

        [Theory]
        [InlineData(null, 13, null)]
        [InlineData("DUCK", -1, "CTBJ")]
        [InlineData("DUCK", 0, "DUCK")]
        [InlineData("DUCK", 13, "QHPX")]
        [InlineData("DUCK", 25, "CTBJ")]
        [InlineData("DUCK", 26, "DUCK")]
        [InlineData("QHPX", 13, "DUCK")]
        [InlineData("duck", -1, "ctbj")]
        [InlineData("duck", 0, "duck")]
        [InlineData("duck", 13, "qhpx")]
        [InlineData("duck", 25, "ctbj")]
        [InlineData("duck", 26, "duck")]
        [InlineData("qhpx", 13, "duck")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@", 13, "NOPQRSTUVWXYZABCDEFGHIJKLMnopqrstuvwxyzabcdefghijklm0123456789!@")]
        public void Rot(string value, int n, string expected)
        {
            var actual = value.Rot(n);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("a", "a")]
        [InlineData("ab", "ba")]
        [InlineData("abc", "bac")]
        [InlineData("abcd", "badc")]
        public void Shuffle(string value, string expected)
        {
            var actual = value.Shuffle();
            Assert.Equal(expected, actual);
        }
    }
}