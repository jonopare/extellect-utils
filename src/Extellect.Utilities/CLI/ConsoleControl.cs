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
    public class ConsoleControl : IConsoleControl
    {
        private EventHandler<ControlEventArgs> _controlEvent;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<ControlEventArgs> ControlEvent
        {
            add
            {
                if (_controlEvent == null)
                    NativeMethods.SetConsoleCtrlHandler(ConsoleEventCallback, true);
                _controlEvent += value;                
            }
            remove
            {
                _controlEvent -= value;
                if (_controlEvent == null)
                    NativeMethods.SetConsoleCtrlHandler(ConsoleEventCallback, false);
            }
        }

        void OnControlEvent(string message)
        {
            var controlEvent = _controlEvent;
            if (controlEvent != null)
            {
                controlEvent(this, new ControlEventArgs(message));
            }
        }

        bool ConsoleEventCallback(int eventType)
        {
            var ctrlEventType = (CtrlTypes)eventType;
            switch (ctrlEventType)
            {
                case CtrlTypes.CTRL_C_EVENT:
                case CtrlTypes.CTRL_BREAK_EVENT:
                case CtrlTypes.CTRL_CLOSE_EVENT:
                case CtrlTypes.CTRL_LOGOFF_EVENT:
                case CtrlTypes.CTRL_SHUTDOWN_EVENT:
                    OnControlEvent( "Console exiting because of " + ctrlEventType + " event");
                    break;
                default:
                    OnControlEvent("Console exiting because of unknown event type (" + eventType + ")");
                    break;
            }
            return true;
        }
    }
}
#endif