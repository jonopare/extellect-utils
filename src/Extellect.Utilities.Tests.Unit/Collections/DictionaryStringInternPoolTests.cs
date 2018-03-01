using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Extellect.Utilities.Collections
{
    [TestClass]
    public class DictionaryStringInternPoolTests
    {
        [TestMethod]
        public void Intern_EachOf10000Strings_100Times()
        {
            var stopwatch = Stopwatch.StartNew();
            using (var sut = new DictionaryStringInternPool())
            {
                for (var i = 0; i < 1000000; i++)
                {
                    var value = $"prefix{i % 100}suffix";
                    var result = sut.Intern(value);
                    Assert.IsTrue(ReferenceEquals(value, result));
                }
                Assert.AreEqual(100, sut.Count);
            }
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);
        }

        [TestMethod]
        public void IsInterned()
        {
            using (var sut = new DictionaryStringInternPool())
            {
                Func<int, string> make = x => $"prefix{x % 100}suffix";
                for (var i = 0; i < 100; i++)
                {
                    var value = make(i);

                    Assert.IsFalse(sut.IsInterned(value)); // just checking that the string hadn't already been interned

                    var interned = sut.Intern(value);

                    Assert.IsTrue(sut.IsInterned(value)); // this isn't really required

                    Assert.IsTrue(sut.IsInterned(interned)); // this is part of the contract - the result of intern must have been interned

                    Assert.IsFalse(sut.IsInterned(make(i))); // this is part of the contract - a new string can't have been interned
                }
                Assert.AreEqual(100, sut.Count);
            }
        }
    }
}
