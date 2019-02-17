#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Extellect.Data
{
    public sealed class ObjectWriter<T> : IDisposable
    {
        private BinaryWriter _binaryWriter;
        private IBinaryWriter<T> _serializer;
        private long _count;

        public ObjectWriter(Stream stream, Func<BinaryWriter, IBinaryWriter<T>> factoryMethod)
        {
            _binaryWriter = new BinaryWriter(stream);
            _serializer = factoryMethod(_binaryWriter);
        }

        public void Write(T item)
        {
            // indicate there's another record coming
            _binaryWriter.Write(true);

            // then write the record itself
            _serializer.Write(item);

            _count++;
        }

        public void Write(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                Write(item);
            }
        }

        public long Count { get { return _count; } }

        public void Dispose()
        {
            _binaryWriter.Flush();
            _binaryWriter.Dispose();
        }
    }
}
