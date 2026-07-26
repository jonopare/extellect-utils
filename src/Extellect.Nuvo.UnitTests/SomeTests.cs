namespace Extellect.Nuvo
{
    public class SomeTests
    {
        [Fact]
        public void Ctor()
        {
            const string expected = "TEXT";
            var sut = new Some<string>(expected);
            Assert.True(sut.HasValue);
            Assert.Equal(expected, sut.Value);
        }

        [Fact]
        public void Deconstruct()
        {
            const string expected = "TEXT";
            var (hasValue, value) = new Some<string>(expected);
            Assert.True(hasValue);
            Assert.Equal(expected, value);
        }
    }
}
