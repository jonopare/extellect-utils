namespace Extellect.Nuvo
{
    internal sealed class ConsoleColorScope : IDisposable
    {
        private readonly ConsoleColor _foreground;
        private readonly ConsoleColor _background;

        public ConsoleColorScope(ConsoleColor foreground, ConsoleColor background)
        {
            _foreground = Console.ForegroundColor;
            _background = Console.BackgroundColor;
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
        }

        public void Dispose()
        {
            Console.ForegroundColor = _foreground;
            Console.BackgroundColor = _background;
        }
    }
}
