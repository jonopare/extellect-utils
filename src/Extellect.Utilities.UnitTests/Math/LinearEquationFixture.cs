using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Extellect.Utilities.Math
{
    public class LinearEquationFixture
    {
        [Fact]
        public void TrySolve_2x2_2x1()
        {
            var abcd = new Matrix(
                new double[,] { 
                    { 2, 3 }, 
                    { 1, -2 } 
                });

            var ef = new Matrix(
                new double[,] { 
                    { 8 }, 
                    { -3 } 
                });

            if (!LinearEquation.TrySolve(abcd, ef, out Matrix result))
            {
                Assert.False(true);
            }

            var expected = new Matrix(
                new double[,] { 
                    { 1 }, 
                    { 2 }
                });

            Assert.Equal(expected, result);
        }

        [Fact]
        public void TrySolve_3x3_3x1()
        {
            var abcd = new Matrix(
                new double[,] { 
                    { 1, 1, 1 }, 
                    { 1, -1, 1 },
                    { 1, 2, -1 }
                });

            var ef = new Matrix(
                new double[,] { 
                    { 6 }, 
                    { 2 },
                    { 2 }
                });

            if (!LinearEquation.TrySolve(abcd, ef, out Matrix result))
            {
                Assert.False(true);
            }

            var expected = new Matrix(
                new double[,] { 
                    { 1 }, 
                    { 2 },
                    { 3 }
                });

            Assert.Equal(expected, result);
        }

        [Fact]
        public void TrySolve_3x3_3x2()
        {
            var abcd = new Matrix(
                new double[,] {
                    { 1, 1, 1 },
                    { 1, -1, 1 },
                    { 1, 2, -1 }
                });

            var ef = new Matrix(
                new double[,] {
                    { 6, 1 },
                    { 2, 1 },
                    { 2, 1 }
                });

            if (!LinearEquation.TrySolve(abcd, ef, out Matrix result))
            {
                Assert.False(true);
            }

            var expected = new Matrix(
                new double[,] {
                    { 1, 1 },
                    { 2, 0 },
                    { 3, 0 }
                });

            Assert.Equal(expected, result);

            Assert.Equal(abcd * expected, ef);
        }

        [Fact]
        public void TrySolve_3x3_3x1_LinearlyDependent()
        {
            var abcd = new Matrix(
                new double[,] {
                    { 1, 0, -1 },
                    { 1, 1, 2 },
                    { 3, 1, 0 } // <-- R3 is equal to 2xR1 + 1xR2
                });

            Assert.Equal(0, abcd.Determinant);

            var ef = new Matrix(
                new double[,] {
                    { 1 },
                    { 1 },
                    { 1 }
                });

            if (LinearEquation.TrySolve(abcd, ef, out Matrix result))
            {
                Assert.False(true);
            }
        }
    }
}
