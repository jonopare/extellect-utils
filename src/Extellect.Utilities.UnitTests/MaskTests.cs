using System;
using Xunit;

namespace Extellect.Utilities
{
    public class MaskTests
    {
        const string Input = "LONWS10888";

        [Fact]
        public void BlockMiddle_NegativeAnything_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Mask(-1, 1));
        }

        [Fact]
        public void BlockMiddle_AnythingNegative_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Mask(1, -1));
        }

        [Fact]
        public void BlockMiddle_ZeroZero_AllHidden()
        {
            var masked = new Mask(0, 0).BlockMiddle(Input);
            Assert.Equal("**********", masked);
        }

        [Fact]
        public void BlockMiddle_ZeroThree_888()
        {
            var masked = new Mask(0, 3).BlockMiddle(Input);
            Assert.Equal("*******888", masked);
        }

        [Fact]
        public void BlockMiddle_TwoThree_LO888()
        {
            var masked = new Mask(2, 3).BlockMiddle(Input);
            Assert.Equal("LO*****888", masked);
        }

        [Fact]
        public void BlockMiddle_SixFour_LONWS10888()
        {
            var masked = new Mask(6, 4).BlockMiddle(Input);
            Assert.Equal("LONWS10888", masked);
        }

        [Fact]
        public void BlockMiddle_SixFive_LONWS10888()
        {
            var masked = new Mask(6, 5).BlockMiddle(Input);
            Assert.Equal("LONWS10888", masked);
        }

        [Fact]
        public void BlockMiddle_FiveSix_LONWS10888()
        {
            var masked = new Mask(5, 6).BlockMiddle(Input);
            Assert.Equal("LONWS10888", masked);
        }
    }
}
