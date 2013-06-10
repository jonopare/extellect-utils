using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Extellect.Utilities.Sequencing;

namespace Extellect.Utilities.Tests.Unit.Sequencing
{
    [TestClass]
    public class ArithmeticSequenceTests
    {
       [TestMethod]
        public void PositiveAscendingInclusive()
        {
            int counter = 0;
            int[] expected = new int[] { 2, 4, 6, 8, 10 };
            foreach (int item in new ListComprehension<int>(new ArithmeticSequence(1, 10), delegate(int value) { return value % 2 == 0; }).Generate())
            {
                Assert.AreEqual(expected[counter++], item);
            }
            Assert.AreEqual(5, counter);
        }

       [TestMethod]
        public void NegativeAscendingInclusive()
        {
            int counter = 0;
            int[] expected = new int[] { -10, -8, -6, -4, -2 };
            foreach (int item in new ListComprehension<int>(new ArithmeticSequence(-10, -1), delegate(int value) { return value % 2 == 0; }).Generate())
            {
                Assert.AreEqual(expected[counter++], item);
            }
            Assert.AreEqual(5, counter);
        }

       [TestMethod]
        public void PositiveDescendingInclusive()
        {
            int counter = 0;
            int[] expected = new int[] { 10, 8, 6, 4, 2 };
            foreach (int item in new ListComprehension<int>(new ArithmeticSequence(10, 1), delegate(int value) { return value % 2 == 0; }).Generate())
            {
                Assert.AreEqual(expected[counter++], item);
            }
            Assert.AreEqual(5, counter);
        }

       [TestMethod]
        public void NegativeDecendingInclusive()
        {
            int counter = 0;
            int[] expected = new int[] { -2, -4, -6, -8, -10 };
            foreach (int item in new ListComprehension<int>(new ArithmeticSequence(-1, -10), delegate(int value) { return value % 2 == 0; }).Generate())
            {
                Assert.AreEqual(expected[counter++], item);
            }
            Assert.AreEqual(5, counter);
        }
    }
}
