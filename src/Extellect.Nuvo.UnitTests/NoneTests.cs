namespace Extellect.Nuvo
{
    public class NoneTests
    {
        [Fact]
        public void Instance()
        {
            var sut = None<string>.Instance;
            Assert.False(sut.HasValue);
            Assert.Throws<InvalidOperationException>(() => sut.Value);
        }

        [Fact]
        public void Deconstruct()
        {
            var (hasValue, value) = None<string>.Instance;
            Assert.False(hasValue);
        }
    }
}
