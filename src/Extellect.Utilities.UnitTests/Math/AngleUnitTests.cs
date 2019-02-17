using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Extellect.Math
{
    public class AngleUnitTests
    {
        [Fact]
        public void TwoPi_ReadonlyStatic_HasCorrectValue()
        {
            Assert.Equal(2 * System.Math.PI, Angle.TwoPi.Radians, 6);
        }

        [Fact]
        public void PiOverTwo_ReadonlyStatic_HasCorrectValue()
        {
            Assert.Equal(System.Math.PI / 2, Angle.PiOverTwo.Radians, 6);
        }

        [Fact]
        public void PiOverFour_ReadonlyStatic_HasCorrectValue()
        {
            Assert.Equal(System.Math.PI / 4, Angle.PiOverFour.Radians, 6);
        }

        [Fact]
        public void Zero_ReadonlyStatic_HasCorrectValue()
        {
            Assert.Equal(0, Angle.Zero.Radians, 6);
        }

        [Fact]
        public void Constrain_BelowZero_WithinBoundary()
        {
            Assert.Equal(359, Angle.FromDegrees(-1).Constrain().Degrees, 6);
        }

        [Fact]
        public void Constrain_EqualsOne_WithinBoundary()
        {
            Assert.Equal(0, Angle.FromDegrees(360).Constrain().Degrees, 6);
        }

        [Fact]
        public void Constrain_AboveOne_WithinBoundary()
        {
            Assert.Equal(1, Angle.FromDegrees(361).Constrain().Degrees, 6);
        }

        [Fact]
        public void Constrain_NegativeOneQuarter_IsPositiveThreeQuarters()
        {
            var original = Angle.FromFraction(-1, 4);
            var actual = original.Constrain();
            var expected = Angle.FromFraction(3, 4);

            Assert.Equal(expected.Degrees, actual.Degrees);
        }

        [Fact]
        public void Constrain_NegativeFiveQuarters_IsPositiveThreeQuarters()
        {
            var original = Angle.FromFraction(-5, 4);
            var actual = original.Constrain();
            var expected = Angle.FromFraction(3, 4);

            Assert.Equal(expected.Degrees, actual.Degrees);
        }

        [Fact]
        public void Constrain_PositiveOneQuarter_IsPositiveOneQuarter()
        {
            var original = Angle.FromFraction(1, 4);
            var actual = original.Constrain();
            var expected = Angle.FromFraction(1, 4);

            Assert.Equal(expected.Degrees, actual.Degrees);
        }

        [Fact]
        public void Constrain_PositiveFiveQuarters_IsPositiveOneQuarter()
        {
            var original = Angle.FromFraction(5, 4);
            var actual = original.Constrain();
            var expected = Angle.FromFraction(1, 4);

            Assert.Equal(expected.Degrees, actual.Degrees);
        }

        [Fact]
        public void Constrain_ZeroQuarters_IsZeroQuarter()
        {
            var original = Angle.FromFraction(0, 4);
            var actual = original.Constrain();
            var expected = Angle.FromFraction(0, 4);

            Assert.Equal(expected.Degrees, actual.Degrees);
        }

        [Fact]
        public void Constrain_PositiveFourQuarters_IsZeroQuarter()
        {
            var original = Angle.FromFraction(4, 4);
            var actual = original.Constrain();
            var expected = Angle.FromFraction(0, 4);

            Assert.Equal(expected.Degrees, actual.Degrees);
        }

        [Fact]
        public void Degrees_SixQuadrants_InputEqualsOutput()
        {
            for (int q = -1; q < 5; q++)
            {
                var degrees = q * 90.0 + 45.0;
                Assert.Equal(degrees, Angle.FromDegrees(degrees).Degrees, 6);
            }
        }

        [Fact]
        public void Fraction_SixQuadrants_InputEqualsOutput()
        {
            for (int q = -1; q < 5; q++)
            {
                var fraction = q / 4.0 + 0.125;
                Assert.Equal(fraction, Angle.FromFraction(fraction).Fraction, 6);
            }
        }

        [Fact]
        public void Fraction2_SixQuadrants_InputEqualsOutput()
        {
            for (int q = -1; q < 5; q++)
            {
                var numerator = q + 0.5;
                var denominator = 4.0;
                var fraction = numerator / denominator;
                Assert.Equal(fraction, Angle.FromFraction(numerator, denominator).Fraction, 6);
            }
        }

        [Fact]
        public void Hours_SixQuadrants_InputEqualsOutput()
        {
            for (int q = -1; q < 5; q++)
            {
                var hours = q * 24 + 8.0;
                Assert.Equal(hours, Angle.FromHours(hours).Hours, 6);
            }
        }

        [Fact]
        public void Radians_SixQuadrants_InputEqualsOutput()
        {
            for (int q = -1; q < 5; q++)
            {
                var radians = q * Angle.PiOverTwo.Radians + Angle.PiOverFour.Radians;
                Assert.Equal(radians, Angle.FromRadians(radians).Radians, 6);
            }
        }

        [Fact]
        public void Add_PositivePositive_BiggerAngle()
        {
            Assert.Equal(3, (Angle.FromRadians(2) + Angle.FromRadians(1)).Radians, 6);
        }

        [Fact]
        public void Add_PositiveNegative_SmallerAngle()
        {
            Assert.Equal(1, (Angle.FromRadians(2) + Angle.FromRadians(-1)).Radians, 6);
        }

        [Fact]
        public void Subtract_PositivePositive_SmallerAngle()
        {
            Assert.Equal(1, (Angle.FromRadians(2) - Angle.FromRadians(1)).Radians, 6);
        }

        [Fact]
        public void Add_PositiveNegative_BiggerAngle()
        {
            Assert.Equal(3, (Angle.FromRadians(2) - Angle.FromRadians(-1)).Radians, 6);
        }

        [Fact]
        public void Ctor_NegativeHour_NegativeHourMinuteSecond()
        {
            var expected = Angle.FromHours(-1.23);
            var actual = Angle.FromHours(-1, 13, 48);
            Assert.Equal(0, (actual - expected).Radians, 6);
        }

        [Fact]
        public void Ctor_NegativeDegree_NegativeDegreeMinuteSecond()
        {
            var expected = Angle.FromDegrees(-1.23);
            var actual = Angle.FromDegrees(-1, 13, 48);
            Assert.Equal(0, (actual - expected).Radians, 6);
        }

        [Fact]
        public void Ctor_ZeroDegree_NegativeMinuteSecond()
        {
            var expected = Angle.FromDegrees(-0.23);
            var actual = Angle.FromDegrees(0, -13, 48);
            Assert.Equal(0, (actual - expected).Radians, 6);
        }

        [Fact]
        public void Ctor_ZeroDegreeMinute_NegativeSecond()
        {
            var expected = Angle.FromDegrees(-0.01333);
            var actual = Angle.FromDegrees(0, 0, -48);
            Assert.Equal(0, (actual - expected).Radians, 6);
        }
    }
}
