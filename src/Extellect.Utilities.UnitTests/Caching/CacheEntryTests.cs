using Moq;
using System;
using Xunit;

namespace Extellect.Caching
{
    public class CacheEntryTests
    {
        private readonly Mock<IClock> _clockMock;

        public CacheEntryTests()
        {
            _clockMock = new Mock<IClock>();
            _clockMock.Setup(x => x.UtcNow);
        }

        [Theory]
        [InlineData(-1, false)]
        [InlineData(0, true)]
        [InlineData(1, true)]
        public void CacheEntry_IsExpired(double timeToExpiry, bool isExpired)
        {
            var expires = new DateTime(2017, 3, 16);
            _clockMock.Setup(x => x.UtcNow).Returns(expires.Add(TimeSpan.FromSeconds(timeToExpiry)));

            var sut = new CacheEntry<string>(_clockMock.Object, "Dummy", expires);

            Assert.Equal(isExpired, sut.IsExpired);
            _clockMock.Verify(x => x.UtcNow, Times.Once);
        }

        [Fact]
        public void CacheEntry_ContainsItem()
        {
            var sut = new CacheEntry<string>(_clockMock.Object, "Hello", null);

            Assert.Equal("Hello", sut.Item);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(20160316)]
        public void ConstructorSetsExpiresProperty(int? expiresInt)
        {
            DateTime? expires = expiresInt.HasValue ? expiresInt.Value.ToDate() : (DateTime?)null;

            var sut = new CacheEntry<string>(_clockMock.Object, "Hello", expires);

            Assert.Equal(expires, sut.Expires);
        }
    }
}
