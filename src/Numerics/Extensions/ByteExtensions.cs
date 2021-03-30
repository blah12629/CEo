using System.Collections.Generic;
using System.Numerics;

namespace System
{
    public static partial class ByteExtensions
    {
        public static BigInteger Join(this IEnumerable<Byte> digits)
        {
            BigInteger value = 0;

            foreach (var digit in digits)
                value = digit + value * (
                    digit < 10 ? 10 : digit < 100 ? 100 : 1000);

            return value;
        }
    }
}