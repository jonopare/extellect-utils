using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extellect.Utilities.Collections;

namespace Extellect.Utilities.Execution
{
    /// <summary>
    /// An action that will be executed later.
    /// </summary>
    public class QueuedCallbackCommand
    {
        private readonly BlockingQueue<QueuedCallbackCommand> queue;
        private readonly QueuedCallback callback;
        private readonly object state;

        /// <summary>
        /// Constructs a new queued callback command. The parameters represent the method to call, the state
        /// to pass into that method call, and a queue so that additional commands can be built up
        /// for future execution.
        /// </summary>
        public QueuedCallbackCommand(QueuedCallback callback, object state, BlockingQueue<QueuedCallbackCommand> queue)
        {
            this.callback = callback;
            this.state = state;
            this.queue = queue;
        }

        /// <summary>
        /// Invokes this command object's callback delegate, passing in any state and a reference to a queue.
        /// </summary>
        public void Execute()
        {
            callback(state, queue);
        }
    }
}
