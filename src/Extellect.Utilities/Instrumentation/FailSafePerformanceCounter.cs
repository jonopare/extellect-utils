#pragma warning disable 1591
using System;
using System.Diagnostics;

namespace Extellect.Utilities.Instrumentation
{
    public class FailSafePerformanceCounter : IPerformanceCounter
    {
        public static class DefaultValue
        {
            public readonly static string TextValue = "PERFORMANCE_COUNTER_FAILURE";
            public readonly static long LongValue = 0L;
            public readonly static bool ReadOnly = true;
            public readonly static PerformanceCounterType CounterType = PerformanceCounterType.NumberOfItemsHEX32;
            public readonly static PerformanceCounterInstanceLifetime CounterInstanceLifetime = PerformanceCounterInstanceLifetime.Global;
            public readonly static CounterSample Sample = new CounterSample();
        }

        private InvalidOperationException rootException;
        private PerformanceCounter counter;

        public FailSafePerformanceCounter(PerformanceCounter counter)
        {
            this.counter = counter;
        }

        public FailSafePerformanceCounter(InvalidOperationException rootException)
        {
            this.rootException = rootException;
        }

        private void FailSafe(InvalidOperationException exception)
        {
            rootException = exception;
            DisposeCounter();
        }

        private void DisposeCounter()
        {
            if (counter != null)
            {
                counter.Dispose();
                counter = null;
            }
        }

        public InvalidOperationException RootException
        {
            get { return rootException; }
        }

        public string CategoryName
        {
            get
            {
                if (rootException != null)
                {
                    return DefaultValue.TextValue;
                }
                try
                {
                    return counter.CategoryName;
                }
                catch (InvalidOperationException exception)
                {
                    FailSafe(exception);
                    return DefaultValue.TextValue;
                }
            }
            set
            {
                if (rootException != null)
                {
                    return;
                }
                try
                {
                    counter.CategoryName = value;
                }
                catch (InvalidOperationException exception)
                {
                    FailSafe(exception);
                }
            }
        }

        public string CounterHelp
        {
            get
            {
                if (rootException != null)
                {
                    return DefaultValue.TextValue;
                }
                try
                {
                    return counter.CounterHelp;
                }
                catch (InvalidOperationException exception)
                {
                    FailSafe(exception);
                    return DefaultValue.TextValue;
                }
            }
        }

        public string CounterName
        {
            get
            {
                if (rootException != null)
                {
                    return DefaultValue.TextValue;
                }
                try
                {
                    return counter.CounterName;
                }
                catch (InvalidOperationException exception)
                {
                    FailSafe(exception);
                    return DefaultValue.TextValue;
                }
            }
            set
            {
                if (rootException != null)
                {
                    return;
                }
                try
                {
                    counter.CategoryName = value;
                }
                catch (InvalidOperationException exception)
                {
                    FailSafe(exception);
                }
            }
        }

        public PerformanceCounterType CounterType
        {
            get
            {
                if (rootException != null)
                {
                    return DefaultValue.CounterType;
                }
                try
                {
                    return counter.CounterType;
                }
                catch (InvalidOperationException exception)
                {
                    FailSafe(exception);
                    return DefaultValue.CounterType;
                }
            }
        }

        public PerformanceCounterInstanceLifetime InstanceLifetime
        {
            get
            {
                if (rootException != null)
                {
                    return DefaultValue.CounterInstanceLifetime;
                }
                try
                {
                    return counter.InstanceLifetime;
                }
                catch (InvalidOperationException exception)
                {
                    FailSafe(exception);
                    return DefaultValue.CounterInstanceLifetime;
                }
            }
            set
            {
                if (rootException != null)
                {
                    return;
                }
                try
                {
                    counter.InstanceLifetime = value;
                }
                catch (InvalidOperationException exception)
                {
                    FailSafe(exception);
                }
            }
        }

        public string InstanceName
        {
            get
            {
                if (rootException != null)
                {
                    return DefaultValue.TextValue;
                }
                try
                {
                    return counter.InstanceName;
                }
                catch (InvalidOperationException exception)
                {
                    FailSafe(exception);
                    return DefaultValue.TextValue;
                }
            }
            set
            {
                if (rootException != null)
                {
                    return;
                }
                try
                {
                    counter.InstanceName = value;
                }
                catch (InvalidOperationException exception)
                {
                    FailSafe(exception);
                }
            }
        }

