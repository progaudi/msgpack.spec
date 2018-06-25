using System;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;

namespace ProGaudi.MsgPack.Light
{
    /// <summary>
    /// Methods for working with signed int 16
    /// </summary>
    public static partial class MsgPackBinary
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteFixInt16(in Span<byte> buffer, short value)
        {
            EnsureCapacity(buffer, 3);
            buffer[0] = DataCodes.Int16;
            BinaryPrimitives.WriteInt16BigEndian(buffer.Slice(1), value);
            return 3;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteInt16(in Span<byte> buffer, short value)
        {
            if (value < sbyte.MinValue || byte.MaxValue < value) return WriteFixInt16(buffer, value);
            return value < 0 ? WriteInt8(buffer, (sbyte)value) : WriteUInt8(buffer, (byte)value);
        }
    }
}
