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
            return BinaryPrimitives.TryWriteInt16BigEndian(buffer.Slice(1), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short ReadFixInt16(in Span<byte> buffer, out int readSize) => TryReadFixInt16(buffer, out var result, out readSize)
            ? result
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadFixInt16(in Span<byte> buffer, out short value, out int readSize)
        {
            readSize = 3;
            var result = buffer[0] == DataCodes.Int16;
            return BinaryPrimitives.TryReadInt16BigEndian(buffer.Slice(1), out value) && result;
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
