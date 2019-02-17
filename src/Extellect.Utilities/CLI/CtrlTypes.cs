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
    internal enum CtrlTypes
    {
        /// <summary>
        /// 
        /// </summary>
        CTRL_C_EVENT = 0,
        /// <summary>
        /// 
        /// </summary>
        CTRL_BREAK_EVENT = 1,
        /// <summary>
        /// 
        /// </summary>
        CTRL_CLOSE_EVENT = 2,
        /// <summary>
        /// 
        /// </summary>
        CTRL_LOGOFF_EVENT = 5,
        /// <summary>
        /// 
        /// </summary>
        CTRL_SHUTDOWN_EVENT = 6
    }
}
#endif