using System;
using Xunit;

namespace Extellect.Utilities.Math
{
    
    public class MatrixFixture
    {
        [Fact]
        public void ToString_2x2_IsPretty()
        {
            var matrix = new Matrix(
                new double[,] { 
                    { 1, 2 }, 
                    { 3, 4 }
                });
            var toString = matrix.ToString();
            Assert.Equal("1\t2\r\n3\t4", toString);
        }

        [Fact]
        public void ToString_3x3_IsPretty()
        {
            var matrix = new Matrix(
                new double[,] { 
                    { 1, 2, 3 }, 
                    { 4, 5, 6 } 
                });
            var toString = matrix.ToString();
            Assert.Equal("1\t2\t3\r\n4\t5\t6", toString);
        }

        [Fact]
        public void Identity_1_LooksOk()
        {
            var matrix = Matrix.Identity(1);
            var expected = new Matrix(
                    new double[,] { 
                        { 1 } 
                    });
            Assert.Equal(expected, matrix);
        }

        [Fact]
        public void Identity_2_LooksOk()
        {
            var matrix = Matrix.Identity(2);
            var expected = new Matrix(
                new double[,] { 
                    { 1, 0 }, 
                    { 0, 1 } 
                });
            Assert.Equal(expected, matrix);
        }

        [Fact]
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
            Assert.Equal(expected, matrix);
        }

        [Fact]
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
                Assert.True(false);
            }
            var expected = new Matrix(
                new double[,] {
                    { 3, -1, -1 }, 
                    { -4, 2, 1 }, 
                    { -1, 0, 1 } 
                });
            Assert.Equal(expected, inverse);
        }

        [Fact]
        public void IEquatableEquals()
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
            Assert.True(a.Equals(b));
        }

        [Fact]
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
            Assert.Equal((object)a, (object)b);
        }

        [Fact]
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
            Assert.Equal(a.GetHashCode(), b.GetHashCode());
        }

        [Fact]
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
            Assert.Equal(expected, matrix.Transpose());
        }

        [Fact]
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
                Assert.True(false);
            }

            var expected = new Matrix(
                new double[,] { 
                    { 7, 7, 7 }, 
                    { 7, 7, 7 } 
                });

            Assert.Equal(expected, result);
        }

        [Fact]
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

            Assert.Equal(expected, a.Minor(0, 0));
        }

        [Fact]
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

            Assert.Equal(expected, a.Minor(2, 0));
        }

        [Fact]
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

            Assert.Equal(expected, a.Minor(2, 2));
        }

        [Fact]
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

            Assert.Equal(expected, a.Minor(1, 1));
        }

        [Fact]
        public void Determinant_OfIdentityMatrix_IsAlwaysOne()
        {
            var a = Matrix.Identity(3);
            
            Assert.Equal(1, a.Determinant);
        }

        [Fact]
        public void Determinant()
        {
            var a = new Matrix(
                new double[,] { 
                    { -2, 2, -3 }, 
                    { -1, 1, 3 },
                    { 2, 0, -1 },
                });

            Assert.Equal(18, a.Determinant);
        }
    }
}

