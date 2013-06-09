using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Extellect.Utilities.Collections;

namespace Extellect.Utilities.Tests.Unit.Collections
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
        public void TestMethod1()
        {
            Indexable<TestItem> items = new Indexable<TestItem>();
            var byId = items.CreateUniqueIndex<int>(i => i.Id);
            var byCode = items.CreateUniqueIndex<string>(i => i.Code);
            items.Add(new TestItem { Id = 1, Code = "One", Description = "First" });
            items.Add(new TestItem { Id = 2, Code = "Two", Description = "Second" });
            Console.WriteLine(byCode["One"].Description);
        }
    }
}
