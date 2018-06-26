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
        public static int WriteFixInt32(Span<byte> buffer, int value) => TryWriteFixInt32(buffer, value, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteFixInt32(Span<byte> buffer, int value, out int wroteSize)
        {
            wroteSize = 5;
            buffer[0] = DataCodes.Int32;
            return BinaryPrimitives.TryWriteInt32BigEndian(buffer.Slice(1), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadFixInt32(ReadOnlySpan<byte> buffer, out int readSize) => TryReadFixInt32(buffer, out var result, out readSize)
            ? result
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadFixInt32(ReadOnlySpan<byte> buffer, out int value, out int readSize)
        {
            readSize = 5;
            var result = buffer[0] == DataCodes.Int32;
            return BinaryPrimitives.TryReadInt32BigEndian(buffer.Slice(1), out value) && result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteInt32(Span<byte> buffer, int value) => TryWriteInt32(buffer, value, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        // https://github.com/msgpack/msgpack/issues/164
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteInt32(Span<byte> buffer, int value, out int wroteSize)
        {
            if (value >= 0) return TryWriteUInt32(buffer, (uint)value, out wroteSize);
            if (value < short.MinValue) return TryWriteFixInt32(buffer, value, out wroteSize);
            if (value < sbyte.MinValue) return TryWriteFixInt16(buffer, (short)value, out wroteSize);
            return TryWriteInt8(buffer, (sbyte)value, out wroteSize);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadInt32(ReadOnlySpan<byte> buffer, out int readSize) => TryReadInt32(buffer, out var value, out readSize)
            ? value
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadInt32(ReadOnlySpan<byte> buffer, out int value, out int readSize)
        {
            var code = buffer[0];
            bool result;

            switch (code)
            {
                case DataCodes.Int32:
                    return TryReadFixInt32(buffer, out value, out readSize);

                case DataCodes.Int16:
                    result = TryReadFixInt16(buffer, out var int16, out readSize);
                    value = int16;
                    return result;

                case DataCodes.Int8:
                    result = TryReadFixInt8(buffer, out var int8, out readSize);
                    value = int8;
                    return result;

                case DataCodes.UInt32:
                    result = TryReadFixUInt32(buffer, out var uint32, out readSize) && uint32 <= int.MaxValue;
                    value = result ? (int)uint32 : default;
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
