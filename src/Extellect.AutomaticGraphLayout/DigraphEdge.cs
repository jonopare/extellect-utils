using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extellect.AutomaticGraphLayout
{
    public class DigraphEdge<T> : Tuple<T, T>
    {
        public T Source => Item1;
        public T Target => Item2;

        public DigraphEdge(T source, T target)
            : base(source, target)
        {
        }
    }
}
