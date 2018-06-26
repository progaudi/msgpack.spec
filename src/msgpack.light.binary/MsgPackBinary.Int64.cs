using System;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;

namespace ProGaudi.MsgPack.Light
{
    /// <summary>
    /// Methods for working with signed int 64
    /// </summary>
    public static partial class MsgPackBinary
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteFixInt64(in Span<byte> buffer, long value)
        {
            EnsureCapacity(buffer, 9);
            buffer[0] = DataCodes.Int64;
            BinaryPrimitives.WriteInt64BigEndian(buffer.Slice(1), value);
            return 9;
        }

        // https://github.com/msgpack/msgpack/issues/164
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteInt64(in Span<byte> buffer, long value)
        {
            if (value >= 0) return WriteUInt64(buffer, (ulong)value);
            if (value < int.MinValue) return WriteFixInt64(buffer, value);
            if (value < short.MinValue) return WriteFixInt32(buffer, (int)value);
            if (value < sbyte.MinValue) return WriteFixInt16(buffer, (short)value);
            return WriteInt8(buffer, (sbyte)value);
        }
    }
}
