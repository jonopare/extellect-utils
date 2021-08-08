using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Extellect.Excel.IntegrationTests
{
    public class CellStyleBuilderTests
    {
        [Theory]
        [InlineData("red", "rgb(255,0,0)")]
        [InlineData("rgb(255, 0, 0)", "rgb(255,0,0)")]
        [InlineData("#FF0000", "rgb(255,0,0)")]
        public void NormalizeColor(string value, string expected)
        {
            var match = CellStyleBuilder._colorRegex.Match(value);
            
            var actual = CellStyleBuilder.NormalizeColor(match);

            Assert.Equal(expected, actual);
        }
    }
}
