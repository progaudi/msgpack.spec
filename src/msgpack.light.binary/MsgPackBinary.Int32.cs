using System;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;

namespace ProGaudi.MsgPack.Light
{
    /// <summary>
    /// Methods for working with signed int 32
    /// </summary>
    public static partial class MsgPackBinary
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteFixInt32(in Span<byte> buffer, int value)
        {
            EnsureCapacity(buffer, 5);
            buffer[0] = DataCodes.Int32;
            BinaryPrimitives.WriteInt32BigEndian(buffer.Slice(1), value);
            return 5;
        }

        // https://github.com/msgpack/msgpack/issues/164
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteInt32(in Span<byte> buffer, int value)
        {
            if (value >= 0) return WriteUInt32(buffer, (uint)value);
            if (value < short.MinValue) return WriteFixInt32(buffer, value);
            if (value < sbyte.MinValue) return WriteFixInt16(buffer, (short)value);
            return WriteInt8(buffer, (sbyte)value);
        }
    }
}
