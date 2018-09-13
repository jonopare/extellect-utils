using System;
using System.Collections.Generic;
using System.Linq;
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
            Assert.Equal("1,2;3,4", toString);
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
            Assert.Equal("1,2,3;4,5,6", toString);
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

        [Fact]
        public void Diagonal()
        {
            var d = Matrix.Diagonal(new[] { 2d, 3d });

            Assert.Equal(6, d.Determinant);
        }

        [Fact]
        public void TryAugment()
        {
            var a = new Matrix(new double[] { 2, 1, -1, -3, -1, 2, -2, 1, 2 }, 3, 3, true);
            var b = new Matrix(new double[] { 8, -11, -3 }, 3, 1, true);
            if (!a.TryAugment(b, out Matrix c))
            {
                Assert.False(true);
            }
            Assert.Equal("2,1,-1,8;-3,-1,2,-11;-2,1,2,-3", c.ToString());
        }

        [Theory]
        [InlineData(0, 0, 3, 4, "2,1,-1,8;-3,-1,2,-11;-2,1,2,-3")]
        [InlineData(0, 1, 3, 3, "1,-1,8;-1,2,-11;1,2,-3")]
        [InlineData(1, 0, 2, 4, "-3,-1,2,-11;-2,1,2,-3")]
        [InlineData(1, 1, 2, 3, "-1,2,-11;1,2,-3")]
        public void Submatrix(int top, int left, int height, int width, string expected)
        {
            var a = new Matrix(new double[] { 2, 1, -1, 8, -3, -1, 2, -11, -2, 1, 2, -3 }, 3, 4, true);
            Assert.Equal(expected, a.Submatrix(top, left, height, width).ToString());
        }

        [Fact]
        public void ReducedRowEchelon()
        {
            var a = new Matrix(new double[] { 2, 1, -1, -3, -1, 2, -2, 1, 2 }, 3, 3, true);
            var b = new Matrix(new double[] { 8, -11, -3 }, 3, 1, true);
            if (!a.TryAugment(b, out Matrix c))
            {
                Assert.False(true);
            }
            c.ReducedRowEchelon();
            Assert.Equal("1,0,0,2;0,1,0,3;0,0,1,-1", c.ToString());
        }

        [Theory]
        [InlineData(2, 3, true, "1,2,3;4,5,6")]
        [InlineData(2, 3, false, "1,3,5;2,4,6")]
        [InlineData(3, 2, true, "1,2;3,4;5,6")]
        [InlineData(3, 2, false, "1,4;2,5;3,6")]
        public void Ctor_ByRow(int rows, int columns, bool isByRow, string expected)
        {
            var m = new Matrix(new double[] { 1, 2, 3, 4, 5, 6 }, rows, columns, isByRow);
            Assert.Equal(expected, m.ToString());
        }

        /// <summary>
        /// A brief attempt at trying to calculate eigenvalues and eigenvectors.
        /// Problems encountered: cannot solve equation using inverse when determinant is zero
        /// Matrix class not expressive enough to calculate lambda (it must be given).
        /// </summary>
        [Theory]
        [InlineData(8d, "3;1")]
        [InlineData(-2d, "3;-9")]
        public void Eigenvectors(double λ, string expected)
        {
            var a = new Matrix(new[] { 7d, 3d, 3d, -1d }, 2, 2, true);
            var λi = Matrix.Identity(2) * λ;
            var diff = a - λi;
            var det = diff.Determinant;
            Assert.Equal(0d, det);

            var x = new Matrix(new[] { 0d, 0d }, 2, 1, true);
            if (!LinearEquation.TrySolve(diff, x, out Matrix solution))
            {
                Assert.False(true);
            }

            Assert.Equal(expected, solution.ToString());

            Assert.Equal((λ * solution).ToString(), (a * solution).ToString()); 
        }

        [Theory]
        [InlineData(7, 3, 3, -1, "Re=-2,Im=0;Re=8,Im=0")]
        [InlineData(3, 1, 0, 2, "Re=2,Im=0;Re=3,Im=0")]
        [InlineData(2, 2, 1, 3, "Re=1,Im=0;Re=4,Im=0")]
        [InlineData(2, 0, 0, 2, "Re=2,Im=0")]
        [InlineData(1, 1, 0, 1, "Re=1,Im=0")]
        [InlineData(0.5, -1, -1, 0.5, "Re=-0.5,Im=0;Re=1.5,Im=0")]
        [InlineData(0, -1, 1, 0, "Re=0,Im=-1;Re=0,Im=1")]
        public void Eigenvalues_2D(double a, double b, double c, double d, string expected)
        {
            var m = new Matrix(new[] { a, b, c, d }, 2, 2, true);

            var eigenvalues = m.Eigenvalues
                .Distinct()
                .OrderBy(x => x.Re)
                .ThenBy(x => x.Im)
                .ToList();

            var actual = string.Join(";", eigenvalues
                .Select(x => $"Re={x.Re},Im={x.Im}"));

            Assert.Equal(expected, actual);

            if (!eigenvalues.Any(x => x.Im != 0))
            {
                foreach (var eigenvalue in eigenvalues)
                {
                    var lambdaIdentity = Matrix.Diagonal(Enumerable.Repeat(eigenvalue.Re, 2).ToArray());
                    Assert.Equal(0, (m - lambdaIdentity).Determinant);
                }
            }
        }
    }
}

