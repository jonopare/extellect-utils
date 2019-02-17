using Extellect.Sequencing;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Extellect.Math
{
    public class BoxMullerTests
    {
        [Fact]
        public void NextDouble()
        {
            var randoms = new Queue<double>(
                Enumerable.Repeat(Enumerable.Range(0, 102).Select(x => x / 100d), 2)
                .SelectMany(x => x));

            var randomMock = new Mock<IIterable<double>>();
            randomMock.Setup(x => x.Next()).Returns(() => randoms.Dequeue());

            var sut = new BoxMuller(randomMock.Object);

            var ts = Enumerable.Range(0, 101)
                .Select(x => sut.NextDouble())
                .ToList();

            Assert.Empty(ts.Where(double.IsInfinity));
            Assert.Empty(ts.Where(double.IsNaN));
        }

        [Fact]
        public void NextDouble_Epsilon()
        {
            var randomMock = new Mock<IIterable<double>>();
            randomMock.Setup(x => x.Next()).Returns(double.Epsilon);

            var sut = new BoxMuller(randomMock.Object);

            var random = sut.NextDouble();

            Assert.Equal(38.586009690595922, random, 10);
        }
    }
}
