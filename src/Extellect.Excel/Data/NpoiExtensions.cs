using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Extellect.Reflection;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Extellect.Data
{
    /// <summary>
    /// 
    /// </summary>
    internal static class NpoiExtensions
    {
        public static string Text(this ICell cell)
        {
            switch (cell.CellType)
            {
                case CellType.Blank:
                    return null;
                case CellType.Boolean:
                    return cell.BooleanCellValue.ToString();
                case CellType.Error:
                    return cell.ErrorCellValue.ToString();
                case CellType.Formula:
                    return cell.CellFormula;
                case CellType.Numeric:
                    return cell.NumericCellValue.ToString();
                case CellType.String:
                    return cell.StringCellValue;
                case CellType.Unknown:
                default:
                    return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sheet"></param>
        /// <param name="address"></param>
        /// <param name="transpose"></param>
        /// <returns></returns>
        public static IEnumerable<T> Load<T>(this XSSFSheet sheet, string address = null, bool transpose = false) where T : new()
        {
            if (address != null && address != "A1")
            {
                throw new NotImplementedException("Address not yet supported for NPOI");
            }
            if (transpose)
            {
                throw new NotImplementedException("Transpose not yet supported for NPOI");
            }

            var propertyIndex = new List<PropertyInfo>();
            var propertyTypes = new List<Type>();

            var rows = sheet.GetRowEnumerator();

            IRow row;
            if (rows.MoveNext())
            {
                row = (IRow)rows.Current;

                // we set the column header based on string value
                for (int columnID = row.FirstCellNum; columnID < row.LastCellNum; columnID++)
                {
                    var propertyName = row.Cells[columnID].Text()?.Trim();
                    var property = typeof(T).GetProperty(propertyName);
                    propertyIndex.Add(property);
                    if (property != null)
                    {
                        var propertyType = property.PropertyType.GetNullableUnderlyingType();

                        propertyTypes.Add(propertyType);
                    }
                }
            }

            while (rows.MoveNext())
            {
                row = (IRow)rows.Current;

                var item = new T();
                for (int columnID = row.FirstCellNum; columnID < row.LastCellNum; columnID++)
                {
                    var property = propertyIndex[columnID];
                    if (property != null)
                    {
                        var text = row.Cells[columnID].Text();
                        if (text == "NULL")
                        {
                            continue;
                        }
                        var value = Convert.ChangeType(text, propertyTypes[columnID]);
                        property.SetValue(item, value, null);
                    }
                }
                yield return item;
            }
        }
    }
}