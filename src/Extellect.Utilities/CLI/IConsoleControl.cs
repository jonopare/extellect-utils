#if NET45
using System;

namespace Extellect.CLI
{
    /// <summary>
    /// 
    /// </summary>
    public interface IConsoleControl
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler<ControlEventArgs> ControlEvent;
    }
}
#endif