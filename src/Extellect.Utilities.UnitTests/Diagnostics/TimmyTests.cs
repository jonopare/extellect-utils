using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Extellect.Utilities.Diagnostics
{
    public class TimmyTests
    {
        [Fact]
        public void Timmy()
        {
            using (var memoryStream = new MemoryStream())
            using (var streamWriter = new StreamWriter(memoryStream))
            {
                using (new Timmy("A unit test", streamWriter))
                {
                    Thread.Sleep(25);
                }
                streamWriter.Flush();
                var actual = Encoding.UTF8.GetString(memoryStream.ToArray());
                Assert.Matches(@"^A unit test took [\d,]+(\.\d+)? ms", actual);
            }
        }
    }
}
