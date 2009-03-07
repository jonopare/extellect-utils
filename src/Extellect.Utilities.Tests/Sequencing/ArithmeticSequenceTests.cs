using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Extellect.Utilities.Sequencing
{
    [TestFixture]
    public class ArithmeticSequenceTests
    {
        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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
