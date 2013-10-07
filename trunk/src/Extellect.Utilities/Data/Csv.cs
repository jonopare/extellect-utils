using System;
using System.Collections.Generic;
using System.IO;

namespace Extellect.Utilities.Data
{
    /// <summary>
    /// Encapsulates the logic required to read comma separated values in a forward-only direction.
    /// Subclasses add behaviour that will be invoked after each field or new line, and could also
    /// add their own buffers to allow random access to fields within a row.
    /// </summary>
    public abstract class Csv
    {
        private const char Separator = ',';
        private const char Quote = '"';
        private const char CarriageReturn = '\r';
        private const char NewLine = '\n';

        private readonly char[] buffer;
        private int count;

        /// <summary>
        /// Gets the string value of the current field.
        /// </summary>
        protected string CurrentField
        {
            get
            {
                return new string(buffer, 0, count);
            }
        }

        /// <summary>
        /// Gets the zero-based current row index of the reader.
        /// </summary>
        protected int Row { get; private set; }

        /// <summary>
        /// Gets the zero-based current column index of the reader.
        /// </summary>
        protected int Column { get; private set; }

        /// <summary>
        /// Allows derived classes to provide behaviour that will be invoked at the end of each field.
        /// </summary>
        protected abstract void DoEndOfField();

        /// <summary>
        /// Allows derived classes to provide behaviour that will be invoked at the end of each line.
        /// </summary>
        protected abstract void DoEndOfLine();

        /// <summary>
        /// Creates a new Csv object with the default maximum single output field capacity.
        /// </summary>
        public Csv()
            : this(2 * 1024)
        {
        }

        /// <summary>
        /// Creates a new Csv object.
        /// </summary>
        /// <param name="fieldCapacity">The maximum size of a single output field in the CSV.</param>
        public Csv(int fieldCapacity)
        {
            this.buffer = new char[fieldCapacity];
        }

        /// <summary>
        /// Processes the input entirely.
        /// </summary>
        /// <param name="csv"></param>
        public void Read(IEnumerable<char> csv)
        {
            count = Row = Column = 0;
            var e = csv.GetEnumerator();
            Process(() => e.MoveNext() ? e.Current : -1);
        }

        /// <summary>
        /// Processes the input entirely.
        /// </summary>
        /// <param name="csv"></param>
        public void Read(TextReader csv)
        {
            count = Row = Column = 0;
            Process(csv.Read);
        }

        /// <summary>
        /// Implementation of CSV state machine that uses aggressive inlining and jumps between
        /// goto statements for performance reasons.
        /// </summary>
        /// <param name="next">A function that returns the next character or -1 at the end of the stream.</param>
        private void Process(Func<int> next)
        {
            int c;
        Separator:
            c = next();
            switch (c)
            {
                case Csv.Separator:
                    DoEndOfField();
                    count = 0;
                    Column++;
                    goto Separator;
                case Csv.Quote:
                    goto QuotedField;
                case -1:
                    DoEndOfField();
                    count = 0;
                    DoEndOfLine();
                    Column = 0;
                    Row++;
                    goto Exit;
                case Csv.NewLine:
                    DoEndOfField();
                    count = 0;
                    DoEndOfLine();
                    Column = 0;
                    Row++;
                    goto Separator;
                case Csv.CarriageReturn:
                    goto CarriageReturn;
                default:
                    buffer[count++] = (char)c;
                    goto Field;
            }
        Field:
            c = next();
            switch (c)
            {
                case Csv.Separator:
                    DoEndOfField();
                    count = 0;
                    Column++;
                    goto Separator;
                case -1:
                    DoEndOfField();
                    count = 0;
                    DoEndOfLine();
                    Column = 0;
                    Row++;
                    goto Exit;
                case Csv.NewLine:
                    DoEndOfField();
                    count = 0;
                    DoEndOfLine();
                    Column = 0;
                    Row++;
                    goto Separator;
                case Csv.CarriageReturn:
                    goto CarriageReturn;
                default:
                    buffer[count++] = (char)c;
                    goto Field;
            }
        QuotedField:
            c = next();
            switch (c)
            {
                case Csv.Quote:
                    goto QuoteInQuotedField;
                case -1:
                    throw new FormatException("Found EOF inside quoted field");
                default:
                    buffer[count++] = (char)c;
                    goto QuotedField;
            }
        QuoteInQuotedField:
            c = next();
            switch (c)
            {
                case Csv.Quote:
                    buffer[count++] = (char)c;
                    goto QuotedField;
                case Csv.Separator:
                    DoEndOfField();
                    count = 0;
                    Column++;
                    goto Separator;
                case -1:
                    DoEndOfField();
                    count = 0;
                    DoEndOfLine();
                    Column = 0;
                    Row++;
                    goto Exit;
                case Csv.NewLine:
                    DoEndOfField();
                    count = 0;
                    DoEndOfLine();
                    Column = 0;
                    Row++;
                    goto Separator;
                case Csv.CarriageReturn:
                    goto CarriageReturn;
                default:
                    throw new FormatException(string.Format("Found unexpected character '\\u{0:x4}' after quote in quoted field at position {1}. Only allowed characters are another quote, comma, EOL or EOF.", c, Position));
            }
        CarriageReturn:
            c = next();
            if (c != Csv.NewLine)
            {
                throw new FormatException(string.Format("Found unexpected character '\\u{0:x4}' after carriage return at position {1}. Only allowed character is line feed.", c, Position));
            }
            DoEndOfField();
            count = 0;
            DoEndOfLine();
            Column = 0;
            Row++;
            goto Separator;
        Exit:
            return;
        }

        private string Position
        {
            get
            {
                return "R" + Row + "C" + Column;
            }
        }
    }
}

