using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Extellect.Utilities.Sql.Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write(@"IF OBJECT_ID('RegexReplace', 'FS') IS NOT NULL
	DROP FUNCTION RegexReplace
GO
IF EXISTS(SELECT * FROM sys.assemblies WHERE name = 'Extellect')
	DROP ASSEMBLY Extellect
GO
CREATE ASSEMBLY Extellect 
AUTHORIZATION dbo
FROM ");

            var binary = new [] { "0x" }.Concat(
                File.ReadAllBytes(typeof(RegexFunctions).Assembly.Location).Select(x => x.ToString("X2"))
                );

            foreach (var c in binary)
            {
                Console.Write(c);
            }

            Console.Write(@"
WITH PERMISSION_SET = SAFE
GO
CREATE FUNCTION RegexReplace(@Input NVARCHAR(MAX), @Pattern NVARCHAR(MAX), @Replacement NVARCHAR(MAX))
RETURNS NVARCHAR(MAX)
AS
	EXTERNAL NAME Extellect.[Extellect.Utilities.Sql.RegexFunctions].RegexReplace
GO
/* you can only run this as a system admin
EXEC sp_configure 'clr enabled', 1
GO
RECONFIGURE
*/
GO
SELECT dbo.RegexReplace('ABC_Statement_03825_20141130', '^.*_(\d+)_\d+$', '$1')");
        }
    }
}
