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
        public static int WriteFixInt16(in Span<byte> buffer, short value) => TryWriteFixInt16(buffer, value, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteFixInt16(in Span<byte> buffer, short value, out int wroteSize)
        {
            wroteSize = 3;
            buffer[0] = DataCodes.Int16;
            BinaryPrimitives.WriteInt16BigEndian(buffer.Slice(1), value);
            return true;
        }

        // https://github.com/msgpack/msgpack/issues/164
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteInt16(in Span<byte> buffer, short value) => TryWriteInt16(buffer, value, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteInt16(in Span<byte> buffer, short value, out int wroteSize)
        {
            if (value >= 0) return TryWriteUInt16(buffer, (ushort)value, out wroteSize);
            if (value < sbyte.MinValue) return TryWriteFixInt16(buffer, value, out wroteSize);
            return TryWriteInt8(buffer, (sbyte)value, out wroteSize);
        }
    }
}
