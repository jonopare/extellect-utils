using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extellect.Utilities.Sequencing
{
    /// <summary>
    /// Implements a monotonically incrementing sequence, similar to the SQL Server 
    /// IDENTITY() function.
    /// </summary>
    public class Identity
    {
        private readonly int increment;
        private int current;

        /// <summary>
        /// Constructs a new identity sequence with the default seed and increment of 1.
        /// </summary>
        public Identity()
            :this(1, 1)
        {
        }

        /// <summary>
        /// Constructs a new identity sequence with a specific seed and increment.
        /// </summary>
        public Identity(int seed, int increment)
        {
            this.current = seed;
            this.increment = increment;
        }

        /// <summary>
        /// Gets the next value in the sequence.
        /// </summary>
        public int Next()
        {
            try
            {
                return current;
            }
            finally
            {
                current += increment;
            }
        }
    }
}
