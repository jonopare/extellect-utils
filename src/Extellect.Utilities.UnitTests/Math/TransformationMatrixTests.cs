using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Extellect.Utilities.Math
{
    public class TransformationMatrixTests
    {
        [Fact]
        public void Transform()
        {
            var transform = new TransformationMatrix(1.5, 1.5, 0, 0, 100, 100);

            transform = transform.Transform(2, 2, 0, 0, 10, 10);

            var actual = transform.ApplyTo(new Triplet(100, 100, 0));

            Assert.Equal(new Triplet(510, 510, 0), actual);
        }

        [Theory]
        [InlineData(System.Math.PI / 3, -36.6025403784438, 136.602540378444)]
        [InlineData(0, 100, 100)]
        [InlineData(System.Math.PI, -100, -100)]
        [InlineData(2 * System.Math.PI, 100, 100)]
        public void Rotate(double theta, double x, double y)
        {
            var transform = new TransformationMatrix()
                .Rotate(theta);

            var actual = transform.ApplyTo(new Triplet(100, 100, 0));

            Assert.Equal(new Triplet(x, y, 0), actual, new TripletComparer(12));
        }

        [Fact]
        public void Scale()
        {
            var transform = new TransformationMatrix()
                .Scale(23, 67);

            var actual = transform.ApplyTo(new Triplet(100, 100, 0));

            Assert.Equal(new Triplet(2300, 6700, 0), actual);
        }

        [Fact]
        public void Translate()
        {
            var transform = new TransformationMatrix()
                .Translate(23, 67);

            var actual = transform.ApplyTo(new Triplet(100, 100, 0));

            Assert.Equal(new Triplet(123, 167, 0), actual);
        }

        [Theory]
        [InlineData(23, 67, 100 + 23 * 50, 50 + 67 * 100)]
        [InlineData(17, 0, 100 + 17 * 50, 50)]
        [InlineData(0, 61, 100, 50 + 61 * 100)]
        public void Skew(double skewX, double skewY, double x, double y)
        {
            var transform = new TransformationMatrix()
                .Skew(skewX, skewY);

            var actual = transform.ApplyTo(new Triplet(100, 50, 0));

            Assert.Equal(new Triplet(x, y, 0), actual, new TripletComparer(12));
        }
    }
}
