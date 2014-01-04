using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extellect.Utilities.Services
{
    /// <summary>
    /// Arguments for event raised by Windows services requesting additional time to perform operations
    /// </summary>
    public class AdditionalTimeEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the number of additional milliseconds requested
        /// </summary>
        public int Milliseconds { get; private set; }

        /// <summary>
        /// Creates a new instance of the class.
        /// </summary>
        /// <param name="milliseconds"></param>
        public AdditionalTimeEventArgs(int milliseconds)
        {
            Milliseconds = milliseconds;
        }
    }
}
