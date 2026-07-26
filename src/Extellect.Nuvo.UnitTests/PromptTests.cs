using Moq;

namespace Extellect.Nuvo
{
    public class PromptTests
    {
        [Theory]
        [InlineData("Y", true)]
        [InlineData("y", true)]
        [InlineData("", true)]
        [InlineData("N", false)]
        [InlineData("n", false)]
        public void Ask_ValidResponse(string responses, bool expectedAnswer)
        {
            var expectedLines = "Does this test pass? [y/n] (default: y)\r\n";
            using var stringWriter = new StringWriter();
            var responseQueue = new Queue<string>(responses.Split('|'));
            var textReaderMock = new Mock<TextReader>(MockBehavior.Strict);
            textReaderMock.Setup(x => x.ReadLine())
                .Returns(responseQueue.Dequeue);
            var sut = new Prompt(stringWriter, textReaderMock.Object);
            var answer = sut.Ask("Does this test pass?");
            Assert.Equal(expectedAnswer, (bool)answer.Value);
            Assert.Equal(expectedLines, stringWriter.ToString());
            Assert.Empty(responseQueue);
        }

        [Theory]
        [InlineData("x|Y", true)]
        [InlineData("x|y", true)]
        [InlineData("x|", true)]
        [InlineData("x|N", false)]
        [InlineData("x|n", false)]
        public void Ask_InvalidResponse(string responses, bool expectedAnswer)
        {
            var expectedLines = "Does this test pass? [y/n] (default: y)\r\nCouldn't convert answer; please try again\r\n[y/n] (default: y)\r\n";
            using var stringWriter = new StringWriter();
            var responseQueue = new Queue<string>(responses.Split('|'));
            var textReaderMock = new Mock<TextReader>(MockBehavior.Strict);
            textReaderMock.Setup(x => x.ReadLine())
                .Returns(responseQueue.Dequeue);
            var sut = new Prompt(stringWriter, textReaderMock.Object);
            var answer = sut.Ask("Does this test pass?");
            Assert.Equal(expectedAnswer, (bool)answer.Value);
            Assert.Equal(expectedLines, stringWriter.ToString());
            Assert.Empty(responseQueue);
        }
    }
}
