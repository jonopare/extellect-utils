using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extellect.Utilities.Math
{
    [TestClass]
    public class MatrixFixture
    {
        [TestMethod]
        public void ToString_2x2_IsPretty()
        {
            var matrix = new Matrix(
                new double[,] { 
                    { 1, 2 }, 
                    { 3, 4 }
                });
            var toString = matrix.ToString();
            Assert.AreEqual("1\t2\r\n3\t4", toString);
        }

        [TestMethod]
        public void ToString_3x3_IsPretty()
        {
            var matrix = new Matrix(
                new double[,] { 
                    { 1, 2, 3 }, 
                    { 4, 5, 6 } 
                });
            var toString = matrix.ToString();
            Assert.AreEqual("1\t2\t3\r\n4\t5\t6", toString);
        }

        [TestMethod]
        public void Identity_1_LooksOk()
        {
            var matrix = Matrix.Identity(1);
            var expected = new Matrix(
                    new double[,] { 
                        { 1 } 
                    });
            Assert.AreEqual(expected, matrix);
        }

        [TestMethod]
        public void Identity_2_LooksOk()
        {
            var matrix = Matrix.Identity(2);
            var expected = new Matrix(
                new double[,] { 
                    { 1, 0 }, 
                    { 0, 1 } 
                });
            Assert.AreEqual(expected, matrix);
        }

        [TestMethod]
        public void Identity_5_LooksOk()
        {
            var matrix = Matrix.Identity(5);
            var expected = new Matrix(
                new double[,] { 
                    { 1, 0, 0, 0, 0 }, 
                    { 0, 1, 0, 0, 0 }, 
                    { 0, 0, 1, 0, 0 }, 
                    { 0, 0, 0, 1, 0 }, 
                    { 0, 0, 0, 0, 1 }
                });
            Assert.AreEqual(expected, matrix);
        }

        [TestMethod]
        public void TryInvert()
        {
            var matrix = new Matrix(
                new double[,] {
                    { 2, 1, 1 }, 
                    { 3, 2, 1 }, 
                    { 2, 1, 2 } 
                });
            Matrix inverse;
            if (!matrix.TryInvert(out inverse))
            {
                Assert.Fail();
            }
            var expected = new Matrix(
                new double[,] {
                    { 3, -1, -1 }, 
                    { -4, 2, 1 }, 
                    { -1, 0, 1 } 
                });
            Assert.AreEqual(expected, inverse);
        }

        [TestMethod]
        public void Equals()
        {
            var a = new Matrix(
                new double[,] { 
                    { 1, 2 }, 
                    { 3, 4 } 
                });
            var b = new Matrix(
                new double[,] { 
                    { 1, 2 }, 
                    { 3, 4 } 
                });
            Assert.AreEqual(a, b);
        }

        [TestMethod]
        public void OverriddenEquals()
        {
            var a = new Matrix(
                new double[,] { 
                    { 1, 2 },
                    { 3, 4 } 
                });
            var b = new Matrix(
                new double[,] {
                    { 1, 2 },
                    { 3, 4 } 
                });
            Assert.AreEqual((object)a, (object)b);
        }

        [TestMethod]
        public void OverriddenGetHashCode()
        {
            var a = new Matrix(
                new double[,] { 
                    { 1, 2 }, 
                    { 3, 4 } 
                });
            var b = new Matrix(
                new double[,] { 
                    { 1, 2 },
                    { 3, 4 } 
                });
            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        }
    }
}
