using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extellect
{
    /// <summary>
    /// Abstracts the access to DateTime.Now and DateTime.UtcNow
    /// </summary>
    public interface IClock
    {
        /// <summary>
        /// Gets the current local time.
        /// </summary>
        DateTime Now { get; }

        /// <summary>
        /// Gets the current UTC time.
        /// </summary>
        DateTime UtcNow { get; }
    }
}
