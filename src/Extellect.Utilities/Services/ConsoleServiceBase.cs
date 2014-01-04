using System;
using System.ServiceProcess;
using System.ComponentModel;

namespace Extellect.Utilities.Services
{
    /// <summary>
    /// Adapts a Windows service into a regular console application for easier debugging.
    /// To use this class, change the following project properties:
    /// 1) On the Application tab set Output Type to Console Application
    /// 2) On the Debug tab set the Start Options -> Command Line Arguments to "console"
    /// 3) Replace the standard boilerplate entry point code to 
    ///    a) use the Main(string[] args) method signature, and 
    ///    b) use this class's Run method passing in the args
    /// </summary>
    public class ConsoleServiceBase : Component
    {
        /// <summary>
        /// Event raised by Windows services to request additional time to complete an operation.
        /// </summary>
        public event EventHandler<AdditionalTimeEventArgs> AdditionalTimeRequested;

        /// <summary>
        /// Creates a new instance of this class.
        /// </summary>
        public ConsoleServiceBase()
        {
            AutoLog = true;
            CanStop = true;
        }

        #region Controller
        /// <summary>
        /// Starts the specified service with arguments passed in from the command line.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="args"></param>
        public static void Start(ConsoleServiceBase service, string[] args)
        {
            if (Environment.UserInteractive && args.Length > 0 && args[0].Equals("console", StringComparison.OrdinalIgnoreCase))
            {
                var destinationArray = new string[args.Length - 1];
                Array.Copy(args, 1, destinationArray, 0, args.Length - 1);
                Debug(service, destinationArray);
            }
            else
            {
                Release(service, args);
            }
        }

        /// <summary>
        /// Starts the specified service in Debug (or Console) mode.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="args"></param>
        private static void Debug(ConsoleServiceBase service, string[] args)
        {
            service.AdditionalTimeRequested += (s, e) => { Console.WriteLine("{0} milliseconds of extra time requested", e.Milliseconds); };
            Console.WriteLine("Starting service...");
            service.OnStart(args);
            Console.WriteLine("Service started. Press enter to stop...");
            Console.ReadLine();
            Console.WriteLine("Stopping service...");
            service.OnStop();
            Console.WriteLine("Service is stopped.");
        }

        /// <summary>
        /// Starts the specified service in Release (or Windows Service) mode.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="args"></param>
        private static void Release(ConsoleServiceBase service, string[] args)
        {
            ServiceBase.Run(
                new ServiceBase[]
                    {
                        new WindowsServiceAdaptor(service),
                    });
        }
        #endregion

        #region Implementation of members from System.ServiceProcess.ServiceBase
        public const int MaxNameLength = 80;

        public string ServiceName { get; set; }

        public bool AutoLog { get; set; }
        public bool CanHandlePowerEvent { get; set; }
        public bool CanHandleSessionChangeEvent { get; set; }
        public bool CanPauseAndContinue { get; set; }
        public bool CanShutdown { get; set; }
        public bool CanStop { get; set; }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        protected virtual void OnContinue() { }

        protected virtual void OnCustomCommand(int command) { }

        protected virtual void OnPause() { }

        protected virtual bool OnPowerEvent(PowerBroadcastStatus powerStatus) { return true; }

        protected virtual void OnSessionChange(SessionChangeDescription changeDescription) { }

        protected virtual void OnShutdown() { }

        protected virtual void OnStart(string[] args) { }

        protected virtual void OnStop() { }

        public void RequestAdditionalTime(int milliseconds)
        {
            var additionalTimeRequested = AdditionalTimeRequested;
            if (additionalTimeRequested != null)
            {
                additionalTimeRequested(this, new AdditionalTimeEventArgs(milliseconds));
            }
        }
        #endregion

        /// <summary>
        /// Adapts an underlying ConsoleServiceBase instance to the standard ServiceBase class as used by Windows services.
        /// </summary>
        private class WindowsServiceAdaptor : ServiceBase
        {
            private readonly ConsoleServiceBase underlying;

            public WindowsServiceAdaptor(ConsoleServiceBase underlying)
            {
                if (underlying == null)
                {
                    throw new ArgumentNullException("underlying", "The underlying service implementation cannot be null.");
                }
                this.underlying = underlying;
                underlying.AdditionalTimeRequested += Service_AdditionalTimeRequested;
            }

            protected override void Dispose(bool disposing)
            {
                underlying.AdditionalTimeRequested -= Service_AdditionalTimeRequested;
                base.Dispose(disposing);
            }

            private void Service_AdditionalTimeRequested(object sender, AdditionalTimeEventArgs args)
            {
                RequestAdditionalTime(args.Milliseconds);
            }

            protected override void OnContinue()
            {
                underlying.OnContinue();
            }

            protected override void OnCustomCommand(int command)
            {
                underlying.OnCustomCommand(command);
            }

            protected override void OnPause()
            {
                underlying.OnPause();
            }

            protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus) 
            {
                return underlying.OnPowerEvent(powerStatus); 
            }

            protected override void OnSessionChange(SessionChangeDescription changeDescription) 
            {
                underlying.OnSessionChange(changeDescription);
            }

            protected override void OnShutdown() 
            {
                underlying.OnShutdown();
            }

            protected override void OnStart(string[] args)
            {
                underlying.OnStart(args);
            }

            protected override void OnStop()
            {
                underlying.OnStop();
            }
        }
    }
}
