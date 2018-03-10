using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extellect.Utilities.Collections;
using Xunit;

namespace Extellect.Utilities
{
    
    public class PermutationTests
    {
        #region Test support classes
        private class Inner
        {
            private double d;
            public Inner(double d)
            {
                this.d = d;
            }
            public double D
            {
                get { return d; }
            }
        }

        private class InnerComparer : IComparer<Inner>, IEqualityComparer<Inner>
        {
            public int Compare(Inner x, Inner y)
            {
                return x.D.CompareTo(y.D);
            }
            
            public bool Equals(Inner x, Inner y)
            {
                return x.D == y.D;
            }

            public int GetHashCode(Inner obj)
            {
                return obj.D.GetHashCode();
            }
        }
        #endregion

        [Fact]
        public void Object_Abc_NonComparableWithExplicitComparer()
        {
            InnerComparer comparer = new InnerComparer();
            Inner[] xs = new Inner[] { new Inner(6.5), new Inner(4.3), new Inner(2.1) };
            Assert.True(
                xs.Permute(comparer).DeepSequenceEqual(
                    new Inner[][] {
                        new Inner[] { new Inner(2.1), new Inner(4.3), new Inner(6.5) },
                        new Inner[] { new Inner(2.1), new Inner(6.5), new Inner(4.3) },
                        new Inner[] { new Inner(4.3), new Inner(2.1), new Inner(6.5) },
                        new Inner[] { new Inner(4.3), new Inner(6.5), new Inner(2.1) },
                        new Inner[] { new Inner(6.5), new Inner(2.1), new Inner(4.3) },
                        new Inner[] { new Inner(6.5), new Inner(4.3), new Inner(2.1) },
                    },
                    comparer
                ));
        }

        [Fact]
        public void Letter_Abc()
        {
            Assert.True(
                "abc".Permute().SequenceEqual(
                    new string[] {
                        "abc",
                        "acb",
                        "bac",
                        "bca",
                        "cab",
                        "cba"
                    }
            ));
        }

        [Fact]
        public void Letter_Aaa()
        {
            Assert.True(
                "aaa".Permute().SequenceEqual(
                    new string[] {
                        "aaa",
                    }
            ));
        }

        [Fact]
        public void Letter_Aba()
        {
            Assert.True(
                "aba".Permute().SequenceEqual(
                    new string[] {
                        "aab",
                        "aba",
                        "baa",
                    }
            ));
        }

        [Fact]
        public void Number_Abc()
        {
            Assert.True(
                new int[] { 1, 2, 3 }.Permute().DeepSequenceEqual(
                    new int[][] {
                        new int[] {1, 2, 3},
                        new int[] {1, 3, 2},
                        new int[] {2, 1, 3},
                        new int[] {2, 3, 1},
                        new int[] {3, 1, 2},
                        new int[] {3, 2, 1},
                    }
            ));
        }

        [Fact]
        public void Number_Aaa()
        {
            Assert.True(
                new int[] { 1, 1, 1 }.Permute().DeepSequenceEqual(
                    new int[][] {
                        new int[] {1, 1, 1},
                    }
            ));
        }       

        [Fact]
        public void Number_Aba()
        {
            Assert.True(
                new int[] { 1, 2, 1 }.Permute().DeepSequenceEqual(
                    new int[][] {
                        new int[] {1, 1, 2},
                        new int[] {1, 2, 1},
                        new int[] {2, 1, 1},
                    }
            ));
        }        
    }
}
