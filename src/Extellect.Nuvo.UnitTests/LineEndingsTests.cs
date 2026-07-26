namespace Extellect.Nuvo
{
    public class LineEndingsTests
    {
        [Theory]
        [InlineData("a\r\nb\r\nc\nd", "a\nb\nc\nd", "a\r\nb\r\nc\r\nd")]
        [InlineData("a\r\nb\r\nc\r\nd", "a\nb\nc\nd", "a\r\nb\r\nc\r\nd")]
        [InlineData("a\nb\nc\nd", "a\nb\nc\nd", "a\r\nb\r\nc\r\nd")]
        [InlineData("a\nb\r\nc\nd", "a\nb\nc\nd", "a\r\nb\r\nc\r\nd")]
        public void Analyze(string input, string unix, string windows)
        {
            var analysis = LineEndings.Analyze(input);
            Assert.Equal(unix, analysis.ToUnix());
            Assert.Equal(windows, analysis.ToWindows());
        }
    }
}
