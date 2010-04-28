#pragma warning disable 1591
using System;

namespace Extellect.Utilities.Instrumentation
{
    public interface IPerformanceCounterFactory
    {
        IPerformanceCounter Create();
        IPerformanceCounter Create(string categoryName, string counterName);
        IPerformanceCounter Create(string categoryName, string counterName, bool readOnly);
        IPerformanceCounter Create(string categoryName, string counterName, string instanceName);
        IPerformanceCounter Create(string categoryName, string counterName, string instanceName, bool readOnly);
        IPerformanceCounter Create(string categoryName, string counterName, string instanceName, string machineName);
    }
}