        public string MachineName
        {
            get
            {
                if (rootException != null)
                {
                    return DefaultValue.TextValue;
                }
                try
                {
                    return counter.MachineName;
                }
                catch (InvalidOperationException exception)
                {
                    FailSafe(exception);
                    return DefaultValue.TextValue;
                }
            }
            set
            {
                if (rootException != null)
                {
                    return;
                }
                try
                {
                    counter.MachineName = value;
                }
                catch (InvalidOperationException exception)
                {
                    FailSafe(exception);
                }
            }
        }

        public long RawValue
        {
            get
            {
                if (rootException != null)
                {
                    return DefaultValue.LongValue;
                }
                try
                {
                    return counter.RawValue;
                }
                catch (InvalidOperationException exception)
                {
                    FailSafe(exception);
                    return DefaultValue.LongValue;
                }
            }
            set
            {
                if (rootException != null)
                {
                    return;
                }
                try
                {
                    counter.RawValue = value;
                }
                catch (InvalidOperationException exception)
                {
                    FailSafe(exception);
                }
            }
        }

        public bool ReadOnly
        {
            get
            {
                if (rootException != null)
                {
                    return DefaultValue.ReadOnly;
                }
                try
                {
                    return counter.ReadOnly;
                }
                catch (InvalidOperationException exception)
                {
                    FailSafe(exception);
                    return DefaultValue.ReadOnly;
                }
            }
            set
            {
                if (rootException != null)
                {
                    return;
                }
                try
                {
                    counter.ReadOnly = value;
                }
                catch (InvalidOperationException exception)
                {
                    FailSafe(exception);
                }
            }
        }

        public void BeginInit()
        {
            if (rootException != null)
            {
                return;
            }
            try
            {
                counter.BeginInit();
            }
            catch (InvalidOperationException exception)
            {
                FailSafe(exception);
            }
        }

        public void Close()
        {
            if (rootException != null)
            {
                return;
            }
            try
            {
                counter.Close();
            }
            catch (InvalidOperationException exception)
            {
                FailSafe(exception);
            }
        }

        public long Decrement()
        {
            if (rootException != null)
            {
                return DefaultValue.LongValue;
            }
            try
            {
                return counter.Decrement();
            }
            catch (InvalidOperationException exception)
            {
                FailSafe(exception);
                return DefaultValue.LongValue;
            }
        }

        public void EndInit()
        {
            if (rootException != null)
            {
                return;
            }
            try
            {
                counter.EndInit();
            }
            catch (InvalidOperationException exception)
            {
                FailSafe(exception);
            }
        }

        public long Increment()
        {
            if (rootException != null)
            {
                return DefaultValue.LongValue;
            }
            try
            {
                return counter.Increment();
            }
            catch (InvalidOperationException exception)
            {
                FailSafe(exception);
                return DefaultValue.LongValue;
            }
        }

        public long IncrementBy(long value)
        {
            if (rootException != null)
            {
                return DefaultValue.LongValue;
            }
            try
            {
                return counter.IncrementBy(value);
            }
            catch (InvalidOperationException exception)
            {
                FailSafe(exception);
                return DefaultValue.LongValue;
            }
        }

        public CounterSample NextSample()
        {
            if (rootException != null)
            {
                return DefaultValue.Sample;
            }
            try
            {
                return counter.NextSample();
            }
            catch (InvalidOperationException exception)
            {
                FailSafe(exception);
                return DefaultValue.Sample;
            }
        }

        public float NextValue()
        {
            if (rootException != null)
            {
                return DefaultValue.LongValue;
            }
            try
            {
                return counter.NextValue();
            }
            catch (InvalidOperationException exception)
            {
                FailSafe(exception);
                return DefaultValue.LongValue;
            }
        }

        public void RemoveInstance()
        {
            if (rootException != null)
            {
                return;
            }
            try
            {
                counter.RemoveInstance();
            }
            catch (InvalidOperationException exception)
            {
                FailSafe(exception);
            }
        }

        public void Dispose()
        {
            if (rootException != null)
            {
                return;
            }
            try
            {
                counter.Dispose();
            }
            catch (InvalidOperationException exception)
            {
                FailSafe(exception);
            }
        }
    }
}
