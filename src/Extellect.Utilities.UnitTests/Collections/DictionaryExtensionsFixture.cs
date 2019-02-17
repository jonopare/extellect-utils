using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Extellect.Collections
{
    
    public class DictionaryExtensionsFixture
    {
        private readonly Dictionary<string, int> _dictionary;

        public DictionaryExtensionsFixture()
        {
            _dictionary = new Dictionary<string, int>
            {
                { "a", 1 },
                { "b", 2 },
            };
        }

        [Fact]
        public void AddOrUpdate_KeyFound()
        {
            var key = "a";

            var value = _dictionary.AddOrUpdate(key, 3, x => x + 10);

            Assert.Equal(11, value);
            Assert.Equal(11, _dictionary[key]);
        }

        [Fact]
        public void AddOrUpdate_KeyNotFound()
        {
            var key = "c";

            var value = _dictionary.AddOrUpdate(key, 3, x => x + 10);

            Assert.Equal(3, value);
            Assert.Equal(3, _dictionary[key]);
        }

        [Fact]
        public void GetValueOrDefault_KeyFound()
        {
            var key = "b";

            var value = _dictionary.GetValueOrDefault(key, () => { Assert.False(true); return 0; });

            Assert.Equal(2, value);
        }

        [Fact]
        public void GetValueOrDefault_KeyNotFound_FactoryMethod()
        {
            var key = "c";

            var value = _dictionary.GetValueOrDefault(key, () => 5);

            Assert.Equal(5, value);
        }

        [Fact]
        public void GetValueOrDefault_KeyNotFound_Value()
        {
            var key = "c";

            var value = _dictionary.GetValueOrDefault(key, 5);

            Assert.Equal(5, value);
        }
    }
}
