#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extellect.Sequencing
{
    public class ArithmeticSequence : ISequenceGenerator<int>
    {
        private int from;
        private int to;

        public ArithmeticSequence(int from, int to)
        {
            this.from = from;
            this.to = to;
        }

        public IEnumerable<int> Generate()
        {
            if (from <= to)
                for (int index = from; index <= to; index++)
                    yield return index;
            else
                for (int index = from; index >= to; index--)
                    yield return index;
        }
    }

}
