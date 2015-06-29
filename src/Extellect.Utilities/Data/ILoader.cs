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
    /// 
    /// </summary>
    public interface ILoader
	{
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="startAddress"></param>
        /// <param name="transpose"></param>
        /// <returns></returns>
		ICollection<T> Load<T>(string tableName = null, string startAddress = "A1", bool transpose = false) where T : new();
	}
}
