using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extellect
{
    /// <summary>
    /// 
    /// </summary>
    public class EventArgs<T> : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public T Item { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public EventArgs(T item)
        {
            Item = item;
        }
    }
}
