using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extellect.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public static class Int32Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        public static DateTime ToDate(this int value)
        {
            return new DateTime(value / 10000, (value / 100) % 100, value % 100);
        }
    }
}
