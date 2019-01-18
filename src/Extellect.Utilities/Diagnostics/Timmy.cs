using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extellect.Utilities.Diagnostics
{
    /// <summary>
    /// Not quite the same use case as SW; decide which one you want to use.
    /// </summary>
    public sealed class Timmy : IDisposable
    {
        private readonly string _name;
        private readonly TextWriter _writer;
        private readonly Stopwatch _stopwatch;

        /// <summary>
        /// 
        /// </summary>
        public Timmy(string name, TextWriter writer)
        {
            _name = name;
            _writer = writer;
            _stopwatch = Stopwatch.StartNew();
        }

        /// <summary>
        /// 
        /// </summary>
        public Timmy(string name)
            : this(name, Console.Out)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Wrap(string name, Action statement)
        {
            using (new Timmy(name))
            {
                statement();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static T Wrap<T>(string name, Func<T> expression)
        {
            using (new Timmy(name))
            {
                return expression();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            _stopwatch.Stop();
            _writer.WriteLine($"{_name} took {_stopwatch.ElapsedMilliseconds} ms");
        }
    }
}
