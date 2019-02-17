#if NET45
#pragma warning disable 1591
using System;
using System.Diagnostics;

namespace Extellect.Instrumentation
{
    public interface IPerformanceCounter : IDisposable
    {
        string CategoryName { get; set; }
        string CounterHelp { get; }
        string CounterName { get; set; }
        PerformanceCounterType CounterType { get; }
        PerformanceCounterInstanceLifetime InstanceLifetime { get; set; }
        string InstanceName { get; set; }
        string MachineName { get; set; }
        long RawValue { get; set; }
        bool ReadOnly { get; set; }
        void BeginInit();
        void Close();
        long Decrement();
        void EndInit();
        long Increment();
        long IncrementBy(long value);
        CounterSample NextSample();
        float NextValue();
        void RemoveInstance();
    }
}
#endif