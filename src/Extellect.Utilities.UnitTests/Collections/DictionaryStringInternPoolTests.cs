using System;
using Xunit;
using System.Diagnostics;

namespace Extellect.Utilities.Collections
{
    
    public class DictionaryStringInternPoolTests
    {
        [Fact]
        public void Intern_EachOf100Strings_10000Times()
        {
            var stopwatch = Stopwatch.StartNew();
            using (var sut = new DictionaryStringInternPool())
            {
                for (var i = 0; i < 10000; i++)
                {
                    var value = $"prefix{i % 100}suffix"; // always creates a new instance which won't have been interned
                    Assert.False(sut.IsInterned(value), $"String was already interned but shouldn't have been: {value}");
                    var result = sut.Intern(value);
                    Assert.True(sut.IsInterned(result), $"String was not interned when it should have been: {result}");
                }
                Assert.Equal(100, sut.Count);
            }
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);
        }

        [Fact]
        public void IsInterned()
        {
            using (var sut = new DictionaryStringInternPool())
            {
                Func<int, string> make = x => $"prefix{x % 100}suffix";
                for (var i = 0; i < 100; i++)
                {
                    var value = make(i);

                    Assert.False(sut.IsInterned(value)); // just checking that the string hadn't already been interned

                    var interned = sut.Intern(value);

                    Assert.True(sut.IsInterned(value)); // this isn't really required

                    Assert.True(sut.IsInterned(interned)); // this is part of the contract - the result of intern must have been interned

                    Assert.False(sut.IsInterned(make(i))); // this is part of the contract - a new string can't have been interned
                }
                Assert.Equal(100, sut.Count);
            }
        }
    }
}
