using System;

namespace Extellect.Utilities.CLI
{
    /// <summary>
    /// This custom attribute allows you to define the name of a command line argument,
    /// specify its default value (if not provided) and whether or not a value must be provided.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ArgumentAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the name of the argument. If set to an empty string then the 
        /// argument will be named for the property on which this attribute is applied.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the default value for the argument, which will be used 
        /// if no value was set on the command line.
        /// </summary>
        public string Default { get; set; }

        /// <summary>
        /// Gets or sets whether a value is required to be set on the command line.
        /// </summary>
        public bool Required { get; set; }
    }
}
