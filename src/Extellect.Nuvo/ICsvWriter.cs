
using System.Numerics;

namespace Extellect.Nuvo
{
    public interface ICsvWriter
    {
        void Flush();
        void WriteField(bool? value);
        void WriteField(DateOnly? value);
        void WriteField(DateTime? value);
        void WriteField(decimal? value);
        void WriteField(double? value);
        void WriteField(int? value);
        void WriteField(string? value);
        void WriteField(byte[]? value);
        void WriteField(BigInteger? value);
        void WriteField(object? value);
        void WriteLine();
    }
}