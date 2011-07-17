using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Extellect.Utilities.Remoting
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Streamable<T> : MarshalByRefObject, IEnumerable<T>
    {
        /// <summary>
        /// 
        /// </summary>
        private class Enumerator : MarshalByRefObject, IEnumerator<T>
        {
            private readonly IEnumerator<T> enumerator;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="enumerator"></param>
            public Enumerator(IEnumerator<T> enumerator)
            {
                this.enumerator = enumerator;
            }

            /// <summary>
            /// 
            /// </summary>
            public T Current
            {
                get { return enumerator.Current; }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public bool MoveNext()
            {
                return enumerator.MoveNext();
            }

            /// <summary>
            /// 
            /// </summary>
            public void Reset()
            {
                enumerator.Reset();
            }

            /// <summary>
            /// 
            /// </summary>
            public void Dispose()
            {
                enumerator.Dispose();
            }

            /// <summary>
            /// 
            /// </summary>
            object System.Collections.IEnumerator.Current
            {
                get { return ((IEnumerator)enumerator).Current; }
            }
        }

        private readonly IEnumerable<T> enumerable;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumerable"></param>
        public Streamable(IEnumerable<T> enumerable)
        {
            this.enumerable = enumerable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(enumerable.GetEnumerator());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(enumerable.GetEnumerator());
        }
    }
}
