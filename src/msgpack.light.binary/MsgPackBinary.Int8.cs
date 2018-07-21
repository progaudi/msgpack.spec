using System;
using System.Runtime.CompilerServices;

namespace ProGaudi.MsgPack.Light
{
    /// <summary>
    /// Methods for working with signed int 8
    /// </summary>
    public static partial class MsgPackBinary
    {
        /// <summary>
        /// Write int8 <paramref name="value"/> into <paramref name="buffer"/>.
        /// </summary>
        /// <returns>Count of bytes, written to <paramref name="buffer"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteFixInt8(Span<byte> buffer, sbyte value)
        {
            buffer[0] = DataCodes.Int8;
            buffer[1] = unchecked((byte)value);
            return 2;
        }

        /// <summary>
        /// Tries to write int8 <paramref name="value"/> into <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to write.</param>
        /// <param name="value">Value to write</param>
        /// <param name="wroteSize">Count of bytes, written to <paramref name="buffer"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="buffer"/> is too small.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteFixInt8(Span<byte> buffer, sbyte value, out int wroteSize)
        {
            wroteSize = 2;
            if (buffer.Length < wroteSize) return false;
            buffer[0] = DataCodes.Int8;
            buffer[1] = unchecked((byte)value);
            return true;
        }

        /// <summary>
        /// Reads uint32 from <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to read from</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="buffer"/></param>
        /// <returns>Read value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte ReadFixInt8(ReadOnlySpan<byte> buffer, out int readSize)
        {
            readSize = 2;
            if (buffer[0] != DataCodes.Int8) throw WrongCodeException(buffer[0], DataCodes.Int8);
            return unchecked((sbyte)buffer[1]);
        }

        /// <summary>
        /// Tries to read from <paramref name="buffer"/>
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="value">Value, read from <paramref name="buffer"/>. If return value is false, value is unspecified.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="buffer"/>. If return value is false, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="buffer"/> is too small or <paramref name="buffer"/>[0] is not <see cref="DataCodes.Int8"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadFixInt8(ReadOnlySpan<byte> buffer, out sbyte value, out int readSize)
        {
            readSize = 2;
            value = default;
            if (buffer.Length < readSize) return false;
            var result = buffer[0] == DataCodes.Int8;
            value = result ? unchecked((sbyte)buffer[1]) : default;
            return result;
        }

        /// <summary>
        /// Write smallest possible representation of <paramref name="value"/> into <paramref name="buffer"/>.
        /// </summary>
        /// <remarks>See https://github.com/msgpack/msgpack/issues/164 on data code selection.</remarks>
        /// <returns>Count of bytes, written to <paramref name="buffer"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteInt8(Span<byte> buffer, sbyte value)
        {
            if (value >= 0) return WriteUInt8(buffer, (byte) value);
            if (value >= DataCodes.FixNegativeMinSByte) return WriteNegativeFixInt(buffer, value);
            return WriteFixInt8(buffer, value);
        }

        /// <summary>
        /// Tries to write smallest possible representation of <paramref name="value"/> into <paramref name="buffer"/>.
        /// </summary>
        /// <remarks>See https://github.com/msgpack/msgpack/issues/164 on data code selection.</remarks>
        /// <param name="buffer">Buffer to write.</param>
        /// <param name="value">Value to write</param>
        /// <param name="wroteSize">Count of bytes, written to <paramref name="buffer"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="buffer"/> is too small.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteInt8(Span<byte> buffer, sbyte value, out int wroteSize)
        {
            if (value >= 0) return TryWriteUInt8(buffer, (byte)value, out wroteSize);
            if (value >= DataCodes.FixNegativeMinSByte) return TryWriteNegativeFixInt(buffer, value, out wroteSize);
            return TryWriteFixInt8(buffer, value, out wroteSize);
        }

        /// <summary>
        /// Read <see cref="sbyte"/> values from <paramref name="buffer"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte ReadInt8(ReadOnlySpan<byte> buffer, out int readSize)
        {
            if (buffer.IsEmpty) throw CantReadEmptyBufferException();
            var code = buffer[0];

            switch (code)
            {
                case DataCodes.Int8:
                    return ReadFixInt8(buffer, out readSize);

                case DataCodes.UInt8:
                    var value = ReadFixUInt8(buffer, out readSize);
                    if (value > sbyte.MaxValue) throw ValueIsTooLargeException(value, short.MaxValue);
                    return unchecked((sbyte) value);
            }

            if (TryReadPositiveFixInt(buffer, out var positive, out readSize))
            {
                return unchecked((sbyte) positive);
            }

            if (TryReadNegativeFixInt(buffer, out var negative, out readSize))
            {
                return negative;
            }

            throw WrongIntCodeException(code, DataCodes.Int8, DataCodes.UInt8);
        }

        /// <summary>
        /// Tries to read <see cref="sbyte"/> value from <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="value">Value, read from <paramref name="buffer"/>. If return value is false, value is unspecified.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="buffer"/>. If return value is false, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="buffer"/> is too small or <paramref name="buffer"/>[0] is not ok.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadInt8(ReadOnlySpan<byte> buffer, out sbyte value, out int readSize)
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

            if (TryReadPositiveFixInt(buffer, out byteResult, out readSize))
            {
                value = unchecked((sbyte)byteResult);
                return true;
            }

            return false;
        }
    }
}
