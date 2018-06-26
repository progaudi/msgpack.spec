using System;
using System.Runtime.CompilerServices;

namespace ProGaudi.MsgPack.Light
{
    /// <summary>
    /// Methods for working with signed int 8
    /// </summary>
    public static partial class MsgPackBinary
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteFixInt8(in Span<byte> buffer, sbyte value) => TryWriteFixInt8(buffer, value, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteFixInt8(in Span<byte> buffer, sbyte value, out int wroteSize)
        {
            wroteSize = 2;
            buffer[0] = DataCodes.Int8;
            buffer[1] = unchecked((byte)value);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte ReadFixInt8(in Span<byte> buffer, out int readSize) => TryReadFixInt8(buffer, out var result, out readSize)
            ? result
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadFixInt8(in Span<byte> buffer, out sbyte value, out int readSize)
        {
            readSize = 2;
            var result = buffer[0] == DataCodes.Int8;
            value = result ? unchecked((sbyte)buffer[1]) : default;
            return result;
        }

        // https://github.com/msgpack/msgpack/issues/164
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteInt8(in Span<byte> buffer, sbyte value) => TryWriteInt8(buffer, value, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteInt8(in Span<byte> buffer, sbyte value, out int wroteSize)
        {
            if (value >= 0) return TryWriteUInt8(buffer, (byte)value, out wroteSize);
            if (value >= DataCodes.FixNegativeMinSByte) return TryWriteNegativeFixInt(buffer, value, out wroteSize);
            return TryWriteFixInt8(buffer, value, out wroteSize);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte ReadInt8(in Span<byte> buffer, out int readSize) => TryReadInt8(buffer, out var result, out readSize)
            ? result
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadInt8(in Span<byte> buffer, out sbyte value, out int readSize)
        {
            if (TryReadFixUInt8(buffer, out var byteResult, out readSize))
            {
                value = unchecked((sbyte)byteResult);
                return true;
            }

            if (TryReadNegativeFixInt(buffer, out value, out readSize))
            {
                return true;
            }

            if (TryReadFixInt8(buffer, out value, out readSize))
                return true;

            if (!TryReadPositiveFixInt(buffer, out byteResult, out readSize))
                return false;

            value = unchecked((sbyte)byteResult);
            return true;
        }
    }
}
