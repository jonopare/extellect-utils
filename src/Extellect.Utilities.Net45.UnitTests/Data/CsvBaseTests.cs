using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Extellect.Utilities.Testing;
using System.IO;

namespace Extellect.Utilities.Data
{
    
    public class CsvBaseTests
    {
        private enum CsvEvents
        {
            EndOfField,
            EndOfLine,
        }

        private class TestCsvBase : CsvBase
        {
            public List<CsvEvents> EventHistory { get; private set; }

            public TestCsvBase()
            {
                EventHistory = new List<CsvEvents>();
            }

            protected override void DoEndOfField()
            {
                EventHistory.Add(CsvEvents.EndOfField);
            }

            protected override void DoEndOfLine()
            {
                EventHistory.Add(CsvEvents.EndOfLine);
            }
        }

        [Fact]
        public void Foo()
        {
            TestCsvBase csv = new TestCsvBase();
            var text = @"a,b,c
";

            using (var reader = new StringReader(text))
            {
                csv.Read(reader);
            }

            var expected = new List<CsvEvents> 
            {
                CsvEvents.EndOfField /*a*/, CsvEvents.EndOfField /*b*/, CsvEvents.EndOfField /*c*/, CsvEvents.EndOfLine,
                CsvEvents.EndOfField /*empty*/, CsvEvents.EndOfLine
            };

            AssertionHelper.AreSequencesEqual(expected, csv.EventHistory);
        }
    }
}
