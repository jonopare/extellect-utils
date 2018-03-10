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
        public void TrySolve_2()
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
            
            Matrix result;
            if (!LinearEquation.TrySolve(abcd, ef, out result))
            {
                Assert.True(false);
            }

            var expected = new Matrix(
                new double[,] { 
                    { 1 }, 
                    { 2 }
                });

            Assert.Equal(expected, result);
        }

        [Fact]
        public void TrySolve_3()
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

            Matrix result;
            if (!LinearEquation.TrySolve(abcd, ef, out result))
            {
                Assert.True(false);
            }

            var expected = new Matrix(
                new double[,] { 
                    { 1 }, 
                    { 2 },
                    { 3 }
                });

            Assert.Equal(expected, result);
        }
    }
}
