﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Extellect.Math
{
    public class BinomialCoefficientTests
    {
        [Theory]
        [InlineData(1, 0, 1)]
        [InlineData(1, 1, 1)]
        [InlineData(2, 0, 1)]
        [InlineData(2, 1, 2)]
        [InlineData(2, 2, 1)]
        [InlineData(3, 0, 1)]
        [InlineData(3, 1, 3)]
        [InlineData(3, 2, 3)]
        [InlineData(3, 3, 1)]
        [InlineData(5, 2, 10)]
        [InlineData(7, 4, 35)]
        public void Combinations(int n, int r, int expected)
        {
            Assert.Equal(expected, BinomialCoefficient.Combinations(n, r));
        }

        [Fact]
        public void Combinations_Overflow()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => BinomialCoefficient.Combinations(30, 2));
        }

        [Fact]
        public void Combinations_Overflow_Finder()
        {
            var caught = false;
            for (var i = 18; i <= 1000; i++)
            {
                for (var j = 0; j <= i; j++)
                {
                    try
                    {
                        var count = BinomialCoefficient.Combinations(i, j);
                    }
                    catch (Exception)
                    {
                        caught = true;
                        break;
                    }
                }
                if (caught)
                {
                    break;
                }
            }
        }
    }
}
