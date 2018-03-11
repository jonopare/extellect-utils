using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Extellect.Utilities.Data;

namespace Extellect.Utilities.Tests.Unit
{
    /// <summary>
    /// Helper class to expose Test data from embedded resources
    /// </summary>
    public static class TestData
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Stream GetManifestResourceStream(string directoryName, string fileName)
        {
            var stackFrame = new StackFrame(2); // external caller
            var method = stackFrame.GetMethod();
            var t = method.DeclaringType;
            return t.Assembly.GetManifestResourceStream(t.FullName.Substring(0, t.FullName.Length - t.Name.Length) + directoryName + "." + fileName);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static ExcelLoader Excel(string fileName)
        {
            var stream = GetManifestResourceStream("Data", fileName);
            if (stream == null)
            {
                throw new ArgumentException("fileName");
            }
            return new ExcelLoader(stream);
        }
    }
}
