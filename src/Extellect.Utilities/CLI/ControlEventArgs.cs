#if NET45
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extellect.CLI
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
#endif