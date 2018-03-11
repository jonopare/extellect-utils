using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Extellect.Utilities
{
    public class EnumExtensionsTests
    {
        public enum Casio
        {
            [Description("Adds two numbers")]
            Plus,
            Minus,
        }

        [Theory]
        [InlineData("Adds two numbers", Casio.Plus)]
        [InlineData("Minus", Casio.Minus)]
        [InlineData("2147483647", (Casio)int.MaxValue)]
        public void Description(string expected, Casio operation)
        {
            Assert.Equal(expected, operation.Description());
        }
    }
}
