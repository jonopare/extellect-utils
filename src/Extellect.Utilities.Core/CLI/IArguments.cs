namespace Extellect.Utilities.CLI
{
    /// <summary>
    /// Base interface from which to define custom type-safe classes that hold command line
    /// argument data.
    /// </summary>
    public interface IArguments
    {
        /// <summary>
        /// Allow parser to raise an error if an unknown key is found at the command line
        /// (i.e. if the key does not match any of the expected properties on the Arguments)
        /// </summary>
        bool IgnoreUnknown { get; }

        /// <summary>
        /// Allow parser to raise an error if keys are missing from the command line.
        /// </summary>
        bool IgnoreMissing { get; }

        /// <summary>
        /// Ignore properties that haven't been marked up with an ArgumentAttribute. Default is true.
        /// NOTE: it is the properties themselves that are ignored - any attempt to set one of these
        /// unmarked properties will still result in a validation error when IgnoreUnknown is false.
        /// </summary>
        bool IgnoreUnmarked { get; }
    }
}
