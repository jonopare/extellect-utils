namespace Extellect.Nuvo
{
    public class Prompt
    {
        private readonly TextWriter _writer;
        private readonly TextReader _reader;

        private static Maybe<YesNo> YN(string text)
        {
            return text switch
            {
                "Y" or "y" => Maybe.Some<YesNo>(true),
                "N" or "n" => Maybe.Some<YesNo>(false),
                _ => Maybe.None<YesNo>()
            };
        }

        private static string? YN(YesNo? defaultOption)
        {
            if (defaultOption == null)
                return null;
            else
                return defaultOption.Value ? "y" : "n";
        }

        public Prompt(TextWriter writer, TextReader reader)
        {
            _writer = writer ?? throw new ArgumentNullException(nameof(writer));
            _reader = reader ?? throw new ArgumentNullException(nameof(reader));
        }

        public Maybe<YesNo> Ask(string question, bool defaultValue = true)
        {
            return Ask(question, ["y", "n"], YN, YN(defaultValue));
        }

        public Maybe<string> Ask(string question, string[] options, string? defaultOption = null)
        {
            return Ask(question, options, x => Maybe.Some(x));
        }

        public Maybe<T> Ask<T>(string question, string[] options, Func<string, Maybe<T>> tryGetValue, string? defaultOption = null)
        {
            _writer.Write(question);
            _writer.Write(" ");
            PrintOptions(options, defaultOption);
            string? line;
            while ((line = _reader.ReadLine()) != null)
            {
                if (line.Length == 0 && defaultOption != null)
                {
                    return tryGetValue(defaultOption);
                }
                else
                {
                    var maybe = tryGetValue(line);
                    if (maybe.HasValue)
                    {
                        if (options.Any(o => tryGetValue(o).HasValue))
                        {
                            return maybe;
                        }
                        else
                        {
                            _writer.WriteLine("Unexpected answer; please try again");
                            PrintOptions(options, defaultOption);
                        }
                    }
                    else
                    {
                        _writer.WriteLine("Couldn't convert answer; please try again");
                        PrintOptions(options, defaultOption);
                    }
                }
            }
            return Maybe.None<T>();
        }

        private void PrintOptions(string[] options, string? defaultOption)
        {
            _writer.Write("[");
            for (var o = 0; o < options.Length; o++)
            {
                if (o > 0)
                    _writer.Write("/");
                _writer.Write(options[o]);
            }
            _writer.Write("]");
            if (defaultOption != null)
            {
                _writer.Write(" (default: ");
                _writer.Write(defaultOption);
                _writer.Write(")");
            }
            _writer.WriteLine();
        }
    }
}
