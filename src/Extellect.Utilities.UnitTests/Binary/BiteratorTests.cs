using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Extellect.Binary
{
    public class BiteratorTests
    {

        [Fact]
        public void Read_x2()
        {
            var bits = new Biterator(new byte[] { 0xA });
            Assert.Equal(1, bits.Read(5));
            Assert.Equal(2, bits.Read(3));
        }

        [Fact]
        public void Read_x4()
        {
            var bits = new Biterator(new byte[] { 1, 2, 3 });
            Assert.Equal(0, bits.Read(1));
            Assert.Equal(0, bits.Read(2));
            Assert.Equal(0, bits.Read(3));
            Assert.Equal(1, bits.Read(2));
        }

        [Fact]
        public void Read_QRCode()
        {
            var bits = new Biterator(new byte[] { 0x20, 0x55, 0xb2, 0x56, 0xbf });
            Assert.Equal(2, bits.Read(4)); // mode
            Assert.Equal(10, bits.Read(9)); // num chars
            Assert.Equal(1458, bits.Read(11)); // char pair
            Assert.Equal(693, bits.Read(11)); // char pair
        }
    }
}
