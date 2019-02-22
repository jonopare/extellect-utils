using Extellect.Data;
using System;
using System.Reflection;
using Xunit;

namespace Extellect.Excel.IntegrationTests
{
    public class NpoiLoaderTests
    {
        private class Table1 : IEquatable<Table1>
        {
            public int Col1 { get; set; }
            public string Col2 { get; set; }
            public string Col3 { get; set; }

            public bool Equals(Table1 other)
            {
                return Col1 == other.Col1
                    && Col2 == other.Col2
                    && Col3 == other.Col3;
            }
        }

        [Fact]
        public void Load()
        {
            using (var stream = typeof(NpoiLoaderTests).Assembly.GetManifestResourceStream("Extellect.Excel.IntegrationTests.Data.xlsx"))
            using (var loader = new NpoiLoader(stream))
            {
                var actual = loader.Load<Table1>();

                var expected = new[]
                {
                    new Table1 { Col1 = 1, Col2 = "AB", Col3 = "aaaaaaaaa" },
                    new Table1 { Col1 = 2, Col2 = "BC", Col3 = "bbbbbbbbbb" },
                    new Table1 { Col1 = 3, Col2 = "CD", Col3 = "ccc" },
                    new Table1 { Col1 = 4, Col2 = "DE", Col3 = "dddddd" },
                    new Table1 { Col1 = 5, Col2 = "EF", Col3 = "eeeee" },
                };

                Assert.Equal(expected, actual);
            }
        }
    }
}
