using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Extellect.Utilities.Collections
{
    /// <summary>
    /// Exposes extension methods that facilitate querying windows within sequences and streams.
    /// Exposes extension methods to find patterns within sequences and streams.
    /// </summary>
    public static class Windowing
    {
        /// <summary>
        /// Exposes an efficient window around a sequence allowing a number of 
        /// items to be read in an iterative manner from an arbitrary offset.
        /// </summary>
        /// <typeparam name="T">Type of items.</typeparam>
        /// <param name="items">Sequence around which the window is placed.</param>
        /// <param name="offset">Offset into the sequence from which the window begins.</param>
        /// <param name="count">Number of items to read from the stream with this iterator.</param>
        /// <returns>The items from the sequence corresponding to the given offset and count.</returns>
        public static IEnumerable<T> Window<T>(this IList<T> items, int offset, int count)
        {
            if (offset + count > items.Count)
            {
                throw new ArgumentOutOfRangeException();
            }
            for (int i = 0; i < count; i++)
            {
                yield return items[offset + i];
            }
        }

        /// <summary>
        /// Exposes an efficient window around a stream allowing bytes to be read
        /// in an iterative manner.
        /// </summary>
        /// <param name="stream">Stream around which the window is placed.</param>
        /// <param name="offset">Offset into the stream from which the window begins.</param>
        /// <param name="count">Number of bytes to read from the stream in this iterator.</param>
        /// <returns>The bytes from the stream corresponding to the given offset and count.</returns>
        public static IEnumerable<byte> Window(this Stream stream, int offset, int count)
        {
            if (offset + count > stream.Length)
            {
                throw new ArgumentOutOfRangeException();
            }
            for (int i = 0; i < count; i++)
            {
                if (stream.Position != offset + i)
                {
                    stream.Seek(offset + i, SeekOrigin.Begin);
                }
                yield return (byte)stream.ReadByte();
            }
        }

        /// <summary>
        /// Finds the occurrences of a pattern in a longer sequence.
        /// </summary>
        /// <typeparam name="T">Type of items in each sequence.</typeparam>
        /// <param name="sequence">Sequence to search.</param>
        /// <param name="pattern">Pattern to search for.</param>
        /// <param name="overlap">True if matches can overlap; otherwise false.</param>
        /// <returns>Indexes in the sequence that matched the given pattern.</returns>
        public static IEnumerable<int> Find<T>(this IList<T> sequence, IList<T> pattern, bool overlap)
        {
            int i = 0; while (i < sequence.Count - (pattern.Count > 0 ? pattern.Count : 1) + 1)
            {
                if (pattern.SequenceEqual(sequence.Window(i, pattern.Count)))
                { 
                    yield return i; 
                    i += overlap ? 1 : (pattern.Count > 0 ? pattern.Count : 1); 
                }
                else 
                {
                    i++; 
                }
            }
        }

        /// <summary>
        /// Finds the occurrences of a byte pattern in a stream.
        /// </summary>
        /// <param name="stream">Stream to search for byte pattern.</param>
        /// <param name="pattern">Byte pattern to search for.</param>
        /// <param name="overlap">True if matches can overlap; otherwise false.</param>
        /// <returns>Positions in the stream that matched the given byte pattern.</returns>
        public static IEnumerable<int> Find(this Stream stream, IList<byte> pattern, bool overlap)
        {
            int i = 0; while (i < stream.Length - (pattern.Count > 0 ? pattern.Count : 1) + 1)
            {
                if (pattern.SequenceEqual(stream.Window(i, pattern.Count)))
                {
                    yield return i;
                    i += overlap ? 1 : (pattern.Count > 0 ? pattern.Count : 1);
                }
                else
                {
                    i++;
                }
            }
        }
    }
}
