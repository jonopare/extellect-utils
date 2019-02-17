using System;
using Xunit;
using Extellect.Testing;

namespace Extellect
{
    
    public class PropertyComparisonTests
    {
        private class Foo
        {
            public int A { get; set; }
            public int B { get; set; }
        }

        [Fact]
        public void ArePropertiesEqual()
        {
            AssertionHelper.ArePropertiesEqual(new Foo { A = 1, B = 2 }, new Foo { A = 1, B = 2 }, new PropertyComparisonBuilder<Foo>().WithPublicInstanceGetProperties().Build());
        }

        [Fact]
        public void ArePropertiesEqual_Partial_ByString()
        {
            AssertionHelper.ArePropertiesEqual(new Foo { A = 1, B = 2 }, new Foo { A = 1, B = 3 }, new PropertyComparisonBuilder<Foo>().WithProperty(nameof(Foo.A)).Build());
        }

        [Fact]
        public void ArePropertiesEqual_Partial_ByExpressionFuncT()
        {
            AssertionHelper.ArePropertiesEqual(new Foo { A = 1, B = 2 }, new Foo { A = 1, B = 3 }, new PropertyComparisonBuilder<Foo>().WithProperty(x => x.A).Build());
        }

        [Fact]
        public void ArePropertiesNotEqual()
        {
            AssertionHelper.ArePropertiesNotEqual(new Foo { A = 1, B = 2 }, new Foo { A = 1, B = 3 }, new PropertyComparisonBuilder<Foo>().WithPublicInstanceGetProperties().Build());
        }

        [Fact]
        public void ArePropertiesNotEqual_Partial_ByString()
        {
            AssertionHelper.ArePropertiesNotEqual(new Foo { A = 1, B = 2 }, new Foo { A = 1, B = 3 }, new PropertyComparisonBuilder<Foo>().WithProperty(nameof(Foo.B)).Build());
        }

        [Fact]
        public void ArePropertiesNotEqual_Partial_ByExpressionFuncT()
        {
            AssertionHelper.ArePropertiesNotEqual(new Foo { A = 1, B = 2 }, new Foo { A = 1, B = 3 }, new PropertyComparisonBuilder<Foo>().WithProperty(x => x.B).Build());
        }
    }
}
