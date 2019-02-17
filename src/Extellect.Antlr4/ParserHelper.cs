using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.IO;

namespace Extellect.Antlr4
{
    public abstract class ParserHelper<TLexer, TParser, TListener, TResult>
        where TLexer : Lexer
        where TParser : Parser
        where TListener : IParseTreeListener
    {
        /// <summary>
        /// Override in a base class to choose which context you want to start the grammar from.
        /// </summary>
        protected abstract ParserRuleContext GetParserRuleContext(TParser parser);

        /// <summary>
        /// Override in a base class to invoke a selector on the listener after the parse tree has been walked.
        /// </summary>
        protected abstract TResult GetResult(TListener listener);

        protected virtual TLexer GetLexer(ICharStream charStream)
        {
            var ctor = typeof(TLexer).GetConstructor(new[] { typeof(ICharStream) });
            return (TLexer)ctor.Invoke(new[] { charStream });
        }

        protected virtual TParser GetParser(ITokenStream tokenStream)
        {
            var ctor = typeof(TParser).GetConstructor(new[] { typeof(ITokenStream) });
            return (TParser)ctor.Invoke(new[] { tokenStream });
        }

        protected virtual TListener GetParseTreeListener()
        {
            var ctor = typeof(TListener).GetConstructor(new Type[0]);
            return (TListener)ctor.Invoke(new object[0]);
        }

        public TResult Parse(TextReader reader)
        {
            var stream = new AntlrInputStream(reader);
            var lexer = GetLexer(stream);
            var tokens = new CommonTokenStream(lexer);
            var parser = GetParser(tokens);

            parser.RemoveErrorListeners();
            lexer.RemoveErrorListeners();
            var syntaxErrorListener = new SyntaxErrorListener();
            lexer.AddErrorListener(syntaxErrorListener);
            parser.AddErrorListener(syntaxErrorListener);

            var context = GetParserRuleContext(parser);

            syntaxErrorListener.ThrowIfAnySyntaxErrors();

            var listener = GetParseTreeListener();
            var walker = new ParseTreeWalker();
            walker.Walk(listener, context);

            return GetResult(listener);
        }

        public TResult Parse(string text)
        {
            using (var reader = new StringReader(text))
            {
                return Parse(reader);
            }
        }
    }
}
