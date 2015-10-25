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

        [TestMethod]
        public void Transpose()
        {
            var matrix = new Matrix(
                new double[,] { 
                    { 1, 2, 3 }, 
                    { 4, 5, 6 } 
                });
            var expected = new Matrix(
                new double[,] { 
                    { 1, 4 },
                    { 2, 5 },
                    { 3, 6 }
                });
            Assert.AreEqual(expected, matrix.Transpose());
        }

        [TestMethod]
        public void TryAdd()
        {
            var a = new Matrix(
                new double[,] { 
                    { 1, 2, 3 }, 
                    { 4, 5, 6 } 
                });
            var b = new Matrix(
                new double[,] { 
                    { 6, 5, 4 }, 
                    { 3, 2, 1 } 
                });
            Matrix result;
            if (!a.TryAdd(b, out result))
            {
                Assert.Fail();
            }

            var expected = new Matrix(
                new double[,] { 
                    { 7, 7, 7 }, 
                    { 7, 7, 7 } 
                });

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Minor_i0_j0()
        {
            var a = new Matrix(
                new double[,] { 
                    { 1, 2, 3 }, 
                    { 4, 5, 6 },
                    { 7, 8, 9 },
                });

            var expected = new Matrix(
                new double[,] { 
                    { 5, 6 }, 
                    { 8, 9 } 
                });

            Assert.AreEqual(expected, a.Minor(0, 0));
        }

        [TestMethod]
        public void Minor_i2_j0()
        {
            var a = new Matrix(
                new double[,] { 
                    { 1, 2, 3 }, 
                    { 4, 5, 6 },
                    { 7, 8, 9 },
                });

            var expected = new Matrix(
                new double[,] { 
                    { 2, 3 }, 
                    { 5, 6 } 
                });

            Assert.AreEqual(expected, a.Minor(2, 0));
        }

        [TestMethod]
        public void Minor_i2_j2()
        {
            var a = new Matrix(
                new double[,] { 
                    { 1, 2, 3 }, 
                    { 4, 5, 6 },
                    { 7, 8, 9 },
                });

            var expected = new Matrix(
                new double[,] { 
                    { 1, 2 }, 
                    { 4, 5 } 
                });

            Assert.AreEqual(expected, a.Minor(2, 2));
        }

        [TestMethod]
        public void Minor_i1_j1()
        {
            var a = new Matrix(
                new double[,] { 
                    { 1, 2, 3 }, 
                    { 4, 5, 6 },
                    { 7, 8, 9 },
                });

            var expected = new Matrix(
                new double[,] { 
                    { 1, 3 }, 
                    { 7, 9 } 
                });

            Assert.AreEqual(expected, a.Minor(1, 1));
        }

        [TestMethod]
        public void Determinant_OfIdentityMatrix_IsAlwaysOne()
        {
            var a = Matrix.Identity(3);
            
            Assert.AreEqual(1, a.Determinant);
        }

        [TestMethod]
        public void Determinant()
        {
            var a = new Matrix(
                new double[,] { 
                    { -2, 2, -3 }, 
                    { -1, 1, 3 },
                    { 2, 0, -1 },
                });

            Assert.AreEqual(18, a.Determinant);
        }
    }
}

