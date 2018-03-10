using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Extellect.Utilities.Testing;

namespace Extellect.Utilities
{
    [TestClass]
    public class PropertyComparisonTests
    {
        private class Foo
        {
            public int A { get; set; }
            public int B { get; set; }
        }

        [TestMethod]
        public void ArePropertiesEqual()
        {
            AssertionHelper.ArePropertiesEqual(new Foo { A = 1, B = 2 }, new Foo { A = 1, B = 2 }, new PropertyComparisonBuilder<Foo>().WithPublicInstanceGetProperties().Build());
        }

        [TestMethod]
        public void ArePropertiesEqual_Partial_ByString()
        {
            AssertionHelper.ArePropertiesEqual(new Foo { A = 1, B = 2 }, new Foo { A = 1, B = 3 }, new PropertyComparisonBuilder<Foo>().WithProperty(nameof(Foo.A)).Build());
        }

        [TestMethod]
        public void ArePropertiesEqual_Partial_ByExpressionFuncT()
        {
            AssertionHelper.ArePropertiesEqual(new Foo { A = 1, B = 2 }, new Foo { A = 1, B = 3 }, new PropertyComparisonBuilder<Foo>().WithProperty(x => x.A).Build());
        }

        [TestMethod]
        public void ArePropertiesNotEqual()
        {
            AssertionHelper.ArePropertiesNotEqual(new Foo { A = 1, B = 2 }, new Foo { A = 1, B = 3 }, new PropertyComparisonBuilder<Foo>().WithPublicInstanceGetProperties().Build());
        }

        [TestMethod]
        public void ArePropertiesNotEqual_Partial_ByString()
        {
            AssertionHelper.ArePropertiesNotEqual(new Foo { A = 1, B = 2 }, new Foo { A = 1, B = 3 }, new PropertyComparisonBuilder<Foo>().WithProperty(nameof(Foo.B)).Build());
        }

        [TestMethod]
        public void ArePropertiesNotEqual_Partial_ByExpressionFuncT()
        {
            AssertionHelper.ArePropertiesNotEqual(new Foo { A = 1, B = 2 }, new Foo { A = 1, B = 3 }, new PropertyComparisonBuilder<Foo>().WithProperty(x => x.B).Build());
        }
    }
}
