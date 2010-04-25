using System;
using System.Collections.Generic;
using System.Text;

namespace Extellect.Utilities.Logging
{
    /// <summary>
    /// Helper class to simplify writing colored text to the console.
    /// </summary>
    public class ConsoleColorSettings : IDisposable
    {
        private readonly ConsoleColor foregroundColor;
        private readonly ConsoleColor backgroundColor;

        /// <summary>
        /// Constructs an object with the specified foreground color and default background color.
        /// </summary>
        public ConsoleColorSettings(ConsoleColor foregroundColor)
        {
            this.foregroundColor = Console.ForegroundColor;
            this.backgroundColor = Console.BackgroundColor;
            Console.ForegroundColor = foregroundColor;
        }

        /// <summary>
        /// Constructs an object with the specified foreground color and background color.
        /// </summary>
        public ConsoleColorSettings(ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            this.foregroundColor = Console.ForegroundColor;
            this.backgroundColor = Console.BackgroundColor;
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
        }

        /// <summary>
        /// Resets the foreground and background colors to the state before the 
        /// object was created.
        /// </summary>
        public void Dispose()
        {
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
        }
    }
}
