#pragma warning disable 1591
using System;
using System.Diagnostics;

namespace Extellect.Utilities.Instrumentation
{
    public class FailSafePerformanceCounterFactory : IPerformanceCounterFactory
    {
        public IPerformanceCounter Create()
        {
            IPerformanceCounter result;
            try
            {
                PerformanceCounter counter = new PerformanceCounter();
                result = new FailSafePerformanceCounter(counter);
            }
            catch (InvalidOperationException exception)
            {
                result = new FailSafePerformanceCounter(exception);
            }
            return result;
        }

        public IPerformanceCounter Create(string categoryName, string counterName)
        {
            IPerformanceCounter result;
            try
            {
                PerformanceCounter counter = new PerformanceCounter(categoryName, counterName);
                result = new FailSafePerformanceCounter(counter);
            }
            catch (InvalidOperationException exception)
            {
                result = new FailSafePerformanceCounter(exception);
            }
            return result;
        }

        public IPerformanceCounter Create(string categoryName, string counterName, bool readOnly)
        {
            IPerformanceCounter result;
            try
            {
                PerformanceCounter counter = new PerformanceCounter(categoryName, counterName, readOnly);
                result = new FailSafePerformanceCounter(counter);
            }
            catch (InvalidOperationException exception)
            {
                result = new FailSafePerformanceCounter(exception);
            }
            return result;
        }

        public IPerformanceCounter Create(string categoryName, string counterName, string instanceName)
        {
            IPerformanceCounter result;
            try
            {
                PerformanceCounter counter = new PerformanceCounter(categoryName, counterName, instanceName);
                result = new FailSafePerformanceCounter(counter);
            }
            catch (InvalidOperationException exception)
            {
                result = new FailSafePerformanceCounter(exception);
            }
            return result;
        }

        public IPerformanceCounter Create(string categoryName, string counterName, string instanceName, bool readOnly)
        {
            IPerformanceCounter result;
            try
            {
                PerformanceCounter counter = new PerformanceCounter(categoryName, counterName, instanceName, readOnly);
                result = new FailSafePerformanceCounter(counter);
            }
            catch (InvalidOperationException exception)
            {
                result = new FailSafePerformanceCounter(exception);
            }
            return result;
        }

        public IPerformanceCounter Create(string categoryName, string counterName, string instanceName, string machineName)
        {
            IPerformanceCounter result;
            try
            {
                PerformanceCounter counter = new PerformanceCounter(categoryName, counterName, instanceName, machineName);
                result = new FailSafePerformanceCounter(counter);
            }
            catch (InvalidOperationException exception)
            {
                result = new FailSafePerformanceCounter(exception);
            }
            return result;
        }
    }
}
