using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extellect.Utilities.CLI
{
    [TestClass]
    public class ArgumentsTests
    {
        [TestMethod]
        public void IgnoreUnknown_DefaultValue_IsFalse()
        {
            var args = new Arguments();
            Assert.IsFalse(args.IgnoreUnknown);
        }

        [TestMethod]
        public void IgnoreMissing_DefaultValue_IsFalse()
        {
            var args = new Arguments();
            Assert.IsFalse(args.IgnoreMissing);
        }

        [TestMethod]
        public void IgnoreUnmarked_DefaultValue_IsFalse()
        {
            var args = new Arguments();
            Assert.IsTrue(args.IgnoreUnmarked);
        }
    }
}
