using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.IO;
using OfficeOpenXml;
using System.Reflection;

namespace Extellect.Utilities.Data
{
    /// <summary>
    /// A simple way to load test data from Excel
    /// </summary>
    public class ExcelLoader : ILoader, IDisposable
    {
        private class Index
        {
            public string TableName { get; set; }
            public string SheetName { get; set; }
        }

        Stream stream;
        ExcelPackage package;
        Dictionary<string, string> index;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        public ExcelLoader(Stream stream)
        {
            this.stream = stream;
            try
            {
                package = new ExcelPackage(stream);
                try
                {
                    index = package.Workbook.Worksheets["Index"].Load<Index>("A1", false).ToDictionary(x => x.TableName, x => x.SheetName);
                }
                catch
                {
                    package.Dispose();
                    throw;
                }
            }
            catch
            {
                stream.Dispose();
                stream = null;
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="startAddress"></param>
        /// <param name="transpose"></param>
        /// <returns></returns>
        public ICollection<T> Load<T>(string tableName = null, string startAddress = "A1", bool transpose = false) where T : new()
        {
            return package.Workbook.Worksheets[index[tableName ?? typeof(T).Name]].Load<T>(startAddress, transpose).ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if (stream != null)
            {
                stream.Dispose();
                stream = null;
            }
            if (package != null)
            {
                package.Dispose();
                package = null;
            }
            index = null;
        }
    }
}
