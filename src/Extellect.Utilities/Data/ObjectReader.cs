#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Extellect.Data
{
    public sealed class ObjectReader<T> : IDisposable
    {
        private BinaryReader _binaryReader;
        private IBinaryReader<T> _deserializer;
        private long _count;

        public ObjectReader(Stream stream, Func<BinaryReader, IBinaryReader<T>> factoryMethod)
        {
            _binaryReader = new BinaryReader(stream);
            _deserializer = factoryMethod(_binaryReader);
        }

        public IEnumerable<T> Read()
        {
            // check if there's another record coming
            while (_binaryReader.ReadBoolean())
            {
                // then read the record itself
                yield return _deserializer.Read();
            }

            _count++;
        }

        public long Count { get { return _count; } }

        public void Dispose()
        {
            _binaryReader.Dispose();
        }
    }
}
