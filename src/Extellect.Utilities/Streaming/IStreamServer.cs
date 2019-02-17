#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extellect.Streaming
{
    public interface IStreamServer
    {
        long Open(string path);
        byte[] Read(long streamId, long position, int length);
        void Close(long streamId);
    }
}
