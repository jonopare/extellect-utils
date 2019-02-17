using System;
using System.Text.RegularExpressions;
using Microsoft.SqlServer.Server;

namespace Extellect.Sql
{
    /// <summary>
    /// Note: this assembly must be built with .NET Framework 2.0 in order to work correctly.
    /// </summary>
    public static class RegexFunctions
    {
        /// <summary>
        /// Calls Regex.Replace and allows capture of values based on regular expressions.
        /// </summary>
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true)]
        public static string RegexReplace(string input, string pattern, string replacement)
        {
            return Regex.Replace(input, pattern, replacement);
        }
    }
}
