using System;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;

namespace ProGaudi.MsgPack.Light
{
    /// <summary>
    /// Methods for working with unsigned int 32
    /// </summary>
    public static partial class MsgPackBinary
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteFixUInt32(in Span<byte> buffer, uint value) => TryWriteFixUInt32(buffer, value, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteFixUInt32(in Span<byte> buffer, uint value, out int wroteSize)
        {
            wroteSize = 5;
            buffer[0] = DataCodes.UInt32;
            return BinaryPrimitives.TryWriteUInt32BigEndian(buffer.Slice(1), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ReadFixUInt32(in ReadOnlySpan<byte> buffer, out int readSize) => TryReadFixUInt32(buffer, out var result, out readSize)
            ? result
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadFixUInt32(in ReadOnlySpan<byte> buffer, out uint value, out int readSize)
        {
            readSize = 5;
            var result = buffer[0] == DataCodes.UInt32;
            return BinaryPrimitives.TryReadUInt32BigEndian(buffer.Slice(1), out value) && result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteUInt32(in Span<byte> buffer, uint value) => TryWriteUInt32(buffer, value, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteUInt32(in Span<byte> buffer, uint value, out int wroteSize)
        {
            if (value > ushort.MaxValue) return TryWriteFixUInt32(buffer, value, out wroteSize);
            return TryWriteUInt16(buffer, (ushort)value, out wroteSize);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ReadUInt32(in ReadOnlySpan<byte> buffer, out int readSize) => TryReadUInt32(buffer, out var value, out readSize)
            ? value
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadUInt32(in ReadOnlySpan<byte> buffer, out uint value, out int readSize)
        {
            var code = buffer[0];
            bool result;

            switch (code)
            {
                case DataCodes.UInt32:
                    return TryReadFixUInt32(buffer, out value, out readSize);

                case DataCodes.UInt16:
                    result = TryReadFixUInt16(buffer, out var uint16, out readSize);
                    value = uint16;
                    return result;

                case DataCodes.UInt8:
                    result = TryReadFixUInt8(buffer, out var uint8, out readSize);
                    value = uint8;
                    return result;

                case DataCodes.Int32:
                    result = TryReadFixInt32(buffer, out var int32, out readSize) && int32 >= 0;
                    value = result ? (uint)int32 : default;
                    return result;

                case DataCodes.Int16:
                    result = TryReadFixInt16(buffer, out var int16, out readSize) && int16 >= 0;
                    value = result ? (uint)int16 : default;
                    return result;

                case DataCodes.Int8:
                    result = TryReadFixInt8(buffer, out var int8, out readSize) && int8 >= 0;
                    value = result ? (uint)int8 : default;
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
