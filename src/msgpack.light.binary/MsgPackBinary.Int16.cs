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
        public static int WriteFixInt16(Span<byte> buffer, short value) => TryWriteFixInt16(buffer, value, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteFixInt16(Span<byte> buffer, short value, out int wroteSize)
        {
            wroteSize = 3;
            buffer[0] = DataCodes.Int16;
            return BinaryPrimitives.TryWriteInt16BigEndian(buffer.Slice(1), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short ReadFixInt16(ReadOnlySpan<byte> buffer, out int readSize) => TryReadFixInt16(buffer, out var result, out readSize)
            ? result
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadFixInt16(ReadOnlySpan<byte> buffer, out short value, out int readSize)
        {
            readSize = 3;
            var result = buffer[0] == DataCodes.Int16;
            return BinaryPrimitives.TryReadInt16BigEndian(buffer.Slice(1), out value) && result;
        }

        // https://github.com/msgpack/msgpack/issues/164
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteInt16(Span<byte> buffer, short value) => TryWriteInt16(buffer, value, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteInt16(Span<byte> buffer, short value, out int wroteSize)
        {
            if (value >= 0) return TryWriteUInt16(buffer, (ushort)value, out wroteSize);
            if (value < sbyte.MinValue) return TryWriteFixInt16(buffer, value, out wroteSize);
            return TryWriteInt8(buffer, (sbyte)value, out wroteSize);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short ReadInt16(ReadOnlySpan<byte> buffer, out int readSize) => TryReadInt16(buffer, out var value, out readSize)
            ? value
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadInt16(ReadOnlySpan<byte> buffer, out short value, out int readSize)
        {
            var code = buffer[0];
            bool result;

            switch (code)
            {
                case DataCodes.Int16:
                    return TryReadFixInt16(buffer, out value, out readSize);

                case DataCodes.Int8:
                    result = TryReadFixInt8(buffer, out var int8, out readSize);
                    value = int8;
                    return result;

                case DataCodes.UInt16:
                    result = TryReadFixUInt16(buffer, out var uint16, out readSize) && uint16 <= short.MaxValue;
                    value = result ? (short)uint16 : default;
                    return result;

                case DataCodes.UInt8:
                    result = TryReadFixUInt8(buffer, out var uint8, out readSize);
                    value = uint8;
                    return result;
            }

            if (TryReadPositiveFixInt(buffer, out var positive, out readSize))
            {
                value = positive;
                return true;
            }

            if (TryReadNegativeFixInt(buffer, out var negative, out readSize))
            {
                value = negative;
                return true;
            }

            value = default;
            return false;
        }
    }
}
