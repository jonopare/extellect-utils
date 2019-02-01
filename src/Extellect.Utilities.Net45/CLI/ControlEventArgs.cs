using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extellect.Utilities.CLI
{
    /// <summary>
    /// 
    /// </summary>
    public class ControlEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public ControlEventArgs(string message)
        {
            Message = message;
        }
    }
}
