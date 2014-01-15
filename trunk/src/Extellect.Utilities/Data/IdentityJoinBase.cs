using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extellect.Utilities.Data
{
    /// <summary>
    /// This class provides functionality to iterate over two ordered sequences and
    /// determine the common elements as well as the elements that exist only 
    /// in either one of the sequences. It takes advantage of the sorted data to
    /// perform the check in a single pass. The operation is conceptually similar
    /// to a full outer join with the primary exception being that an element can only
    /// appear once in the output (in a full outer join, all elements in set B that 
    /// match the given element from set A will be joined). 
    /// Developers should subclass this class and provide override implementations
    /// for the LeftOnly, RightOnly and Both template methods, which are invoked during
    /// a call to the public method Reconcile.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public abstract class IdentityJoinBase<TKey, TValue> where TKey : IComparable<TKey>
    {
        private readonly Func<TValue, TKey> keySelector;

        public IdentityJoinBase(Func<TValue, TKey> keySelector)
        {
            this.keySelector = keySelector;
        }

        /// <summary>
        /// Steps through the items in two ordered sequences and compares the values in each.
        /// If an item appears only in the left sequence then LeftOnly is invoked and the left sequence is advanced.
        /// If an item appears in both sequences then Both is invoked and both sequences are advanced.
        /// If an item appears only in the right sequence then RightOnly is invoked and the right sequence is advanced.
        /// </summary>
        /// <param name="lefts"></param>
        /// <param name="rights"></param>
        public void Reconcile(IEnumerable<TValue> lefts, IEnumerable<TValue> rights)
        {
            var le = lefts.GetEnumerator();
            var re = rights.GetEnumerator();

            var la = le.MoveNext();  // left advance
            var ra = re.MoveNext();  // right advance

            TKey rk;
            TKey lk;
            while (la || ra)
            {
                int cmp;
                if (la)
                {
                    lk = keySelector(le.Current);
                    if (ra)
                    {
                        rk = keySelector(re.Current);
                        cmp = lk.CompareTo(rk);
                    }
                    else
                    {
                        cmp = -1;
                    }
                }
                else
                {
                    rk = keySelector(re.Current);
                    cmp = 1;
                }

                if (cmp < 0)
                {
                    LeftOnly(le.Current);
                    la = le.MoveNext();
                }
                else if (cmp == 0)
                {
                    Both(le.Current, re.Current);
                    la = le.MoveNext();
                    ra = re.MoveNext();
                }
                else
                {
                    RightOnly(re.Current);
                    ra = re.MoveNext();
                }
            }
        }

        protected abstract void LeftOnly(TValue left);
        protected abstract void Both(TValue left, TValue right);
        protected abstract void RightOnly(TValue right);
    }
}
