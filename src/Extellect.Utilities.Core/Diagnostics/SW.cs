#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Extellect.Utilities.Diagnostics
{
    /// <summary>
    /// Thread-safe-(ish?) string-keyed stopwatches for non-interrupting timing. Use the Items iterator once completed to read back the logs.
    /// </summary>
    public class SW
    {
        [ThreadStatic]
        private static Dictionary<string, Stopwatch> _stopwatchesByKey;

        private static Dictionary<string, Stopwatch> StopwatchesByKey
        {
            get
            {
                if (_stopwatchesByKey == null)
                {
                    _stopwatchesByKey = new Dictionary<string, Stopwatch>();
                }
                return _stopwatchesByKey;
            }
        }

        public static IDisposable Measure(string key)
        {
            return new SWMeasure(key);
        }

        private class SWMeasure : IDisposable
        {
            private readonly string _key;

            public SWMeasure(string key)
            {
                _key = key;
                Start(key);
            }

            public void Dispose()
            {
                Stop(_key);
            }
        }

        public static void Start(string key)
        {
            var items = StopwatchesByKey;
            if (!items.ContainsKey(key))
            {
                items.Add(key, Stopwatch.StartNew());
            }
            else
            {
                items[key].Start();
            }
        }

        public static void Stop(string key)
        {
            var items = StopwatchesByKey;
            if (items.ContainsKey(key))
            {
                items[key].Stop();
            }
        }

        public static T Wrap<T>(string key, Func<T> expression)
        {
            Start(key);
            try
            {
                return expression();
            }
            finally
            {
                Stop(key);
            }
        }

        public static void Wrap(string key, Action statement)
        {
            Start(key);
            try
            {
                statement();
            }
            finally
            {
                Stop(key);
            }
        }
        
        public static void Clear()
        {
            StopwatchesByKey.Clear();
        }

        public static IEnumerable<string> Items => StopwatchesByKey.Select(pair => $"{pair.Key} took {1:#,##0} ms");
    }
}