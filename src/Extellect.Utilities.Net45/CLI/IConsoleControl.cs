using System;

namespace Extellect.Utilities.CLI
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
