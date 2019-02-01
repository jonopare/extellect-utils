using System;

namespace Extellect.Utilities.CLI
{
    /// <summary>
    /// Base class from which to define custom type-safe classes that hold command line
    /// argument data.
    /// </summary>
    public class Arguments : IArguments
    {
        /// <summary>
        /// Allow parser to raise an error if an unknown key is found at the command line
        /// (i.e. if the key does not match any of the expected properties on the Arguments)
        /// </summary>
        public virtual bool IgnoreUnknown
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Allow parser to raise an error if keys are missing from the command line.
        /// NOTE: this option is equivalent to the changing the default value of the Required
        /// property of an ArgumentAttribute.
        /// </summary>
        public virtual bool IgnoreMissing
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Ignore properties that haven't been marked up with an ArgumentAttribute. Default is true.
        /// NOTE: it is the properties themselves that are ignored - any attempt to set one of these
        /// unmarked properties will still result in a validation error when IgnoreUnknown is false.
        /// </summary>
        public virtual bool IgnoreUnmarked
        {
            get { return true; }
        }

        /// <summary>
        /// Override this method to provide validation functionality in a common way.
        /// </summary>
        public virtual void Validate(Action<string> validationErrorCallback)
        {
        }
    }
}
