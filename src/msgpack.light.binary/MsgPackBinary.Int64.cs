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
        public static int WriteFixInt64(in Span<byte> buffer, int value) => TryWriteFixInt64(buffer, value, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteFixInt64(in Span<byte> buffer, long value, out int wroteSize)
        {
            wroteSize = 9;
            buffer[0] = DataCodes.Int64;
            return BinaryPrimitives.TryWriteInt64BigEndian(buffer.Slice(1), value);
        }

        // https://github.com/msgpack/msgpack/issues/164
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteInt64(in Span<byte> buffer, long value) => TryWriteInt64(buffer, value, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteInt64(in Span<byte> buffer, long value, out int wroteSize)
        {
            if (value >= 0) return TryWriteUInt64(buffer, (ulong)value, out wroteSize);
            if (value < int.MinValue) return TryWriteFixInt64(buffer, value, out wroteSize);
            if (value < short.MinValue) return TryWriteFixInt32(buffer, (int)value, out wroteSize);
            if (value < sbyte.MinValue) return TryWriteFixInt16(buffer, (short)value, out wroteSize);
            return TryWriteInt8(buffer, (sbyte)value, out wroteSize);
        }
    }
}
