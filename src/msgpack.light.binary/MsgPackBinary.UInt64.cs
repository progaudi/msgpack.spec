using System;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;

namespace ProGaudi.MsgPack.Light
{
    /// <summary>
    /// Methods for working with unsigned int 16
    /// </summary>
    public static partial class MsgPackBinary
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteFixUInt64(in Span<byte> buffer, ulong value)
        {
            EnsureCapacity(buffer, 9);
            buffer[0] = DataCodes.UInt64;
            BinaryPrimitives.WriteUInt64BigEndian(buffer.Slice(1), value);
            return 9;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteUInt64(in Span<byte> buffer, ulong value)
        {
            if (value > uint.MaxValue) return WriteFixUInt64(buffer, value);
            return WriteUInt32(buffer, (uint)value);
        }
    }
}
