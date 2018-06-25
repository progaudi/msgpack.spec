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
        public static int WriteFixUInt16(in Span<byte> buffer, ushort value)
        {
            EnsureCapacity(buffer, 3);
            buffer[0] = DataCodes.UInt16;
            BinaryPrimitives.WriteUInt16BigEndian(buffer.Slice(1), value);
            return 3;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteUInt16(in Span<byte> buffer, ushort value)
        {
            if (value > byte.MaxValue) return WriteFixUInt16(buffer, value);
            return WriteUInt8(buffer, (byte)value);
        }
    }
}
