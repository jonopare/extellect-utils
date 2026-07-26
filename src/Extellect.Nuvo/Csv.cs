using System.Text;

namespace Extellect.Nuvo
{
    /// <summary>
    /// Encapsulates the logic required to read comma separated values in a forward-only direction.
    /// Subclasses add behaviour that will be invoked after each field or new line, and could also
    /// add their own buffers to allow random access to fields within a row.
    /// It's take the firehose approach and invokes callbacks at the appropriate points in the stream.
    /// A different approach might be required to maintain a state machine that could progress through 
    /// a stream at the caller's pace.
    /// </summary>
    public abstract class Csv
    {
        private const char Separator = ',';
        private const char Quote = '"';
        private const char CarriageReturn = '\r';
        private const char NewLine = '\n';

        private readonly char[] outbuf;
        private int outbufcount;

        /// <summary>
        /// Gets the string value of the current field.
        /// </summary>
        protected string CurrentField
        {
            get
            {
                return new string(outbuf, 0, outbufcount);
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
        /// Allows derived classes to provide behaviour that will be invoked at the end of the file.
        /// </summary>
        protected virtual void DoEndOfFile()
        {

        }

        /// <summary>
        /// Allows derived classes to provide behaviour that will be invoked at the start of the file.
        /// </summary>
        protected virtual void DoStartOfFile()
        {

        }

        /// <summary>
        /// Creates a new Csv object with the default maximum single output field capacity.
        /// </summary>
        public Csv()
            : this(8 * 1024)
        {
        }

        /// <summary>
        /// Creates a new Csv object.
        /// </summary>
        /// <param name="fieldCapacity">The maximum size of a single output field in the CSV.</param>
        public Csv(int fieldCapacity)
        {
            this.outbuf = new char[fieldCapacity];
        }

        /// <summary>
        /// Reads the specified CSV stream. The abstract methods DoEndOfField and DoEndOfLine
        /// are invoked at the appropriate points in the stream. You should override these two 
        /// methods in a derived class to make use of the CSV reader.
        /// </summary>
        /// <remarks>
        /// Implementation of CSV state machine that uses aggressive inlining and jumps between
        /// goto statements for performance reasons.
        /// </remarks>
        /// <param name="csv">A TextReader that contains CSV formatted data.</param>
        public void Read(TextReader csv)
        {
            outbufcount = Row = Column = 0;
            DoStartOfFile();
            var inbuf = new char[8 * 1024];
            var inbufcount = -1;
            var inbufpos = 0;
            char c;
        Separator:
            if (inbufpos >= inbufcount)
            {
                inbufcount = csv.Read(inbuf, 0, inbuf.Length);
                if (inbufcount == 0)
                {
                    DoEndOfField();
                    outbufcount = 0;
                    DoEndOfLine();
                    Column = 0;
                    Row++;
                    goto Exit;
                }
                inbufpos = 0;
            }
            c = inbuf[inbufpos++];
            switch (c)
            {
                case Csv.Separator:
                    DoEndOfField();
                    outbufcount = 0;
                    Column++;
                    goto Separator;
                case Csv.Quote:
                    goto QuotedField;
                case Csv.NewLine:
                    DoEndOfField();
                    outbufcount = 0;
                    DoEndOfLine();
                    Column = 0;
                    Row++;
                    goto Separator;
                case Csv.CarriageReturn:
                    goto CarriageReturn;
                default:
                    outbuf[outbufcount++] = c;
                    goto Field;
            }
        Field:
            if (inbufpos >= inbufcount)
            {
                inbufcount = csv.Read(inbuf, 0, inbuf.Length);
                if (inbufcount == 0)
                {
                    DoEndOfField();
                    outbufcount = 0;
                    DoEndOfLine();
                    Column = 0;
                    Row++;
                    goto Exit;
                }
                inbufpos = 0;
            }
            c = inbuf[inbufpos++];
            switch (c)
            {
                case Csv.Separator:
                    DoEndOfField();
                    outbufcount = 0;
                    Column++;
                    goto Separator;
                case Csv.NewLine:
                    DoEndOfField();
                    outbufcount = 0;
                    DoEndOfLine();
                    Column = 0;
                    Row++;
                    goto Separator;
                case Csv.CarriageReturn:
                    goto CarriageReturn;
                default:
                    outbuf[outbufcount++] = c;
                    goto Field;
            }
        QuotedField:
            if (inbufpos >= inbufcount)
            {
                inbufcount = csv.Read(inbuf, 0, inbuf.Length);
                if (inbufcount == 0)
                {
                    throw new FormatException("Found EOF inside quoted field");
                }
                inbufpos = 0;
            }
            c = inbuf[inbufpos++];
            switch (c)
            {
                case Csv.Quote:
                    goto QuoteInQuotedField;
                default:
                    outbuf[outbufcount++] = c;
                    goto QuotedField;
            }
        QuoteInQuotedField:
            if (inbufpos >= inbufcount)
            {
                inbufcount = csv.Read(inbuf, 0, inbuf.Length);
                if (inbufcount == 0)
                {
                    DoEndOfField();
                    outbufcount = 0;
                    DoEndOfLine();
                    Column = 0;
                    Row++;
                    goto Exit;
                }
                inbufpos = 0;
            }
            c = inbuf[inbufpos++];
            switch (c)
            {
                case Csv.Quote:
                    outbuf[outbufcount++] = c;
                    goto QuotedField;
                case Csv.Separator:
                    DoEndOfField();
                    outbufcount = 0;
                    Column++;
                    goto Separator;
                case Csv.NewLine:
                    DoEndOfField();
                    outbufcount = 0;
                    DoEndOfLine();
                    Column = 0;
                    Row++;
                    goto Separator;
                case Csv.CarriageReturn:
                    goto CarriageReturn;
                default:
                    throw new FormatException($"Found unexpected character '\\u{0:x4}' after quote in quoted field at position {Position}. Only allowed characters are another quote, comma, EOL or EOF.");
            }
        CarriageReturn:
            if (inbufpos >= inbufcount)
            {
                inbufcount = csv.Read(inbuf, 0, inbuf.Length);
                if (inbufcount == 0)
                {
                    throw new FormatException("Found EOF after carriage return.");
                }
                inbufpos = 0;
            }
            c = inbuf[inbufpos++];
            if (c != NewLine)
            {
                throw new FormatException($"Found unexpected character '\\u{0:x4}' after carriage return at position {Position}. Only allowed character is line feed.");
            }
            DoEndOfField();
            outbufcount = 0;
            DoEndOfLine();
            Column = 0;
            Row++;
            goto Separator;
        Exit:
            DoEndOfFile();
            return;
        }

        protected string Position
        {
            get
            {
                return "R" + Row + "C" + Column;
            }
        }

        /// <summary>
        /// The BOM is important for CSV files!
        /// </summary>
        public static StreamWriter Open(Stream stream)
        {
            return new StreamWriter(stream, Encoding.UTF8);
        }

        public static string QuoteValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            else if (value.IndexOfAny(new[] { Separator, Quote, CarriageReturn, NewLine }) < 0)
            {
                return value;
            }
            return "\"" + value.Replace("\"", "\"\"") + "\"";
        }
    }
}

