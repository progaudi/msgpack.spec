using System;
using System.Runtime.CompilerServices;

namespace ProGaudi.MsgPack.Light
{
    /// <summary>
    /// Methods for working with signed int 8
    /// </summary>
    public static partial class MsgPackBinary
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteFixInt8(in Span<byte> buffer, sbyte value)
        {
            EnsureCapacity(buffer, 2);
            buffer[0] = DataCodes.Int8;
            buffer[1] = unchecked((byte)value);
            return 2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteInt8(in Span<byte> buffer, sbyte value)
        {
            if (value >= 0) return WritePositiveFixInt(buffer, value);
            if (value >= DataCodes.FixNegativeMinSByte) return WriteNegativeFixInt(buffer, value);
            return WriteFixInt8(buffer, value);
        }
    }
}
