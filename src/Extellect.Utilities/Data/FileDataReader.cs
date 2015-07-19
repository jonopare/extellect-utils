using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;

namespace Extellect.Utilities.Data
{
    /// <summary>
    /// Tries to wrap a CSV file into an IDataReader style object, useful in many cases.
    /// </summary>
    public class FileDataReader : IDataReader
    {
        private IEnumerator<string> enumerator;
        private int fieldCount;
        private string[] textFields;
        private object[] fields;

        private Dictionary<string, int> fileOrdinals;
        private Dictionary<string, Func<object>> other;
        private List<Func<object>> functions;
        private int firstFunctionOrdinal;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileOrdinals">A map of column names to ordinal position in the CSV input</param>
        /// <param name="other">A map of column names to functions that can execute and return data that is not from any of the CSV fields</param>
        public FileDataReader(string path, Dictionary<string, int> fileOrdinals, Dictionary<string, Func<object>> other)
        {
            enumerator = File.ReadLines(path).GetEnumerator();


            this.fileOrdinals = fileOrdinals;
            this.other = other;
            functions = new List<Func<object>>();
            firstFunctionOrdinal = fileOrdinals.Max(x => x.Value) + 1;

            this.fieldCount = firstFunctionOrdinal + other.Count;

            //Func<string, int> getOrdinal, Func<int, object> getValue
            //this.getOrdinal = getOrdinal;
            //this.getValue = getValue;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Close()
        {
            Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        public int Depth
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsClosed
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool NextResult()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This doesn't use proper CSV, which is bad. Next version, I promise.
        /// </summary>
        /// <returns></returns>
        public bool Read()
        {
            var result = enumerator.MoveNext();
            if (result)
            {
                textFields = enumerator.Current.Split(',').Select(x => x.Trim()).ToArray();
            }
            else
            {
                textFields = null;
            }
            fields = textFields;
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public int RecordsAffected
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            enumerator.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        public int FieldCount
        {
            get { return fieldCount; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public bool GetBoolean(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public byte GetByte(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <param name="fieldOffset"></param>
        /// <param name="buffer"></param>
        /// <param name="bufferoffset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public char GetChar(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <param name="fieldoffset"></param>
        /// <param name="buffer"></param>
        /// <param name="bufferoffset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public string GetDataTypeName(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public DateTime GetDateTime(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public decimal GetDecimal(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public double GetDouble(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Type GetFieldType(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public float GetFloat(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Guid GetGuid(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public short GetInt16(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public int GetInt32(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public long GetInt64(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public string GetName(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetOrdinal(string name)
        {
            int ordinal;
            if (fileOrdinals.TryGetValue(name, out ordinal))
            {
                return ordinal;
            }
            Func<object> function;
            if (other.TryGetValue(name, out function))
            {
                ordinal = functions.IndexOf(function);
                if (ordinal >= 0)
                {
                    return ordinal + firstFunctionOrdinal;
                }
                functions.Add(function);
                return functions.Count - 1 + firstFunctionOrdinal;
            }
            throw new InvalidOperationException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public string GetString(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public object GetValue(int i)
        {
            if (i < firstFunctionOrdinal)
            {
                return fields[i];
            }
            else
            {
                return functions[i - firstFunctionOrdinal]();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public bool IsDBNull(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object this[string name]
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public object this[int i]
        {
            get { throw new NotImplementedException(); }
        }
    }

}
