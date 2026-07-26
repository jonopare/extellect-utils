namespace Extellect.Nuvo
{
    public class VersionGeneratorTests
    {
        private readonly VersionGenerator _sut;

        public VersionGeneratorTests()
        {
            _sut = new VersionGenerator(new DateTime(2019, 1, 1));
        }

        [Theory]
        [InlineData(2, 3, 20190913.1545, "2.3.256.1545")]
        [InlineData(2, 3, 20191231.2359, "2.3.365.2359")]
        [InlineData(2, 3, 20200101.0001, "2.3.1001.1")]
        [InlineData(2, 3, 20511231.2359, "2.3.32365.2359")]
        public void Generate(short major, short minor, decimal dateTimeUtcNow, string expected)
        {
            var actual = _sut.Generate(major, minor, dateTimeUtcNow.ToDateTime());
            Assert.Equal(expected, actual.ToString());
        }

        [Theory]
        [InlineData(20190913.1545, 256)]
        [InlineData(20191231.2359, 365)]
        [InlineData(20200101.0001, 1001)]
        public void Build(decimal dateTimeUtcNow, short expected)
        {
            var actual = _sut.Build(dateTimeUtcNow.ToDateTime());
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(20190913.1545, 1545)]
        [InlineData(20191231.2359, 2359)]
        [InlineData(20200101.0001, 0001)]
        public void Revision(decimal dateTimeUtcNow, short expected)
        {
            var actual = _sut.Revision(dateTimeUtcNow.ToDateTime());
            Assert.Equal(expected, actual);
        }
    }
}