using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using System.IO;

namespace Extellect.Utilities.Data
{
    
    public class FileDataReaderTests
    {
        [Fact]
        public void TestMethod1()
        {
            var fileOrdinals = new Dictionary<string, int>
            {
                { "a", 0 },
                { "b", 1 },
                { "c", 2 },

            };

            var other = new Dictionary<string, Func<object>>
            {
                { "d", () => "delta" },
            };


            //using (var sr = new StringReader(@"alpha,beta,charlie"))
            //using (var rbcr = new DammitCharlie(sr))
            //using (var fdr = new FileDataReader(rbcr, fileOrdinals, other))
            //{
            //    var rowCount = 0;
            //    var ai = fdr.GetOrdinal("a");
            //    var bi = fdr.GetOrdinal("b");
            //    var ci = fdr.GetOrdinal("c");
            //    var di = fdr.GetOrdinal("d");
            //    while (fdr.Read())
            //    {
            //        rowCount++;
            //        var a = fdr.GetValue(ai);
            //        var b = fdr.GetValue(bi);
            //        var c = fdr.GetValue(ci);
            //        var d = fdr.GetValue(di);
            //    }
            //    Assert.Equal(rowCount, 1);
            //}
        }
    }
}
