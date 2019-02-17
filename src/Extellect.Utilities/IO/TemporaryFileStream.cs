#pragma warning disable 1591
using System;
using System.IO;

namespace Extellect.IO
{
    public sealed class TemporaryFileStream : Stream, IDisposable
    {
        private readonly FileInfo _file;
        private readonly FileStream _stream;

        public TemporaryFileStream(FileMode fileMode)
        {
            _file = new FileInfo(Path.GetTempFileName());
            _stream = _file.Open(fileMode);
        }

        public override bool CanRead => _stream.CanRead;

        public override bool CanSeek => _stream.CanSeek;

        public override bool CanWrite => _stream.CanWrite;

        public override long Length => _stream.Length;

        public override long Position { get => _stream.Position; set => _stream.Position = value; }
        
        public override void Flush()
        {
            _stream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _stream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _stream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _stream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _stream.Write(buffer, offset, count);
        }

        protected override void Dispose(bool disposing)
        {
            _stream.Dispose();
            _file.Delete();

            base.Dispose(disposing);
        }
    }
}
