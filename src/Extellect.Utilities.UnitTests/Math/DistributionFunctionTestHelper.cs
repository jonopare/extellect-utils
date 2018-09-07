using Extellect.Utilities.Sequencing;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extellect.Utilities.Math
{
    public class DistributionFunctionTestHelper
    {
        private readonly Mock<IIterable<double>> _randomMock;
        private int _index;
        private int _count;

        public DistributionFunctionTestHelper(int count)
        {
            _randomMock = new Mock<IIterable<double>>();
            _randomMock.Setup(x => x.Next()).Returns(NextUniformRandomDouble);
            _count = count;
        }

        private double NextUniformRandomDouble() => (_index++ % _count) / (double)(_count - 1);

        public Mock<IIterable<double>> RandomMock => _randomMock;

        public int Count => _count;
    }
}
