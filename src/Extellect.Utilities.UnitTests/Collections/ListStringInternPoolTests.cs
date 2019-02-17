using System;
using Xunit;
using System.Diagnostics;

namespace Extellect.Collections
{
    
    public class ListStringInternPoolTests
    {
        [Fact]
        public void Intern_EachOf10000Strings_100Times()
        {
            var stopwatch = Stopwatch.StartNew();
            using (var sut = new ListStringInternPool())
            {
                for (var i = 0; i < 1000000; i++)
                {
                    sut.Intern($"prefix{i % 100}suffix");
                }
                Assert.Equal(100, sut.Count);
            }
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);
        }
    }
}
