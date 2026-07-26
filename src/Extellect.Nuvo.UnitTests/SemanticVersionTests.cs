namespace Extellect.Nuvo
{
    public class SemanticVersionTests
    {
        [Fact]
        public void Parse_Valid_Returns()
        {
            var sut = SemanticVersion.Parse("1.2.3");
            Assert.Equal(1, sut.Major);
            Assert.Equal(2, sut.Minor);
            Assert.Equal(3, sut.Patch);
        }

        [Theory]
        [InlineData(".2.3")]
        [InlineData("1.2.3.4")]
        [InlineData("1.two.3.4")]
        public void Parse_Invalid_Throws(string versionText)
        {
            var caught = Assert.Throws<FormatException>(() => SemanticVersion.Parse(versionText));
        }

        [Theory]
        [InlineData(SemanticVersionParts.Patch, "1.2.4")]
        [InlineData(SemanticVersionParts.Minor, "1.3.0")]
        [InlineData(SemanticVersionParts.Major, "2.0.0")]
        public void Bump_IncreasesPartAndResetsLesserParts(SemanticVersionParts part, string expected)
        {
            var sut = new SemanticVersion(1, 2, 3);
            var inc = sut.Bump(part);
            Assert.Equal(expected, inc.ToString());
        }

        [Theory]
        [InlineData("0.2.2", 1)]
        [InlineData("1.1.2", 1)]
        [InlineData("1.2.2", 1)]
        [InlineData("1.2.3", 0)]
        [InlineData("1.2.4", -1)]
        [InlineData("1.3.0", -1)]
        [InlineData("2.0.0", -1)]
        public void CompareTo(string comparand, int expected)
        {
            var sut = new SemanticVersion(1, 2, 3);
            var actual = sut.CompareTo(SemanticVersion.Parse(comparand));
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("0.2.2", false)]
        [InlineData("1.1.2", false)]
        [InlineData("1.2.2", false)]
        [InlineData("1.2.3", true)]
        [InlineData("1.2.4", false)]
        [InlineData("1.3.0", false)]
        [InlineData("2.0.0", false)]
        [InlineData(null, false)]
        public void Equals_Both(string? comparand, bool expected)
        {
            var sut = new SemanticVersion(1, 2, 3);
            var other = comparand != null ? SemanticVersion.Parse(comparand) : null;
            Assert.Equal(expected, sut.Equals(other)); // IEquatable<T>
            Assert.Equal(expected, sut.Equals((object?)other)); // object
        }

        [Fact]
        public void ImplicitCastToVersion()
        {
            var sut = new SemanticVersion(1, 2, 3);
            Version actual = sut;
            Assert.Equal("1.2.3", actual.ToString());
        }

        [Fact]
        public void ExplicitCastFromVersion()
        {
            var version = new Version(1, 2, 3, 4);
            var actual = (SemanticVersion)version;
            Assert.Equal("1.2.3", actual.ToString());
        }
    }
}
