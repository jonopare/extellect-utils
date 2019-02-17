using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Extellect.Sequencing;

namespace Extellect.Sequencing
{
    
    public class ArithmeticSequenceTests
    {
       [Fact]
        public void PositiveAscendingInclusive()
        {
            int counter = 0;
            int[] expected = new int[] { 2, 4, 6, 8, 10 };
            foreach (int item in new ListComprehension<int>(new ArithmeticSequence(1, 10), delegate(int value) { return value % 2 == 0; }).Generate())
            {
                Assert.Equal(expected[counter++], item);
            }
            Assert.Equal(5, counter);
        }

       [Fact]
        public void NegativeAscendingInclusive()
        {
            int counter = 0;
            int[] expected = new int[] { -10, -8, -6, -4, -2 };
            foreach (int item in new ListComprehension<int>(new ArithmeticSequence(-10, -1), delegate(int value) { return value % 2 == 0; }).Generate())
            {
                Assert.Equal(expected[counter++], item);
            }
            Assert.Equal(5, counter);
        }

       [Fact]
        public void PositiveDescendingInclusive()
        {
            int counter = 0;
            int[] expected = new int[] { 10, 8, 6, 4, 2 };
            foreach (int item in new ListComprehension<int>(new ArithmeticSequence(10, 1), delegate(int value) { return value % 2 == 0; }).Generate())
            {
                Assert.Equal(expected[counter++], item);
            }
            Assert.Equal(5, counter);
        }

       [Fact]
        public void NegativeDecendingInclusive()
        {
            int counter = 0;
            int[] expected = new int[] { -2, -4, -6, -8, -10 };
            foreach (int item in new ListComprehension<int>(new ArithmeticSequence(-1, -10), delegate(int value) { return value % 2 == 0; }).Generate())
            {
                Assert.Equal(expected[counter++], item);
            }
            Assert.Equal(5, counter);
        }
    }
}
