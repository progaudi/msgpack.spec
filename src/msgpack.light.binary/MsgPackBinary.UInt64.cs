using System;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;

namespace ProGaudi.MsgPack.Light
{
    /// <summary>
    /// Methods for working with unsigned int 64
    /// </summary>
    public static partial class MsgPackBinary
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteFixUInt64(in Span<byte> buffer, ulong value) => TryWriteFixUInt64(buffer, value, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteFixUInt64(in Span<byte> buffer, ulong value, out int wroteSize)
        {
            wroteSize = 9;
            buffer[0] = DataCodes.UInt64;
            return BinaryPrimitives.TryWriteUInt64BigEndian(buffer.Slice(1), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ReadFixUInt64(in ReadOnlySpan<byte> buffer, out int readSize) => TryReadFixUInt64(buffer, out var result, out readSize)
            ? result
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadFixUInt64(in ReadOnlySpan<byte> buffer, out ulong value, out int readSize)
        {
            readSize = 9;
            var result = buffer[0] == DataCodes.UInt64;
            return BinaryPrimitives.TryReadUInt64BigEndian(buffer.Slice(1), out value) && result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteUInt64(in Span<byte> buffer, ulong value) => TryWriteUInt64(buffer, value, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteUInt64(in Span<byte> buffer, ulong value, out int wroteSize)
        {
            if (value > uint.MaxValue) return TryWriteFixUInt64(buffer, value, out wroteSize);
            return TryWriteUInt32(buffer, (uint)value, out wroteSize);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ReadUInt64(in ReadOnlySpan<byte> buffer, out int readSize) => TryReadUInt64(buffer, out var value, out readSize)
            ? value
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadUInt64(in ReadOnlySpan<byte> buffer, out ulong value, out int readSize)
        {
            var code = buffer[0];
            bool result;

            switch (code)
            {
                case DataCodes.UInt64:
                    return TryReadFixUInt64(buffer, out value, out readSize);

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

                case DataCodes.Int64:
                    result = TryReadFixInt64(buffer, out var int64, out readSize) && int64 >= 0;
                    value = result ? (ulong)int64 : default;
                    return result;

                case DataCodes.Int32:
                    result = TryReadFixInt32(buffer, out var int32, out readSize) && int32 >= 0;
                    value = result ? (ulong)int32 : default;
                    return result;

                case DataCodes.Int16:
                    result = TryReadFixInt16(buffer, out var int16, out readSize) && int16 >= 0;
                    value = result ? (ulong)int16 : default;
                    return result;

                case DataCodes.Int8:
                    result = TryReadFixInt8(buffer, out var int8, out readSize) && int8 >= 0;
                    value = result ? (ulong)int8 : default;
                    return result;
            }

            if (TryReadPositiveFixInt(buffer, out var positive, out readSize))
            {
                value = positive;
                return true;
            }

            value = default;
            return false;
        }
    }
}
