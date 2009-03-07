using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Extellect.Utilities.Remoting
{
    public class Streamable<T> : MarshalByRefObject, IEnumerable<T>
    {
        private class Enumerator : MarshalByRefObject, IEnumerator<T>
        {
            private readonly IEnumerator<T> enumerator;

            public Enumerator(IEnumerator<T> enumerator)
            {
                this.enumerator = enumerator;
            }

            public T Current
            {
                get { return enumerator.Current; }
            }

            public bool MoveNext()
            {
                return enumerator.MoveNext();
            }

            public void Reset()
            {
                enumerator.Reset();
            }

            public void Dispose()
            {
                enumerator.Dispose();
            }

            object System.Collections.IEnumerator.Current
            {
                get { return ((IEnumerator)enumerator).Current; }
            }
        }

        private readonly IEnumerable<T> enumerable;

        public Streamable(IEnumerable<T> enumerable)
        {
            this.enumerable = enumerable;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(enumerable.GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(enumerable.GetEnumerator());
        }
    }
}
