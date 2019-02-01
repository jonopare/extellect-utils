using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extellect.Utilities.CLI
{
    /// <summary>
    /// ArgumentValidationException class.
    /// </summary>
    public class ArgumentValidationException : Exception
    {
        /// <summary>
        /// Creates a new instance of an ArgumentValidationException with the specified message.
        /// </summary>
        public ArgumentValidationException(string message) : base(message) { }

        /// <summary>
        /// Creates a new instance of an ArgumentValidationException with the specified message and inner exception.
        /// </summary>
        public ArgumentValidationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
