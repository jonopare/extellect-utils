using System.Globalization;
using System.Numerics;

namespace Extellect.Nuvo
{
    public sealed class CsvWriter : IDisposable, ICsvWriter
    {
        private readonly TextWriter _writer;
        private int _column;
        private bool _disposed;

        public CsvWriter(TextWriter writer)
        {
            _writer = writer;
        }

        public void Flush()
        {
            ThrowIfDisposed();
            _writer.Flush();
        }

        public void WriteField(BigInteger? value)
        {
            WriteField(ToString(value));
        }

        public void WriteField(char? value)
        {
            WriteField(ToString(value));
        }

        public void WriteField(int? value)
        {
            WriteField(ToString(value));
        }

        public void WriteField(byte[]? value)
        {
            WriteField(ToString(value));
        }

        public void WriteField(decimal? value)
        {
            WriteField(ToString(value));
        }

        public void WriteField(double? value)
        {
            WriteField(ToString(value));
        }

        public void WriteField(DateTime? value)
        {
            WriteField(ToString(value));
        }

        public void WriteField(DateOnly? value)
        {
            WriteField(ToString(value));
        }

        public void WriteField(bool? value)
        {
            WriteField(ToString(value));
        }

        public void WriteField(object? value)
        {
            WriteField(ToString(value));
        }

        public void WriteField()
        {
            WriteField((string?)null);
        }

        public void WriteField(string? value)
        {
            ThrowIfDisposed();
            if (_column++ > 0)
            {
                _writer.Write(',');
            }
            if (value != null)
                _writer.Write(Csv.QuoteValue(value));
        }

        public void WriteLine()
        {
            ThrowIfDisposed();
            _column = 0;
            _writer.WriteLine();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _writer.Flush();
                _writer.Dispose();
                _disposed = true;
            }
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(CsvWriter));
        }

        private static string? ToString(decimal? value)
        {
            return value?.ToString(CultureInfo.InvariantCulture);
        }

        private static string? ToString(double? value)
        {
            return value?.ToString("R", CultureInfo.InvariantCulture);
        }

        private static string? ToString(char? value)
        {
            return value?.ToString();
        }

        private static string? ToString(long? value)
        {
            return value?.ToString(CultureInfo.InvariantCulture);
        }

        private static string? ToString(DateTime? value)
        {
            return value?.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        }

        private static string? ToString(DateOnly? value)
        {
            return value?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        private static string? ToString(bool? value)
        {
            return value?.ToString(CultureInfo.InvariantCulture);
        }

        private static string? ToString(Guid? value)
        {
            return value?.ToString();
        }

        private static string? ToString(byte[]? value)
        {
            return value == null ? null : Convert.ToBase64String(value);
        }

        private static string? ToString(BigInteger? value)
        {
            return value?.ToString();
        }

        private static string? ToString(object? value)
        {
            if (value == null)
            {
                return null;
            }
            else
            {
                var valueType = value.GetType();
                var underlyingType = valueType.GetNullableUnderlyingType();
                if (underlyingType.IsEnum)
                {
                    return value.ToString();
                }
                else
                {
                    switch (Type.GetTypeCode(underlyingType))
                    {
                        case TypeCode.String:
                            return (string)value;
                        case TypeCode.Char:
                            return ((char)value).ToString();
                        case TypeCode.Decimal:
                            return ToString((decimal?)value);
                        case TypeCode.Double:
                            return ToString((double?)value);
                        case TypeCode.Byte:
                            return ToString((byte?)value);
                        case TypeCode.SByte:
                            return ToString((sbyte?)value);
                        case TypeCode.Int16:
                            return ToString((short?)value);
                        case TypeCode.UInt16:
                            return ToString((ushort?)value);
                        case TypeCode.Int32:
                            return ToString((int?)value);
                        case TypeCode.UInt32:
                            return ToString((uint?)value);
                        case TypeCode.Int64:
                            return ToString((long?)value);
                        case TypeCode.Boolean:
                            return ToString((bool?)value);
                        case TypeCode.DateTime:
                            return ToString((DateTime?)value);
                        default:
                            if (underlyingType == typeof(DateOnly))
                                return ToString((DateOnly?)value);
                            else if (underlyingType == typeof(Guid))
                                return ToString((Guid?)value);
                            else if (underlyingType == typeof(byte[]))
                                return ToString((byte[]?)value);
                            else if (underlyingType == typeof(BigInteger))
                                return ToString((BigInteger?)value);
                            throw new NotSupportedException();
                    }
                }
            }
        }
    }
}
