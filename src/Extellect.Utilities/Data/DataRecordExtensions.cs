#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Extellect.Data
{
    /// <summary>
    /// Extension methods to facilitate working with nullable fields and 
    /// string indexed fields on IDataRecord objects.
    /// </summary>
    public static class DataRecordExtensions
    {
        #region HasField
        /// <summary>
        /// This method has poor performance in the case when a field with the
        /// specified name doesn't exist, so it's best to avoid calling it often
        /// especially if it's unlikely that the field exists.
        /// </summary>
        public static bool HasField(this IDataRecord dataRecord, string name)
        {
            try
            {
                // http://msdn.microsoft.com/en-us/library/system.data.idatarecord.getordinal.aspx
                // if the index is not found an IndexOutOfRangeException is thrown
                dataRecord.GetOrdinal(name);
                return true;
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
        }
        #endregion

        #region Boolean
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRecord"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool GetBoolean(this IDataRecord dataRecord, string name)
        {
            return dataRecord.GetBoolean(dataRecord.GetOrdinal(name));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRecord"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static bool? GetBooleanNullable(this IDataRecord dataRecord, int i)
        {
            return dataRecord.IsDBNull(i) ? (bool?)null : dataRecord.GetBoolean(i);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRecord"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool? GetBooleanNullable(this IDataRecord dataRecord, string name)
        {
            return dataRecord.GetBooleanNullable(dataRecord.GetOrdinal(name));
        }
        #endregion

        #region Int16
        public static short GetInt16(this IDataRecord dataRecord, string name)
        {
            return dataRecord.GetInt16(dataRecord.GetOrdinal(name));
        }

        public static short? GetInt16Nullable(this IDataRecord dataRecord, int i)
        {
            return dataRecord.IsDBNull(i) ? (short?)null : dataRecord.GetInt16(i);
        }

        public static short? GetInt16Nullable(this IDataRecord dataRecord, string name)
        {
            return dataRecord.GetInt16Nullable(dataRecord.GetOrdinal(name));
        }
        #endregion

        #region Int32
        public static int GetInt32(this IDataRecord dataRecord, string name)
        {
            return dataRecord.GetInt32(dataRecord.GetOrdinal(name));
        }

        public static int? GetInt32Nullable(this IDataRecord dataRecord, int i)
        {
            return dataRecord.IsDBNull(i) ? (int?)null : dataRecord.GetInt32(i);
        }

        public static int? GetInt32Nullable(this IDataRecord dataRecord, string name)
        {
            return dataRecord.GetInt32Nullable(dataRecord.GetOrdinal(name));
        }
        #endregion

        #region Int64
        public static long GetInt64(this IDataRecord dataRecord, string name)
        {
            return dataRecord.GetInt64(dataRecord.GetOrdinal(name));
        }

        public static long? GetInt64Nullable(this IDataRecord dataRecord, int i)
        {
            return dataRecord.IsDBNull(i) ? (long?)null : dataRecord.GetInt64(i);
        }

        public static long? GetInt64Nullable(this IDataRecord dataRecord, string name)
        {
            return dataRecord.GetInt64Nullable(dataRecord.GetOrdinal(name));
        }
        #endregion

        #region Float
        public static float GetFloat(this IDataRecord dataRecord, string name)
        {
            return dataRecord.GetFloat(dataRecord.GetOrdinal(name));
        }

        public static float? GetFloatNullable(this IDataRecord dataRecord, int i)
        {
            return dataRecord.IsDBNull(i) ? (float?)null : dataRecord.GetFloat(i);
        }

        public static float? GetFloatNullable(this IDataRecord dataRecord, string name)
        {
            return dataRecord.GetFloatNullable(dataRecord.GetOrdinal(name));
        }
        #endregion

        #region Double
        public static double GetDouble(this IDataRecord dataRecord, string name)
        {
            return dataRecord.GetDouble(dataRecord.GetOrdinal(name));
        }

        public static double? GetDoubleNullable(this IDataRecord dataRecord, int i)
        {
            return dataRecord.IsDBNull(i) ? (double?)null : dataRecord.GetDouble(i);
        }

        public static double? GetDoubleNullable(this IDataRecord dataRecord, string name)
        {
            return dataRecord.GetDoubleNullable(dataRecord.GetOrdinal(name));
        }
        #endregion

        #region Decimal
        public static decimal GetDecimal(this IDataRecord dataRecord, string name)
        {
            return dataRecord.GetDecimal(dataRecord.GetOrdinal(name));
        }

        public static decimal? GetDecimalNullable(this IDataRecord dataRecord, int i)
        {
            return dataRecord.IsDBNull(i) ? (decimal?)null : dataRecord.GetDecimal(i);
        }

        public static decimal? GetDecimalNullable(this IDataRecord dataRecord, string name)
        {
            return dataRecord.GetDecimalNullable(dataRecord.GetOrdinal(name));
        }
        #endregion

        #region Guid
        public static Guid GetGuid(this IDataRecord dataRecord, string name)
        {
            return dataRecord.GetGuid(dataRecord.GetOrdinal(name));
        }

        public static Guid? GetGuidNullable(this IDataRecord dataRecord, int i)
        {
            return dataRecord.IsDBNull(i) ? (Guid?)null : dataRecord.GetGuid(i);
        }

        public static Guid? GetGuidNullable(this IDataRecord dataRecord, string name)
        {
            return dataRecord.GetGuidNullable(dataRecord.GetOrdinal(name));
        }
        #endregion

        #region Byte(s)
        public static byte GetByte(this IDataRecord dataRecord, string name)
        {
            return dataRecord.GetByte(dataRecord.GetOrdinal(name));
        }

        public static byte? GetByteNullable(this IDataRecord dataRecord, int i)
        {
            return dataRecord.IsDBNull(i) ? (byte?)null : dataRecord.GetByte(i);
        }

        public static byte? GetByteNullable(this IDataRecord dataRecord, string name)
        {
            return dataRecord.GetByteNullable(dataRecord.GetOrdinal(name));
        }

        public static long GetBytes(this IDataRecord dataRecord, string name, long fieldoffset, byte[] buffer, int bufferoffset, int length)
        {
            return dataRecord.GetBytes(dataRecord.GetOrdinal(name), fieldoffset, buffer, bufferoffset, length);
        }

        public static long? GetBytesNullable(this IDataRecord dataRecord, int i, long fieldoffset, byte[] buffer, int bufferoffset, int length)
        {
            return dataRecord.IsDBNull(i) ? (long?)null : dataRecord.GetBytes(i, fieldoffset, buffer, bufferoffset, length);
        }

        public static long? GetBytesNullable(this IDataRecord dataRecord, string name, long fieldoffset, byte[] buffer, int bufferoffset, int length)
        {
            return dataRecord.GetBytesNullable(dataRecord.GetOrdinal(name), fieldoffset, buffer, bufferoffset, length);
        }
        #endregion

        #region Char(s)
        public static char GetChar(this IDataRecord dataRecord, string name)
        {
            return dataRecord.GetChar(dataRecord.GetOrdinal(name));
        }

        public static char? GetCharNullable(this IDataRecord dataRecord, int i)
        {
            return dataRecord.IsDBNull(i) ? (char?)null : dataRecord.GetChar(i);
        }

        public static char? GetCharNullable(this IDataRecord dataRecord, string name)
        {
            return dataRecord.GetCharNullable(dataRecord.GetOrdinal(name));
        }

        public static long GetChars(this IDataRecord dataRecord, string name, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            return dataRecord.GetChars(dataRecord.GetOrdinal(name), fieldoffset, buffer, bufferoffset, length);
        }

        public static long? GetCharsNullable(this IDataRecord dataRecord, int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            return dataRecord.IsDBNull(i) ? (long?)null : dataRecord.GetChars(i, fieldoffset, buffer, bufferoffset, length);
        }

        public static long? GetCharsNullable(this IDataRecord dataRecord, string name, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            return dataRecord.GetCharsNullable(dataRecord.GetOrdinal(name), fieldoffset, buffer, bufferoffset, length);
        }
        #endregion

        #region String
        public static string GetString(this IDataRecord dataRecord, string name)
        {
            return dataRecord.GetString(dataRecord.GetOrdinal(name));
        }

        public static string GetStringNullable(this IDataRecord dataRecord, int i)
        {
            return dataRecord.IsDBNull(i) ? (string)null : dataRecord.GetString(i);
        }

        public static string GetStringNullable(this IDataRecord dataRecord, string name)
        {
            return dataRecord.GetStringNullable(dataRecord.GetOrdinal(name));
        }
        #endregion

        #region DateTime
        public static DateTime GetDateTime(this IDataReader dataRecord, string name)
        {
            return dataRecord.GetDateTime(dataRecord.GetOrdinal(name));
        }

        public static DateTime? GetDateTimeNullable(this IDataRecord dataRecord, int i)
        {
            return dataRecord.IsDBNull(i) ? (DateTime?)null : dataRecord.GetDateTime(i);
        }

        public static DateTime? GetDateTimeNullable(this IDataRecord dataRecord, string name)
        {
            return dataRecord.GetDateTimeNullable(dataRecord.GetOrdinal(name));
        }
        #endregion

        #region RowVersion
        public static byte[] GetRowVersion(this IDataRecord dataRecord, string name)
        {
            return (byte[])dataRecord[name];
        }
        #endregion
    }
}
