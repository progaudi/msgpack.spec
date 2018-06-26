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
        public static int WriteFixInt32(in Span<byte> buffer, int value) => TryWriteFixInt32(buffer, value, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteFixInt32(in Span<byte> buffer, int value, out int wroteSize)
        {
            wroteSize = 5;
            buffer[0] = DataCodes.Int32;
            BinaryPrimitives.WriteInt32BigEndian(buffer.Slice(1), value);
            return true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteInt32(in Span<byte> buffer, int value) => TryWriteInt32(buffer, value, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        // https://github.com/msgpack/msgpack/issues/164
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteInt32(in Span<byte> buffer, int value, out int wroteSize)
        {
            if (value >= 0) return TryWriteUInt32(buffer, (uint)value, out wroteSize);
            if (value < short.MinValue) return TryWriteFixInt32(buffer, value, out wroteSize);
            if (value < sbyte.MinValue) return TryWriteFixInt16(buffer, (short)value, out wroteSize);
            return TryWriteInt8(buffer, (sbyte)value, out wroteSize);
        }
    }
}
