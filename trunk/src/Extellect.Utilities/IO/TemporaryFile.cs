using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Diagnostics;
using System.IO.Compression;
using System.IO;

namespace Extellect.Utils.IO
{
    public class TemporaryFile : IDisposable
    {
        private readonly string path;

        public TemporaryFile(string name)
        {
            path = Path.Combine(
                Path.GetTempPath(),
                Path.GetRandomFileName() + "-" + name
            );
        }

        protected void IDisposable.Dispose()
        {
            File.Delete(path);
        }

        public string FullName
        {
            get { return path; }
        }
    }
}