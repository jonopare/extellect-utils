using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extellect.Utilities.Polling
{
    /// <summary>
    /// Interface that allows for a blocking type delay to be inserted into the execution flow.
    /// </summary>
    public interface IBlockable
    {
        /// <summary>
        /// Blocks for a specific duration
        /// </summary>
        void Block(TimeSpan duration);
    }
}
