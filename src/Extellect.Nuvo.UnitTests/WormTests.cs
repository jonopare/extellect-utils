namespace Extellect.Nuvo
{
    public class WormTests
    {
        [Fact]
        public void Set_SameValueTwice_ThrowsExceptionOnTheSecondTime()
        {
            const string value = "One";
            var sut = new Worm<string>();
            sut.Value = value;
            Assert.Equal(value, sut.Value);
            Assert.Throws<InvalidOperationException>(() => sut.Value = value);
        }
    }
}