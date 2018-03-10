using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Extellect.Utilities.Collections;

namespace Extellect.Utilities.Collections
{
    
    public class IndexableTests
    {
        public class TestItem
        {
            public int Id;
            public string Code;
            public string Description;
        }

        [Fact]
        public void CreateUniqueIndex_DistinctValues_AllowsRetrieval()
        {
            Indexable<TestItem> items = new Indexable<TestItem>();
            var byId = items.CreateUniqueIndex<int>(i => i.Id);
            var byCode = items.CreateUniqueIndex<string>(i => i.Code);
            items.Add(new TestItem { Id = 1, Code = "One", Description = "First" });
            items.Add(new TestItem { Id = 2, Code = "Two", Description = "Second" });
            Assert.Equal("One", byId[1].Code);
            Assert.Equal("Two", byId[2].Code);
        }

        [Fact]
        public void CreateUniqueIndex_NonDistinctValues_FailsAtInsertion()
        {
            Indexable<TestItem> items = new Indexable<TestItem>();
            var byId = items.CreateUniqueIndex<int>(i => i.Id);
            var byCode = items.CreateUniqueIndex<string>(i => i.Code);
            items.Add(new TestItem { Id = 1, Code = "One", Description = "First" });
            Assert.Throws<ArgumentException>(() => items.Add(new TestItem { Id = 2, Code = "One", Description = "Duplicate of first" }));
            // ^^ should fail at above line with message: "An entry with the same key already exists."
        }
    }
}
