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
        public static long ReadFixInt64(in ReadOnlySpan<byte> buffer, out int readSize) => TryReadFixInt64(buffer, out var result, out readSize)
            ? result
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadFixInt64(in ReadOnlySpan<byte> buffer, out long value, out int readSize)
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
            if (value < sbyte.MinValue) return TryWriteFixInt16(buffer, (short)value, out wroteSize);
            return TryWriteInt8(buffer, (sbyte)value, out wroteSize);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ReadInt64(in ReadOnlySpan<byte> buffer, out int readSize) => TryReadInt64(buffer, out var value, out readSize)
            ? value
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadInt64(in ReadOnlySpan<byte> buffer, out long value, out int readSize)
        {
            var code = buffer[0];
            bool result;

            switch (code)
            {
                case DataCodes.Int64:
                    return TryReadFixInt64(buffer, out value, out readSize);

                case DataCodes.Int32:
                    result = TryReadFixInt32(buffer, out var int32, out readSize);
                    value = int32;
                    return result;

                case DataCodes.Int16:
                    result = TryReadFixInt16(buffer, out var int16, out readSize);
                    value = int16;
                    return result;

                case DataCodes.Int8:
                    result = TryReadFixInt8(buffer, out var int8, out readSize);
                    value = int8;
                    return result;

                case DataCodes.UInt64:
                    result = TryReadFixUInt64(buffer, out var uint64, out readSize) && uint64 <= long.MaxValue;
                    value = result ? (long)uint64 : default;
                    return result;

                case DataCodes.UInt32:
                    result = TryReadFixUInt32(buffer, out var uint32, out readSize);
                    value = uint32;
                    return result;

                case DataCodes.UInt16:
                    result = TryReadFixUInt16(buffer, out var uint16, out readSize);
                    value = uint16;
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
