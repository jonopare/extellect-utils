#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extellect.Sequencing
{
    public class ListComprehension<T> : ISequenceGenerator<T>
    {
        private ISequenceGenerator<T> generator;
        private Predicate<T> predicate;

        public ListComprehension(ISequenceGenerator<T> generator, Predicate<T> predicate)
        {
            this.generator = generator;
            this.predicate = predicate;
        }

        public IEnumerable<T> Generate()
        {
            foreach (T element in generator.Generate())
                if (predicate(element))
                    yield return element;
        }
    }

}
