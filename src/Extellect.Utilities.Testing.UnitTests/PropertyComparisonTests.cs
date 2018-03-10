using Xunit;

namespace Extellect.Utilities.Testing
{
    public class PropertyComparisonTests
    {
        private class Foo
        {
            public int A { get; set; }
            public int B { get; set; }
        }

        [Fact]
        public void TestMethod1()
        {
            AssertionHelper.ArePropertiesEqual(new Foo { A = 1, B = 2 }, new Foo { A = 1, B = 2 }, new PropertyComparisonBuilder<Foo>().WithPublicInstanceGetProperties().Build());
        }

        [Fact]
        public void TestMethod2()
        {
            AssertionHelper.ArePropertiesNotEqual(new Foo { A = 1, B = 3 }, new Foo { A = 1, B = 2 }, new PropertyComparisonBuilder<Foo>().WithPublicInstanceGetProperties().Build());
        }
    }
}
