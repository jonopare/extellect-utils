using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extellect
{
    /// <summary>
    /// Allows a user to mask out portions of strings.
    /// For example the machine name LONWS10888 might be returned as *******888 to 
    /// enable debugging without divulging excessive private information.
    /// </summary>
    public class Mask
    {
        private const char Hidden = '*';

        private readonly int left;
        private readonly int right;

        /// <summary>
        /// Creates a new mask in which the left m characters and right n characters are visible.
        /// </summary>
        public Mask(int left, int right)
        {
            if (left < 0 || right < 0)
                throw new ArgumentOutOfRangeException(left < 0 ? "left" : "right", "Cannot be less than zero");
            this.left = left;
            this.right = right;
        }

        /// <summary>
        /// Returns the masked text corresponding to the specified input.
        /// If the input length is shorter than the left and right values, no characters will be substituted.
        /// </summary>
        public string BlockMiddle(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            if (value.Length <= left + right)
                return value;

            return value.Substring(0, left)
                + new string(Hidden, value.Length - right - left)
                + value.Substring(value.Length - right);
        }
    }
}
