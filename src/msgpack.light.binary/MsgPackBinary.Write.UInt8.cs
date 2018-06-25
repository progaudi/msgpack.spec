using System;
using System.Runtime.CompilerServices;

namespace ProGaudi.MsgPack.Light
{
    /// <summary>
    /// Methods for working with unsigned int 8
    /// </summary>
    public static partial class MsgPackBinary
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteFixUInt8(in Span<byte> buffer, byte value)
        {
            EnsureCapacity(buffer, 2);
            buffer[0] = DataCodes.UInt8;
            buffer[1] = value;
            return 2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteUInt8(in Span<byte> buffer, byte value)
        {
            if (value <= DataCodes.FixPositiveMax) return WritePositiveFixInt(buffer, value);
            return WriteFixUInt8(buffer, value);
        }
    }
}
