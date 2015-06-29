using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.IO;
using OfficeOpenXml;
using System.Reflection;

namespace Extellect.Utilities
{
    /// <summary>
    /// A simple way to load test data from Excel
    /// </summary>
	public class ExcelLoader  : ILoader, IDisposable
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
					index = Load<Index>(package.Workbook.Worksheets["Index"], "A1", false).ToDictionary(x => x.TableName, x => x.SheetName);
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
			return Load<T>(package.Workbook.Worksheets[index[tableName ?? typeof(T).Name]], startAddress, transpose).ToArray();
		}

		private static IEnumerable<T> Load<T>(ExcelWorksheet sheet, string startAddress, bool transpose) where T : new()
		{
			var start = sheet.Dimension.Start;
			var end = sheet.Dimension.End;

			if (start.Row != 1 || start.Column != 1)
			{
				throw new InvalidOperationException(string.Format("Expected data to start from cell A1 but it didn't (found data at R={0} and C={1} instead)", start.Row, start.Column));
			}

			var rowID = 1; // one based in EPPlus

			var propertyIndex = new PropertyInfo[end.Column];
			var propertyTypes = new Type[end.Column];

			// we set the column header based on string value
			for (int columnID = 1; columnID <= end.Column; columnID++)
			{
				var propertyName = sheet.Cells[rowID, columnID].Text.Trim().Replace(" ", "_");
				var property = typeof(T).GetProperty(propertyName);
				propertyIndex[columnID - 1] = property;
				if (property == null)
				{
					// todo: warn
					continue;
				}
				var propertyType = property.PropertyType;
				if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
				{
					propertyType = propertyType.GetGenericArguments()[0];
				}
				propertyTypes[columnID - 1] = propertyType;
			}

			for (rowID = 2; rowID <= end.Row; rowID++)
			{
				var item = new T();
				for (int columnID = 1; columnID <= end.Column; columnID++)
				{
					var property = propertyIndex[columnID - 1];
					if (property != null)
					{
						var text = sheet.Cells[rowID, columnID].Text;
						if (text == "NULL")
						{
							continue;
						}
						var value = Convert.ChangeType(text, propertyTypes[columnID - 1]);
						property.SetValue(item, value, null);
					}
				}
				yield return item;
			}
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
