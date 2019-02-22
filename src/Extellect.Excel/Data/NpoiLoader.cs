using Extellect.Collections;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extellect.Data
{
    public sealed class NpoiLoader : ILoader, IDisposable
    {
        private readonly XSSFWorkbook _workbook;
        private readonly UniqueSortedIndex<string, XSSFSheet> _index;

        public NpoiLoader(Stream stream)
        {
            _workbook = new XSSFWorkbook(stream);
            _index = new UniqueSortedIndex<string, XSSFSheet>(x => x.SheetName);
            for (var i = 0; i < _workbook.NumberOfSheets; i++)
            {
                _index.Add((XSSFSheet)_workbook.GetSheetAt(i));
            }
        }

        public void Dispose()
        {
            
        }

        public ICollection<T> Load<T>(string tableName = null, string startAddress = null, bool transpose = false) where T : new()
        {
            return _index[tableName ?? typeof(T).Name].Load<T>(startAddress, transpose).ToArray();
        }
    }
}
