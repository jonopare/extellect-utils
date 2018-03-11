using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extellect.Utilities
{
    /// <summary>
    /// This enumeration allows us to declare which part of a DateTime object we are interested in.
    /// </summary>
    [Flags]
    public enum DatePart
    {
        /// <summary>
        /// Only interested in the date portion.
        /// </summary>
        DateOnly = 1,

        /// <summary>
        /// Only interested in the time portion.
        /// </summary>
        TimeOnly = 2,

        /// <summary>
        /// Interested in both date and time portions.
        /// </summary>
        Both = DateOnly | TimeOnly
    }
}
