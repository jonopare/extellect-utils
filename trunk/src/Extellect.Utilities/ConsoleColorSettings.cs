using System;
using System.Collections.Generic;
using System.Text;

namespace Extellect.Utilities
{
    public class ConsoleColorSettings : IDisposable
    {
        private readonly ConsoleColor foregroundColor;
        private readonly ConsoleColor backgroundColor;

        public ConsoleColorSettings(ConsoleColor foregroundColor)
        {
            this.foregroundColor = Console.ForegroundColor;
            this.backgroundColor = Console.BackgroundColor;
            Console.ForegroundColor = foregroundColor;
        }

        public ConsoleColorSettings(ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            this.foregroundColor = Console.ForegroundColor;
            this.backgroundColor = Console.BackgroundColor;
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
        }

        public void Dispose()
        {
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
        }
    }
}
