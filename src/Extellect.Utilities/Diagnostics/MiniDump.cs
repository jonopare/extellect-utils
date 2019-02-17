#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

namespace Extellect.Diagnostics
{
    public static class MiniDump
    {
        [DllImport("dbghelp.dll")]
        private static extern bool MiniDumpWriteDump(
            IntPtr hProcess,
            Int32 ProcessId,
            IntPtr hFile,
            MINIDUMP_TYPE DumpType,
            IntPtr ExceptionParam,
            IntPtr UserStreamParam,
            IntPtr CallackParam
            );

        public static bool Write(Process process, FileInfo file, MINIDUMP_TYPE type)
        {
            using (FileStream outputStream = file.OpenWrite())
            {
                return MiniDumpWriteDump(process.Handle, process.Id, outputStream.SafeFileHandle.DangerousGetHandle(), type, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            }
        }
    }
}
