using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Extellect.Antlr4
{
    public class SyntaxErrorListener : IAntlrErrorListener<int>, IAntlrErrorListener<IToken>
    {
        private readonly List<string> _syntaxErrorMessages;

        public SyntaxErrorListener()
        {
            _syntaxErrorMessages = new List<string>();
        }

        public void SyntaxError(IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            AddSyntaxError(line, charPositionInLine, msg);
        }

        public void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            AddSyntaxError(line, charPositionInLine, msg);
        }

        private void AddSyntaxError(int line, int charPositionInLine, string msg)
        {
            _syntaxErrorMessages.Add($"line: {line}:{charPositionInLine} {msg}");
        }

        public IEnumerable<string> SyntaxErrorMessages => _syntaxErrorMessages;

        public void ThrowIfAnySyntaxErrors()
        {
            if (SyntaxErrorMessages.Any())
                throw new AggregateException(SyntaxErrorMessages.Select(x => new InvalidDataException(x)));
        }
    }
}
