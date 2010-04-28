using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extellect.Utilities.Collections;

namespace Extellect.Utilities.Execution
{
    /// <summary>
    /// This delegate accepts some kind of state, as well as a queue onto which further
    /// callbacks can be queued, via the queued callback command construct.
    /// </summary>
    public delegate void QueuedCallback(object state, BlockingQueue<QueuedCallbackCommand> queue);
}
