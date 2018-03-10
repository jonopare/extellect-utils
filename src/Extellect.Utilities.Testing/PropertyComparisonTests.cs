﻿using System;
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
        public void TestMethod1()
        {
            AssertionHelper.ArePropertiesEqual(new Foo { A = 1, B = 2 }, new Foo { A = 1, B = 2 }, new PropertyComparisonBuilder<Foo>().WithPublicInstanceGetProperties().Build());
        }

        [TestMethod]
        public void TestMethod2()
        {
            AssertionHelper.ArePropertiesNotEqual(new Foo { A = 1, B = 3 }, new Foo { A = 1, B = 2 }, new PropertyComparisonBuilder<Foo>().WithPublicInstanceGetProperties().Build());
        }
    }
}
