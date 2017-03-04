#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Extellect.Utilities.Diagnostics
{
    public class SW
    {
        [ThreadStatic]
        private static Dictionary<string, Stopwatch> _items;

        private static Dictionary<string, Stopwatch> Items
        {
            get
            {
                if (_items == null)
                {
                    _items = new Dictionary<string, Stopwatch>();
                }
                return _items;
            }
        }

        public static IDisposable Measure(string key)
        {
            return new SWMeasure(key);
        }

        private class SWMeasure : IDisposable
        {
            private string key;

            public SWMeasure(string key)
            {
                this.key = key;
                Start(key);
            }

            public void Dispose()
            {
                Stop(key);
                key = null;
            }
        }

        public static void Start(string key)
        {
            var items = Items;
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
            var items = Items;
            if (items.ContainsKey(key))
            {
                items[key].Stop();
            }
        }
        
        public static void Clear()
        {
            Items.Clear();
        }

        public static IEnumerable<string> Write(TextWriter textWriter)
        {
            return Items.Select(pair => string.Format("{0} took {1} ms", pair.Key, pair.Value.ElapsedMilliseconds));
        }
    }
}