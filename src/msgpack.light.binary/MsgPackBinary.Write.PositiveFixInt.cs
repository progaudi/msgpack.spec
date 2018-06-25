using System;
using System.Runtime.CompilerServices;

namespace ProGaudi.MsgPack.Light
{
    /// <summary>
    /// Methods for working with PositiveFixInt
    /// </summary>
    public static partial class MsgPackBinary
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WritePositiveFixInt(in Span<byte> buffer, byte value)
        {
            if (value > DataCodes.FixPositiveMax) throw new ArgumentOutOfRangeException(nameof(value));

            EnsureCapacity(buffer, 1);
            buffer[0] = value;
            return 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WritePositiveFixInt(in Span<byte> buffer, sbyte value)
        {
            if (value < DataCodes.FixPositiveMin) throw new InvalidOperationException(nameof(value));

            EnsureCapacity(buffer, 1);
            buffer[0] = unchecked((byte) value);
            return 1;
        }
    }
}
