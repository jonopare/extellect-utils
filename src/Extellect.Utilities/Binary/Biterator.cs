#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extellect.Binary
{
    public class Biterator
    {
        private readonly byte[] _bytes;
        private int _position;

        public Biterator(byte[] bytes)
        {
            _bytes = bytes;
        }

        public int Read(int count)
        {
            var result = 0;
            while (count > 0)
            {
                var b = _bytes[_position / 8];
                var usedBits = _position % 8;
                var unusedBits = 8 - usedBits;

                var bits = System.Math.Min(unusedBits, count);

                result <<= bits;
                result |= (b >> (8 - (bits + usedBits))) & (0xff >> (8 - bits));

                count -= bits;
                _position += bits;
            }
            return result;
        }
    }
}
