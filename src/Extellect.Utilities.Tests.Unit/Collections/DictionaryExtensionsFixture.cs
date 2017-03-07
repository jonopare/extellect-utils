using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extellect.Utilities.Collections
{
    [TestClass]
    public class DictionaryExtensionsFixture
    {
        Dictionary<string, int> _dictionary;

        [TestInitialize]
        public void Init()
        {
            _dictionary = new Dictionary<string, int>
            {
                { "a", 1 },
                { "b", 2 },
            };
        }

        [TestMethod]
        public void AddOrUpdate_KeyFound()
        {
            var key = "a";

            var value = _dictionary.AddOrUpdate(key, 3, x => x + 10);

            Assert.AreEqual(11, value);
            Assert.AreEqual(11, _dictionary[key]);
        }

        [TestMethod]
        public void AddOrUpdate_KeyNotFound()
        {
            var key = "c";

            var value = _dictionary.AddOrUpdate(key, 3, x => x + 10);

            Assert.AreEqual(3, value);
            Assert.AreEqual(3, _dictionary[key]);
        }

        [TestMethod]
        public void GetValueOrDefault_KeyFound()
        {
            var key = "b";

            var value = _dictionary.GetValueOrDefault(key, () => { Assert.Fail(); return 0; });

            Assert.AreEqual(2, value);
        }

        [TestMethod]
        public void GetValueOrDefault_KeyNotFound_FactoryMethod()
        {
            var key = "c";

            var value = _dictionary.GetValueOrDefault(key, () => 5);

            Assert.AreEqual(5, value);
        }

        [TestMethod]
        public void GetValueOrDefault_KeyNotFound_Value()
        {
            var key = "c";

            var value = _dictionary.GetValueOrDefault(key, 5);

            Assert.AreEqual(5, value);
        }
    }
}
