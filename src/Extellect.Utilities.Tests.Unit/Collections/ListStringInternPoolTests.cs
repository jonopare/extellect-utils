using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Extellect.Utilities.Collections
{
    [TestClass]
    public class ListStringInternPoolTests
    {
        [TestMethod]
        public void Intern_EachOf10000Strings_100Times()
        {
            var stopwatch = Stopwatch.StartNew();
            using (var sut = new ListStringInternPool())
            {
                for (var i = 0; i < 1000000; i++)
                {
                    sut.Intern($"prefix{i % 100}suffix");
                }
                Assert.AreEqual(100, sut.Count);
            }
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);
        }
    }
}
