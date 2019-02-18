#if NETFULL
using Extellect.Drawing.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Extellect.Drawing.Imaging
{
    public class IronbowTests
    {
        [Fact]
        public void Foo()
        {
            var colors = Enumerable.Range(0, byte.MaxValue)
                .Select(x => (byte)x)
                .Select(Ironbow.GetColorAt);

            var text = string.Join(Environment.NewLine, colors.Select(x => $"{x.R}\t{x.B}\t{x.G}"));
        }
    }
}
#endif