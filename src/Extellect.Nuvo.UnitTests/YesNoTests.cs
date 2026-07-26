namespace Extellect.Nuvo
{
    public class YesNoTests
    {
        [Fact]
        public void Ctor_NoArgs_SetsValueToFalse()
        {
            var sut = new YesNo();
            Assert.False(sut.Value);
        }

        [Fact]
        public void Ctor_SetsValue()
        {
            var sut = new YesNo(true);
            Assert.True(sut.Value);
        }

        [Fact]
        public void Implicit_CastToBoolean_GetsValue()
        {
            var sut = new YesNo(true);
            bool value = sut;
            Assert.True(value);
        }

        [Fact]
        public void Implicit_CastToYesNo_SetsValue()
        {
            YesNo sut = true;
            Assert.True(sut.Value);
        }
    }
}