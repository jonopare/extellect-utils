namespace Extellect.Nuvo
{
    public static class ModernConsole
    {
        public static void WriteLine(string value, ConsoleColor foreground, ConsoleColor? background = null)
        {
            using (new ConsoleColorScope(foreground, background ?? Console.BackgroundColor))
            {
                Console.WriteLine(value);
            }
        }

        public static void Write(string value, ConsoleColor foreground, ConsoleColor? background = null)
        {
            using (new ConsoleColorScope(foreground, background ?? Console.BackgroundColor))
            {
                Console.Write(value);
            }
        }
    }
}
