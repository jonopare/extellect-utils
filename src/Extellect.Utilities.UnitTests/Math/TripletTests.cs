using System;
using Xunit;

namespace Extellect.Math
{
    public class TripletTests
    {
        #region Length
        [Fact]
        public void Length_Origin_IsZero()
        {
            var t = new Triplet(0, 0, 0);
            Assert.Equal(0, t.Length);
        }

        [Fact]
        public void Length_UnitX_IsOne()
        {
            var t = new Triplet(1, 0, 0);
            Assert.Equal(1, t.Length);
        }

        [Fact]
        public void Length_UnitY_IsOne()
        {
            var t = new Triplet(0, 1, 0);
            Assert.Equal(1, t.Length);
        }

        [Fact]
        public void Length_UnitZ_IsOne()
        {
            var t = new Triplet(0, 0, 1);
            Assert.Equal(1, t.Length);
        }

        [Fact]
        public void Length_UnitXY_IsRoot2()
        {
            var t = new Triplet(1, 1, 0);
            Assert.Equal(System.Math.Sqrt(2), t.Length);
        }

        [Fact]
        public void Length_UnitXZ_IsRoot2()
        {
            var t = new Triplet(1, 0, 1);
            Assert.Equal(System.Math.Sqrt(2), t.Length);
        }

        [Fact]
        public void Length_UnitYZ_IsRoot2()
        {
            var t = new Triplet(0, 1, 1);
            Assert.Equal(System.Math.Sqrt(2), t.Length);
        }

        [Fact]
        public void Length_UnitXYZ_IsOnePointSevenThree()
        {
            var t = new Triplet(1, 1, 1);
            Assert.Equal(1.7320508075688772, t.Length, 15);
        }
        #endregion Length

        #region Unit
        [Fact]
        public void Unit_RandomXYZ_LengthIsOne()
        {
            var t = new Triplet(200, 2, 20).Unit;
            Assert.Equal(1d, t.Length, 15);
        }
        #endregion

        #region Add
        [Fact]
        public void Add_UnitXToUnitY_IsUnitXY()
        {
            var t = new Triplet(1, 0, 0) + new Triplet(0, 1, 0);
            Assert.Equal(new Triplet(1, 1, 0), t);
        }

        [Fact]
        public void Add_Umm_IsOrigin()
        {
            var t = new Triplet(1, 10, 100) + new Triplet(-1, -10, -100);
            Assert.Equal(new Triplet(0, 0, 0), t);
        }

        #endregion

        #region Subtract
        [Fact]
        public void Subtract_UnitYFromUnitX_IsUmm()
        {
            var t = new Triplet(1, 0, 0) - new Triplet(0, 1, 0);
            Assert.Equal(new Triplet(1, -1, 0), t);
        }

        [Fact]
        public void Subtract_Umm_IsOrigin()
        {
            var t = new Triplet(1, 10, 100) - new Triplet(1, 10, 100);
            Assert.Equal(new Triplet(0, 0, 0), t);
        }
        #endregion

        #region Dot
        [Fact]
        public void Dot_()
        {
            var a = new Triplet(1, 2, 3);
            var b = new Triplet(10, 100, 1000);
            Assert.Equal(3210, a.Dot(b));
        }
        #endregion

        #region Cross
        [Fact]
        public void Cross_()
        {
            var a = new Triplet(1, 2, 3);
            var b = new Triplet(10, 100, 1000);
            Assert.Equal(new Triplet(1700, -970, 80), a.Cross(b), new TripletComparer(15));
        }
        #endregion
    }
}
