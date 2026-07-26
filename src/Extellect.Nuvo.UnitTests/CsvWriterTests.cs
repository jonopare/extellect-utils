using System.Globalization;
using System.Numerics;

namespace Extellect.Nuvo
{
    /// <summary>
    /// The WriteField tests in this class exercise the CsvWriter's exposure of multiple
    /// WriteField overloads, including one that takes an object?. The latter must unbox
    /// and produce the same output that the correct overload would have produced.
    /// </summary>
    public class CsvWriterTests
    {
        private enum PhoneticEnum
        {
            None = 0,
            Alpha,
            Bravo,
        }

        private enum ByteEnum : byte
        {
            None = 0,
            One,
            Two,
        }

        [Fact]
        public void WriteField_Empty_WritesBlanks()
        {
            using var sw = new StringWriter();
            using (var csv = new CsvWriter(sw))
            {
                csv.WriteField();
                csv.WriteField();
                csv.WriteField();
            }

            Assert.Equal(",,", sw.ToString());
        }

        [Fact]
        public void WriteField_Enums_WritesValues()
        {
            using var sw = new StringWriter();
            using (var csv = new CsvWriter(sw))
            {
                csv.WriteField(PhoneticEnum.Alpha);
                csv.WriteField((PhoneticEnum?)null);
                csv.WriteField((object?)PhoneticEnum.Bravo);
            }

            Assert.Equal("Alpha,,Bravo", sw.ToString());
        }

        [Fact]
        public void WriteField_String_WritesValue()
        {
            using var sw = new StringWriter();
            using (var csv = new CsvWriter(sw))
            {
                csv.WriteField("abc");
                csv.WriteField((string?)null);
                csv.WriteField((object?)"def");
            }

            Assert.Equal("abc,,def", sw.ToString());
        }

        [Fact]
        public void WriteField_Char_WritesValues()
        {
            using var sw = new StringWriter();
            using (var csv = new CsvWriter(sw))
            {
                csv.WriteField('X');
                csv.WriteField((char?)null);
                csv.WriteField((object?)'Y');
            }

            Assert.Equal("X,,Y", sw.ToString());
        }

        [Fact]
        public void WriteField_Int_WritesValues()
        {
            using var sw = new StringWriter();
            using (var csv = new CsvWriter(sw))
            {
                csv.WriteField(123);
                csv.WriteField((int?)null);
                csv.WriteField((object?)456);
            }

            Assert.Equal("123,,456", sw.ToString());
        }

        [Fact]
        public void WriteField_Decimal_WritesValues()
        {
            using var sw = new StringWriter();
            using (var csv = new CsvWriter(sw))
            {
                csv.WriteField(12.5m);
                csv.WriteField((decimal?)null);
                csv.WriteField((object?)6.25m);
            }

            Assert.Equal("12.5,,6.25", sw.ToString());
        }

        [Fact]
        public void WriteField_Double_WritesValues()
        {
            using var sw = new StringWriter();
            using (var csv = new CsvWriter(sw))
            {
                csv.WriteField(12.5d);
                csv.WriteField((double?)null);
                csv.WriteField((object?)6.25d);
            }

            Assert.Equal("12.5,,6.25", sw.ToString());
        }

        [Fact]
        public void WriteField_Boolean_WritesValues()
        {
            using var sw = new StringWriter();
            using (var csv = new CsvWriter(sw))
            {
                csv.WriteField(true);
                csv.WriteField((bool?)null);
                csv.WriteField((object?)false);
            }

            Assert.Equal("true,,false", sw.ToString(), ignoreCase: true);
        }

        [Fact]
        public void WriteField_DateTime_WritesValues()
        {
            var value = new DateTime(2024, 01, 02, 03, 04, 05, DateTimeKind.Unspecified);

            using var sw = new StringWriter();
            using (var csv = new CsvWriter(sw))
            {
                csv.WriteField(value);
                csv.WriteField((DateTime?)null);
                csv.WriteField((object?)value);
            }

            Assert.Equal("2024-01-02 03:04:05,,2024-01-02 03:04:05", sw.ToString());
        }

        [Fact]
        public void WriteField_DateOnly_WritesValues()
        {
            var value = new DateOnly(2024, 01, 02);

            using var sw = new StringWriter();
            using (var csv = new CsvWriter(sw))
            {
                csv.WriteField(value);
                csv.WriteField((DateOnly?)null);
                csv.WriteField((object?)value);
            }

            Assert.Equal("2024-01-02,,2024-01-02", sw.ToString());
        }

        [Fact]
        public void WriteField_BigInteger_WritesValues()
        {
            var expected = "123456789012345678901234567890";

            var value = BigInteger.Parse(expected, CultureInfo.InvariantCulture);

            using var sw = new StringWriter();
            using (var csv = new CsvWriter(sw))
            {
                csv.WriteField(value);
                csv.WriteField((BigInteger?)null);
                csv.WriteField((object?)value);
            }

            Assert.Equal($"{expected},,{expected}", sw.ToString());
        }

        [Fact]
        public void WriteField_ByteArray_WritesValues()
        {
            var bytes = new byte[] { 1, 2, 3 };

            using var sw = new StringWriter();
            using (var csv = new CsvWriter(sw))
            {
                csv.WriteField(bytes);
                csv.WriteField((byte[]?)null);
                csv.WriteField((object?)bytes);
            }

            Assert.Equal("AQID,,AQID", sw.ToString());
        }

        [Fact]
        public void WriteField_ObjectNull_WritesEmptyField()
        {
            using var sw = new StringWriter();
            using (var csv = new CsvWriter(sw))
            {
                csv.WriteField((object?)null);
            }

            Assert.Equal("", sw.ToString());
        }
    }
}
