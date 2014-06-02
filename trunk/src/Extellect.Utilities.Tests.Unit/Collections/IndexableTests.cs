using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Extellect.Utilities.Collections;

namespace Extellect.Utilities.Collections
{
    [TestClass]
    public class IndexableTests
    {
        public class TestItem
        {
            public int Id;
            public string Code;
            public string Description;
        }

        [TestMethod]
        public void CreateUniqueIndex_DistinctValues_AllowsRetrieval()
        {
            Indexable<TestItem> items = new Indexable<TestItem>();
            var byId = items.CreateUniqueIndex<int>(i => i.Id);
            var byCode = items.CreateUniqueIndex<string>(i => i.Code);
            items.Add(new TestItem { Id = 1, Code = "One", Description = "First" });
            items.Add(new TestItem { Id = 2, Code = "Two", Description = "Second" });
            Assert.AreEqual("One", byId[1].Code);
            Assert.AreEqual("Two", byId[2].Code);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void CreateUniqueIndex_NonDistinctValues_FailsAtInsertion()
        {
            Indexable<TestItem> items = new Indexable<TestItem>();
            var byId = items.CreateUniqueIndex<int>(i => i.Id);
            var byCode = items.CreateUniqueIndex<string>(i => i.Code);
            items.Add(new TestItem { Id = 1, Code = "One", Description = "First" });
            items.Add(new TestItem { Id = 2, Code = "One", Description = "Duplicate of first" });
            // ^^ should fail at above line with message: "An entry with the same key already exists."
        }
    }
}
