using System;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;

namespace ProGaudi.MsgPack.Light
{
    /// <summary>
    /// Methods for working with unsigned int 16
    /// </summary>
    public static partial class MsgPackBinary
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteFixUInt16(Span<byte> buffer, ushort value) => TryWriteFixUInt16(buffer, value, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteFixUInt16(Span<byte> buffer, ushort value, out int wroteSize)
        {
            wroteSize = 3;
            buffer[0] = DataCodes.UInt16;
            return BinaryPrimitives.TryWriteUInt16BigEndian(buffer.Slice(1), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ReadFixUInt16(ReadOnlySpan<byte> buffer, out int readSize) => TryReadFixUInt16(buffer, out var result, out readSize)
            ? result
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadFixUInt16(ReadOnlySpan<byte> buffer, out ushort value, out int readSize)
        {
            readSize = 3;
            var result = buffer[0] == DataCodes.UInt16;
            return BinaryPrimitives.TryReadUInt16BigEndian(buffer.Slice(1), out value) && result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteUInt16(Span<byte> buffer, ushort value) => TryWriteUInt16(buffer, value, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteUInt16(Span<byte> buffer, ushort value, out int wroteSize)
        {
            if (value > byte.MaxValue) return TryWriteFixUInt16(buffer, value, out wroteSize);
            return TryWriteUInt8(buffer, (byte)value, out wroteSize);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ReadUInt16(ReadOnlySpan<byte> buffer, out int readSize) => TryReadUInt16(buffer, out var value, out readSize)
            ? value
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadUInt16(ReadOnlySpan<byte> buffer, out ushort value, out int readSize)
        {
            var code = buffer[0];
            bool result;

            switch (code)
            {
                case DataCodes.UInt16:
                    return TryReadFixUInt16(buffer, out value, out readSize);

                case DataCodes.UInt8:
                    result = TryReadFixUInt8(buffer, out var uint8, out readSize);
                    value = uint8;
                    return result;

                case DataCodes.Int16:
                    result = TryReadFixInt16(buffer, out var int16, out readSize) && int16 >= 0;
                    value = result ? (ushort)int16 : default;
                    return result;

                case DataCodes.Int8:
                    result = TryReadFixInt8(buffer, out var int8, out readSize) && int8 >= 0;
                    value = result ? (ushort)int8 : default;
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
