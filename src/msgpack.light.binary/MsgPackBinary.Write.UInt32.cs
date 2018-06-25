using System;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;

namespace ProGaudi.MsgPack.Light
{
    /// <summary>
    /// Methods for working with unsigned int 32
    /// </summary>
    public static partial class MsgPackBinary
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteFixUInt32(in Span<byte> buffer, uint value)
        {
            EnsureCapacity(buffer, 5);
            buffer[0] = DataCodes.UInt32;
            BinaryPrimitives.WriteUInt32BigEndian(buffer.Slice(1), value);
            return 5;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteUInt32(in Span<byte> buffer, uint value)
        {
            if (value > ushort.MaxValue) return WriteFixUInt32(buffer, value);
            return WriteUInt16(buffer, (ushort)value);
        }
    }
}
