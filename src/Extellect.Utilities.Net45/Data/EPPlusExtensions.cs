using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OfficeOpenXml;
using System.Reflection;

namespace Extellect.Utilities.Data
{
    /// <summary>
    /// 
    /// </summary>
    internal static class EPPlusExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sheet"></param>
        /// <param name="address"></param>
        /// <param name="transpose"></param>
        /// <returns></returns>
        public static IEnumerable<T> Load<T>(this ExcelWorksheet sheet, string address = null, bool transpose = false) where T : new()
        {
            var range = address == null ? sheet.Dimension : new ExcelAddress(address);

            var rowID = range.Start.Row;

            var propertyIndex = new PropertyInfo[range.End.Column];
            var propertyTypes = new Type[range.End.Column];

            // we set the column header based on string value
            for (int columnID = range.Start.Column; columnID <= range.End.Column; columnID++)
            {
                var propertyName = sheet.Cells[rowID, columnID].Text.Trim();
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

            for (rowID = range.Start.Row + 1; rowID <= range.End.Row; rowID++)
            {
                var item = new T();
                for (int columnID = range.Start.Column; columnID <= range.End.Column; columnID++)
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
    }
}
