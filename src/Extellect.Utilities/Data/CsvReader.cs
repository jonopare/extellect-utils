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
    public abstract class CsvReader : IDisposable
    {
        private const char Separator = ',';
        private const char Quote = '"';
        private const char CarriageReturn = '\r';
        private const char NewLine = '\n';

        private char[] inbuf;
        private int inbufcount;
        private int inbufpos;

        private char[] outbuf;
        private int outbufcount;

        private TextReader csv;

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
        /// Creates a new Csv object with the default maximum single output field capacity.
        /// </summary>
        public CsvReader(TextReader csv)
            : this(csv, 8 * 1024)
        {
        }

        /// <summary>
        /// Creates a new Csv object.
        /// </summary>
        public CsvReader(TextReader csv, int fieldCapacity)
        {
            this.csv = csv;
            this.outbuf = new char[fieldCapacity];

            this.inbuf = new char[8 * 1024];
            this.inbufcount = -1;
            this.inbufpos = 0;

            this.Row = -1;
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
        public bool Read()
        {
            Column = 0;
            Row++;
            outbufcount = 0;
            char c;
        Separator:
            if (inbufpos >= inbufcount)
            {
                inbufcount = csv.Read(inbuf, 0, inbuf.Length);
                if (inbufcount == 0)
                {
                    DoEndOfField();
                    outbufcount = 0;
                    return false;
                }
                inbufpos = 0;
            }
            c = inbuf[inbufpos++];
            switch (c)
            {
                case CsvReader.Separator:
                    DoEndOfField();
                    outbufcount = 0;
                    Column++;
                    goto Separator;
                case CsvReader.Quote:
                    goto QuotedField;
                case CsvReader.NewLine:
                    DoEndOfField();
                    outbufcount = 0;
                    return true;
                    //goto Separator;
                case CsvReader.CarriageReturn:
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
                    return false;
                }
                inbufpos = 0;
            }
            c = inbuf[inbufpos++];
            switch (c)
            {
                case CsvReader.Separator:
                    DoEndOfField();
                    outbufcount = 0;
                    Column++;
                    goto Separator;
                case CsvReader.NewLine:
                    DoEndOfField();
                    outbufcount = 0;
                    goto Separator;
                case CsvReader.CarriageReturn:
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
                case CsvReader.Quote:
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
                    return false;
                }
                inbufpos = 0;
            }
            c = inbuf[inbufpos++];
            switch (c)
            {
                case CsvReader.Quote:
                    outbuf[outbufcount++] = c;
                    goto QuotedField;
                case CsvReader.Separator:
                    DoEndOfField();
                    outbufcount = 0;
                    Column++;
                    goto Separator;
                case CsvReader.NewLine:
                    DoEndOfField();
                    outbufcount = 0;
                    return true;
                    //goto Separator;
                case CsvReader.CarriageReturn:
                    goto CarriageReturn;
                default:
                    throw new FormatException(string.Format("Found unexpected character '\\u{0:x4}' after quote in quoted field at position {1}. Only allowed characters are another quote, comma, EOL or EOF.", c, Position));
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
            if (c != CsvReader.NewLine)
            {
                throw new FormatException(string.Format("Found unexpected character '\\u{0:x4}' after carriage return at position {1}. Only allowed character is line feed.", c, Position));
            }
            DoEndOfField();
            outbufcount = 0;
            return true;
            //goto Separator;
        }

        private string Position
        {
            get
            {
                return "R" + Row + "C" + Column;
            }
        }

        void IDisposable.Dispose()
        {
            if (csv != null)
            {
                csv.Dispose();
                csv = null;
            }
            if (inbuf != null)
            {
                inbuf = null;
            }
            if (outbuf != null)
            {
                outbuf = null;
            }
        }
    }
}

