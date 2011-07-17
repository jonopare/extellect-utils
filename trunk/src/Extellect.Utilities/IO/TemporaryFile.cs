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
    /// <summary>
    /// 
    /// </summary>
    public class TemporaryFile : IDisposable
    {
        private readonly string path;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public TemporaryFile(string name)
        {
            path = Path.Combine(
                Path.GetTempPath(),
                Path.GetRandomFileName() + "-" + name
            );
        }

        /// <summary>
        /// 
        /// </summary>
        void IDisposable.Dispose()
        {
            File.Delete(path);
        }

        /// <summary>
        /// 
        /// </summary>
        public string FullName
        {
            get { return path; }
        }
    }
}