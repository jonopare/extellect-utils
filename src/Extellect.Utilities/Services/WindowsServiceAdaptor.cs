using System;
using System.ServiceProcess;

namespace Extellect.Utilities.Services
{
    /// <summary>
    /// Adapts an underlying IService instance to the standard ServiceBase class as used by Windows services.
    /// </summary>
    public class WindowsServiceAdaptor : ServiceBase
    {
        private readonly IService underlying;

        public WindowsServiceAdaptor(IService underlying)
        {
            if (underlying == null)
            {
                throw new ArgumentNullException("underlying", "The underlying service implementation cannot be null.");
            }
            this.underlying = underlying;
        }

        protected override void OnStart(string[] args)
        {
            underlying.Start(args);
        }

        protected override void OnStop()
        {
            underlying.Stop();
        }
    }
}
