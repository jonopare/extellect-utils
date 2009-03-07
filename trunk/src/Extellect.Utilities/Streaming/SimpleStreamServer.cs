using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Extellect.Utilities.Streaming
{
    public class SimpleStreamServer : IStreamServer
    {
        Dictionary<long, Stream> streams = new Dictionary<long, Stream>();
        Random random = new Random();

        #region IStreamServer Members

        public long Open(string path)
        {
            int streamId = random.Next();
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            streams.Add(streamId, stream);
            return streamId;
        }

        public byte[] Read(long streamId, long position, int length)
        {
            Stream stream = streams[streamId];
            if (stream.Position != position)
            {
                stream.Seek(position, SeekOrigin.Begin);
            }
            byte[] buffer = new byte[Math.Min(length, 64 * 1024)];
            Array.Resize(ref buffer, stream.Read(buffer, 0, buffer.Length));
            return buffer;
        }

        public void Close(long streamId)
        {
            streams[streamId].Dispose();
            streams.Remove(streamId);
        }

        #endregion
    }
}
