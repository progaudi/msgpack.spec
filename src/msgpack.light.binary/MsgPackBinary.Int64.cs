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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ReadFixInt64(in Span<byte> buffer, out int readSize) => TryReadFixInt64(buffer, out var result, out readSize)
            ? result
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadFixInt64(in Span<byte> buffer, out long value, out int readSize)
        {
            readSize = 9;
            var result = buffer[0] == DataCodes.Int64;
            return BinaryPrimitives.TryReadInt64BigEndian(buffer.Slice(1), out value) && result;
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
            if (value < sbyte.MinValue) return TryWriteFixInt64(buffer, (short)value, out wroteSize);
            return TryWriteInt8(buffer, (sbyte)value, out wroteSize);
        }
    }
}
