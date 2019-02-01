using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extellect.Utilities.Collections
{
    /// <summary>
    /// I didn't have a great name for this class.
    /// It's essentially an array helper that can help with bitmaps
    /// or matrices, when a subset of one array needs to be copied to another array
    /// </summary>
    public static class Plane
    {
        /// <summary>
        /// Writes the contents of one 2d array into another 2d array, given a
        /// set of offsets into each array, and the width and height of the area
        /// to write.
        /// </summary>
        public static void Write<T>(T[,] source, int sourceTop, int sourceLeft, T[,] target, int targetTop, int targetLeft, int width, int height)
        {
            if (targetTop < 0 || targetLeft < 0)
                throw new ArgumentOutOfRangeException(targetTop < 0 ? "targetTop" : "targetLeft", "Value cannot be negative");
            if (width < 0 || height < 0)
                throw new ArgumentOutOfRangeException(width < 0 ? "width" : "height", "Value cannot be negative");
            if (height > source.GetLength(0))
                throw new ArgumentOutOfRangeException("height", "Value cannot be greater than source height");
            if (width > source.GetLength(1))
                throw new ArgumentOutOfRangeException("width", "Value cannot be greater that source width");
            if (target.GetLength(0) - height - targetTop < 0)
                throw new ArgumentOutOfRangeException("height");
            if (target.GetLength(1) - width - targetLeft < 0)
                throw new ArgumentOutOfRangeException("width");

            for (int m = 0; m < height; m++)
            {
                for (int n = 0; n < width; n++)
                {
                    target[m + targetTop, n + targetLeft] = source[m + sourceTop, n + sourceLeft];
                }
            }
        }
    }
}
