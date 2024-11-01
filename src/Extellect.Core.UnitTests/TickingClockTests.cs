namespace Extellect.Core.UnitTests
{
    public class TickingClockTests
    {
        [Fact]
        public void Ctor_Local()
        {
            var now = DateTime.Now;
            var sut = new TickingClock(now);
            Assert.Equal(now.ToUniversalTime(), sut.UtcNow);
        }
    }
}
