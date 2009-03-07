using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extellect.Utilities.Sequencing
{
    public class GenericSequenceGenerator<T> : ISequenceGenerator<T> where T : IComparable<T>
    {
        private readonly T from;
        private readonly T to;
        private readonly NextInSequence<T> next;

        public GenericSequenceGenerator(T from, T to, NextInSequence<T> next)
        {
            this.from = from;
            this.to = to;
            this.next = next;
        }

        #region ISequenceGenerator<T> Members

        public IEnumerable<T> Generate()
        {
            T current = from;
            while (current.CompareTo(to) <= 0)
            {
                yield return current;
                current = next(current);
            }
        }

        #endregion
    }
}
