using System;
using System.Runtime.CompilerServices;
using System.Text;

using static System.Buffers.Binary.BinaryPrimitives;
using static ProGaudi.MsgPack.DataCodes;

#if NET45 || NET46
using System.Buffers;
#endif

namespace ProGaudi.MsgPack
{
    /// <summary>
    /// Methods for working with strings
    /// </summary>
    public static partial class MsgPackSpec
    {
        private static readonly Encoding _defaultEncoding = new UTF8Encoding(false, true);

        /// <summary>
        /// Writes FixString header into <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to write. Ensure that is at least 1 byte long.</param>
        /// <param name="length">Length of string. Should be less or equal to <see cref="DataLengths.FixStringMaxLength"/>.</param>
        /// <returns>Count of bytes, written to <paramref name="buffer"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteFixStringHeader(Span<byte> buffer, byte length)
        {
            if (length > DataLengths.FixStringMaxLength)
                return ThrowWrongRangeCodeException(length, FixStringMin, FixStringMax);

            buffer[0] = (byte) (FixStringMin + length);
            return DataLengths.FixStringHeader;
        }

        /// <summary>
        /// Tries to write FixString header into <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to write.</param>
        /// <param name="length">Length of string. Should be less or equal to <see cref="DataLengths.FixArrayMaxLength"/>.</param>
        /// <param name="wroteSize">Count of bytes, written to <paramref name="buffer"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if:
        /// <list type="bullet">
        ///     <item><description><paramref name="buffer"/> is too small or</description></item>
        ///     <item><description><paramref name="length"/> is greater <see cref="DataLengths.FixArrayMaxLength"/>.</description></item>
        /// </list>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteFixStringHeader(Span<byte> buffer, byte length, out int wroteSize)
        {
            wroteSize = DataLengths.FixStringHeader;
            if (buffer.Length < wroteSize) return false;
            buffer[0] = (byte) (FixStringMin + length);
            return length < DataLengths.FixStringMaxLength;
        }

        /// <summary>
        /// Writes <see cref="String8"/> header into <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to read from. Ensure that is at least 2 bytes long.</param>
        /// <param name="length">Length of string. Should be less or equal to <see cref="byte.MaxValue"/>.</param>
        /// <returns>Count of bytes, written to <paramref name="buffer"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteString8Header(Span<byte> buffer, byte length)
        {
            buffer[1] = length;
            buffer[0] = String8;
            return DataLengths.String8Header;
        }

        /// <summary>
        /// Tries to write <see cref="String8"/> header into <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to write.</param>
        /// <param name="length">Length of string. Should be less or equal to <see cref="byte.MaxValue"/>.</param>
        /// <param name="wroteSize">Count of bytes, written to <paramref name="buffer"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if:
        /// <list type="bullet">
        ///     <item><description><paramref name="buffer"/> is too small or</description></item>
        ///     <item><description><paramref name="length"/> is greater <see cref="byte.MaxValue"/>.</description></item>
        /// </list>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteString8Header(Span<byte> buffer, byte length, out int wroteSize)
        {
            wroteSize = DataLengths.String8Header;
            if (buffer.Length < wroteSize) return false;
            buffer[1] = length;
            buffer[0] = String8;
            return true;
        }

        /// <summary>
        /// Writes <see cref="String16"/> header into <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to read from. Ensure that is at least 3 byte long.</param>
        /// <param name="length">Length of string. Should be less or equal to <see cref="ushort.MaxValue"/>.</param>
        /// <returns>Count of bytes, written to <paramref name="buffer"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteString16Header(Span<byte> buffer, ushort length)
        {
            WriteUInt16BigEndian(buffer.Slice(1), length);
            buffer[0] = String16;
            return DataLengths.String16Header;
        }

        /// <summary>
        /// Tries to write <see cref="String16"/> header into <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to write.</param>
        /// <param name="length">Length of string. Should be less or equal to <see cref="ushort.MaxValue"/>.</param>
        /// <param name="wroteSize">Count of bytes, written to <paramref name="buffer"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if:
        /// <list type="bullet">
        ///     <item><description><paramref name="buffer"/> is too small or</description></item>
        ///     <item><description><paramref name="length"/> is greater <see cref="ushort.MaxValue"/>.</description></item>
        /// </list>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteString16Header(Span<byte> buffer, ushort length, out int wroteSize)
        {
            wroteSize = DataLengths.String16Header;
            if (buffer.Length < wroteSize) return false;
            buffer[0] = String16;
            return TryWriteUInt16BigEndian(buffer.Slice(1), length);
        }

        /// <summary>
        /// Writes <see cref="String32"/> header into <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to read from. Ensure that is at least 5 byte long.</param>
        /// <param name="length">Length of string.</param>
        /// <returns>Count of bytes, written to <paramref name="buffer"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteString32Header(Span<byte> buffer, uint length)
        {
            WriteUInt32BigEndian(buffer.Slice(1), length);
            buffer[0] = String32;
            return DataLengths.String32Header;
        }

        /// <summary>
        /// Tries to write <see cref="String32"/> header into <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to write.</param>
        /// <param name="length">Length of string.</param>
        /// <param name="wroteSize">Count of bytes, written to <paramref name="buffer"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="buffer"/> is too small.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteString32Header(Span<byte> buffer, uint length, out int wroteSize)
        {
            wroteSize = DataLengths.String32Header;
            if (buffer.Length < wroteSize) return false;
            buffer[0] = String32;
            return TryWriteUInt32BigEndian(buffer.Slice(1), length);
        }

        /// <summary>
        /// Reads FixString header from <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="buffer"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <returns>Returns length of string.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ReadFixStringHeader(ReadOnlySpan<byte> buffer, out int readSize)
        {
            readSize = DataLengths.FixStringHeader;
            var value = buffer[0];
            if (FixStringMin <= value && value <= FixStringMax)
            {
                return (byte) (value - FixStringMin);
            }

            return ThrowWrongRangeCodeException(value, FixStringMin, FixStringMax);
        }

        /// <summary>
        /// Tries to read FixString header from <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="value">Length of string. If return value is <c>false</c>, value is unspecified.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="buffer"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="buffer"/> is too small.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadFixStringHeader(ReadOnlySpan<byte> buffer, out byte value, out int readSize)
        {
            readSize = DataLengths.FixStringHeader;
            value = default;
            if (buffer.Length < readSize) return false;
            value = buffer[0];
            if (value < FixStringMin || FixStringMax < value)
                return false;
            value -= FixStringMin;
            return true;
        }

        /// <summary>
        /// Reads <see cref="String8"/> header from <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="buffer"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <returns>Returns length of <see cref="String8"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ReadString8Header(ReadOnlySpan<byte> buffer, out int readSize)
        {
            readSize = DataLengths.String8Header;
            if (buffer[0] != String8) ThrowWrongCodeException(buffer[0], String8);
            return buffer[1];
        }

        /// <summary>
        /// Tries to read <see cref="String8"/> header from <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="value">Length of string. If return value is <c>false</c>, value is unspecified.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="buffer"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="buffer"/> is too small.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadString8Header(ReadOnlySpan<byte> buffer, out byte value, out int readSize)
        {
            readSize = DataLengths.String8Header;
            value = default;
            if (buffer.Length < readSize) return false;
            value = buffer[1];
            return buffer[0] == String8;
        }

        /// <summary>
        /// Reads <see cref="String16"/> header from <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="buffer"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <returns>Returns length of <see cref="String16"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ReadString16Header(ReadOnlySpan<byte> buffer, out int readSize)
        {
            readSize = DataLengths.String16Header;
            if (buffer[0] != String16) ThrowWrongCodeException(buffer[0], String16);
            return ReadUInt16BigEndian(buffer.Slice(1));
        }

        /// <summary>
        /// Tries to read <see cref="String16"/> header from <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="value">Length of string. If return value is <c>false</c>, value is unspecified.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="buffer"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="buffer"/> is too small.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadString16Header(ReadOnlySpan<byte> buffer, out ushort value, out int readSize)
        {
            readSize = DataLengths.String16Header;
            value = default;
            if (buffer.Length < readSize) return false;
            return TryReadUInt16BigEndian(buffer.Slice(1), out value) && buffer[0] == String16;
        }

        /// <summary>
        /// Reads <see cref="String32"/> header from <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="buffer"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <returns>Returns length of <see cref="String32"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ReadString32Header(ReadOnlySpan<byte> buffer, out int readSize)
        {
            readSize = DataLengths.String32Header;
            if (buffer[0] != String32) ThrowWrongCodeException(buffer[0], String32);
            return ReadUInt32BigEndian(buffer.Slice(1));
        }

        /// <summary>
        /// Tries to read <see cref="String32"/> header from <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="value">Length of string. If return value is <c>false</c>, value is unspecified.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="buffer"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="buffer"/> is too small.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadString32Header(ReadOnlySpan<byte> buffer, out uint value, out int readSize)
        {
            readSize = DataLengths.String32Header;
            value = default;
            if (buffer.Length < readSize) return false;
            return TryReadUInt32BigEndian(buffer.Slice(1), out value) && buffer[0] == String32;
        }

        /// <summary>
        /// Writes smallest header into <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to write.</param>
        /// <param name="length">Length of string.</param>
        /// <returns>Count of bytes, written to <paramref name="buffer"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteStringHeader(Span<byte> buffer, int length)
        {
            if (length < 0)
            {
                return 0;
            }

            if (length <= DataLengths.FixStringMaxLength)
            {
                return WriteFixStringHeader(buffer, (byte) length);
            }

            if (length <= byte.MaxValue)
            {
                return WriteString8Header(buffer, (byte) length);
            }

            if (length <= ushort.MaxValue)
            {
                return WriteString16Header(buffer, (ushort) length);
            }

            return WriteString32Header(buffer, (uint) length);
        }

        /// <summary>
        /// Tries to write string into <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to write.</param>
        /// <param name="length">Length of string.</param>
        /// <param name="wroteSize">Count of bytes, written to <paramref name="buffer"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="buffer"/> is too small.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteStringHeader(Span<byte> buffer, int length, out int wroteSize)
        {
            if (length < 0)
            {
                wroteSize = 0;
                return false;
            }

            if (length <= DataLengths.FixStringMaxLength)
            {
                return TryWriteFixStringHeader(buffer, (byte) length, out wroteSize);
            }

            if (length <= byte.MaxValue)
            {
                return TryWriteString8Header(buffer, (byte) length, out wroteSize);
            }

            if (length <= ushort.MaxValue)
            {
                return TryWriteString16Header(buffer, (ushort) length, out wroteSize);
            }

            return TryWriteString32Header(buffer, (uint) length, out wroteSize);
        }

        /// <summary>
        /// Reads string header from <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="buffer"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <returns>Returns length of string.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadStringHeader(ReadOnlySpan<byte> buffer, out int readSize)
        {
            switch (buffer[0])
            {
                case String8:
                    return ReadString8Header(buffer, out readSize);
                case String16:
                    return ReadString16Header(buffer, out readSize);
                case String32:
                    var uint32Value = ReadString32Header(buffer, out readSize);
                    if (uint32Value > int.MaxValue) ThrowDataIsTooLarge(uint32Value);
                    return (int) uint32Value;
            }

            return ReadFixStringHeader(buffer, out readSize);
        }

        /// <summary>
        /// Tries to read string header from <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="value">Length of string.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="buffer"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="buffer"/> is too small.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadStringHeader(ReadOnlySpan<byte> buffer, out int value, out int readSize)
        {
            if (TryReadFixStringHeader(buffer, out var byteValue, out readSize))
            {
                value = byteValue;
                return true;
            }

            if (TryReadString8Header(buffer, out byteValue, out readSize))
            {
                value = byteValue;
                return true;
            }

            if (TryReadString16Header(buffer, out var uint16Value, out readSize))
            {
                value = uint16Value;
                return true;
            }

            if (TryReadString32Header(buffer, out var uint32Value, out readSize))
            {
                // .net array size limitation
                // https://blogs.msdn.microsoft.com/joshwil/2005/08/10/bigarrayt-getting-around-the-2gb-array-size-limit/
                if (uint32Value <= int.MaxValue)
                {
                    value = (int)uint32Value;
                    return true;
                }
            }

            value = 0;
            return false;
        }

        /// <summary>
        /// Writes small string (up to <see cref="DataLengths.FixStringMaxLength"/>) into <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to write.</param>
        /// <param name="chars">String</param>
        /// <param name="encoding">Encoding. If not passed, utf8 without bom will be used.</param>
        /// <returns>Count of bytes, written to <paramref name="buffer"/>.</returns>
        public static int WriteFixString(Span<byte> buffer, ReadOnlySpan<char> chars, Encoding encoding = null)
        {
            if (chars.Length > DataLengths.FixStringMaxLength)
                return ThrowDataIsTooLarge(chars.Length, DataLengths.FixStringMaxLength, "string", FixStringMin, FixStringMax);

            var length = (encoding ?? _defaultEncoding).GetBytes(chars, buffer.Slice(1));

            var result = WriteFixStringHeader(buffer, (byte) length);

            return result + length;
        }

        /// <summary>
        /// Tries to write <paramref name="chars"/> into <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to write.</param>
        /// <param name="chars">String</param>
        /// <param name="wroteSize">Count of bytes, written to <paramref name="buffer"/>.</param>
        /// <param name="encoding">Encoding. If not passed, utf8 without bom will be used.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if:
        /// <list type="bullet">
        ///     <item><description><paramref name="buffer"/> is too small or</description></item>
        ///     <item><description><paramref name="chars"/> is greater <see cref="DataLengths.FixArrayMaxLength"/>.</description></item>
        /// </list>
        /// </returns>
        public static bool TryWriteFixString(Span<byte> buffer, ReadOnlySpan<char> chars, out int wroteSize, Encoding encoding = null)
        {
            wroteSize = 0;
            if (chars.Length > DataLengths.FixStringMaxLength) return false;
            if (chars.Length > buffer.Length + 1) return false;

            var length = (encoding ?? _defaultEncoding).GetByteCount(chars);
            if (length > DataLengths.FixStringMaxLength)
                return false;

            if (!TryWriteFixStringHeader(buffer, (byte) length, out wroteSize)) return false;

            var stringBuffer = buffer.Slice(wroteSize);
            if (length > stringBuffer.Length) return false;

            wroteSize += (encoding ?? _defaultEncoding).GetBytes(chars, stringBuffer);

            return true;
        }

        /// <summary>
        /// Writes string (up to <see cref="byte.MaxValue"/>) into <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to write.</param>
        /// <param name="chars">String</param>
        /// <param name="encoding">Encoding. If not passed, utf8 without bom will be used.</param>
        /// <returns>Count of bytes, written to <paramref name="buffer"/>.</returns>
        public static int WriteString8(Span<byte> buffer, ReadOnlySpan<char> chars, Encoding encoding = null)
        {
            if (chars.Length > byte.MaxValue)
                return ThrowDataIsTooLarge(chars.Length, byte.MaxValue, "string", String8);

            var length = (encoding ?? _defaultEncoding).GetBytes(chars, buffer.Slice(2));
            if (length > byte.MaxValue)
                return ThrowDataIsTooLarge(chars.Length, byte.MaxValue, "string", String8);

            var result = WriteString8Header(buffer, (byte) length);

            return result + length;
        }

        /// <summary>
        /// Tries to write <paramref name="chars"/> into <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to write.</param>
        /// <param name="chars">String</param>
        /// <param name="wroteSize">Count of bytes, written to <paramref name="buffer"/>.</param>
        /// <param name="encoding">Encoding. If not passed, utf8 without bom will be used.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if:
        /// <list type="bullet">
        ///     <item><description><paramref name="buffer"/> is too small or</description></item>
        ///     <item><description><paramref name="chars"/> is greater <see cref="byte.MaxValue"/>.</description></item>
        /// </list>
        /// </returns>
        public static bool TryWriteString8(Span<byte> buffer, ReadOnlySpan<char> chars, out int wroteSize, Encoding encoding = null)
        {
            wroteSize = 0;
            if (chars.Length > byte.MaxValue) return false;
            if (chars.Length > buffer.Length + DataLengths.String8Header) return false;

            var length = (encoding ?? _defaultEncoding).GetByteCount(chars);
            if (!TryWriteString8Header(buffer, (byte) length, out wroteSize)) return false;

            var stringBuffer = buffer.Slice(wroteSize);
            if (length > stringBuffer.Length) return false;

            wroteSize += (encoding ?? _defaultEncoding).GetBytes(chars, stringBuffer);

            return true;
        }

        /// <summary>
        /// Writes string (up to <see cref="ushort.MaxValue"/>) into <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to write.</param>
        /// <param name="chars">String</param>
        /// <param name="encoding">Encoding. If not passed, utf8 without bom will be used.</param>
        /// <returns>Count of bytes, written to <paramref name="buffer"/>.</returns>
        public static int WriteString16(Span<byte> buffer, ReadOnlySpan<char> chars, Encoding encoding = null)
        {
            if (chars.Length > ushort.MaxValue)
                return ThrowDataIsTooLarge(chars.Length, ushort.MaxValue, "string", String16);

            var length = (encoding ?? _defaultEncoding).GetBytes(chars, buffer.Slice(2));
            if (length > ushort.MaxValue)
                return ThrowDataIsTooLarge(chars.Length, ushort.MaxValue, "string", String16);

            var result = WriteString16Header(buffer, (ushort) length);

            return result + length;
        }

        /// <summary>
        /// Tries to write <paramref name="chars"/> into <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to write.</param>
        /// <param name="chars">String</param>
        /// <param name="wroteSize">Count of bytes, written to <paramref name="buffer"/>.</param>
        /// <param name="encoding">Encoding. If not passed, utf8 without bom will be used.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if:
        /// <list type="bullet">
        ///     <item><description><paramref name="buffer"/> is too small or</description></item>
        ///     <item><description><paramref name="chars"/> is greater <see cref="ushort.MaxValue"/>.</description></item>
        /// </list>
        /// </returns>
        public static bool TryWriteString16(Span<byte> buffer, ReadOnlySpan<char> chars, out int wroteSize, Encoding encoding = null)
        {
            wroteSize = 0;
            if (chars.Length > ushort.MaxValue) return false;
            if (chars.Length > buffer.Length + DataLengths.String16Header) return false;

            var length = (encoding ?? _defaultEncoding).GetByteCount(chars);
            if (length > ushort.MaxValue) return false;
            if (!TryWriteString16Header(buffer, (ushort) length, out wroteSize)) return false;

            var stringBuffer = buffer.Slice(wroteSize);
            if (length > stringBuffer.Length) return false;

            wroteSize += (encoding ?? _defaultEncoding).GetBytes(chars, stringBuffer);

            return true;
        }

        /// <summary>
        /// Writes string (up to <see cref="int.MaxValue"/>) into <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to write.</param>
        /// <param name="chars">String</param>
        /// <param name="encoding">Encoding. If not passed, utf8 without bom will be used.</param>
        /// <returns>Count of bytes, written to <paramref name="buffer"/>.</returns>
        public static int WriteString32(Span<byte> buffer, ReadOnlySpan<char> chars, Encoding encoding = null)
        {
            var length = (encoding ?? _defaultEncoding).GetBytes(chars, buffer.Slice(2));

            var result = WriteString32Header(buffer, (uint) length);

            return result + length;
        }

        /// <summary>
        /// Tries to write <paramref name="chars"/> into <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to write.</param>
        /// <param name="chars">String</param>
        /// <param name="wroteSize">Count of bytes, written to <paramref name="buffer"/>.</param>
        /// <param name="encoding">Encoding. If not passed, utf8 without bom will be used.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="buffer"/> is too small.
        /// </returns>
        public static bool TryWriteString32(Span<byte> buffer, ReadOnlySpan<char> chars, out int wroteSize, Encoding encoding = null)
        {
            wroteSize = 0;
            if (chars.Length > buffer.Length + DataLengths.String32Header) return false;

            var length = (encoding ?? _defaultEncoding).GetByteCount(chars);
            if (!TryWriteString32Header(buffer, (uint) length, out wroteSize)) return false;

            var stringBuffer = buffer.Slice(wroteSize);
            if (length > stringBuffer.Length) return false;

            wroteSize += (encoding ?? _defaultEncoding).GetBytes(chars, stringBuffer);

            return true;
        }

        /// <summary>
        /// Writes string into <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to write.</param>
        /// <param name="chars">String</param>
        /// <param name="encoding">Encoding. If not passed, utf8 without bom will be used.</param>
        /// <returns>Count of bytes, written to <paramref name="buffer"/>.</returns>
        public static int WriteString(Span<byte> buffer, ReadOnlySpan<char> chars, Encoding encoding = null)
        {
            var length = (encoding ?? _defaultEncoding).GetByteCount(chars);
            if (length <= DataLengths.FixStringMaxLength)
            {
                return WriteFixString(buffer, chars, encoding);
            }

            if (length <= byte.MaxValue)
            {
                return WriteString8(buffer, chars, encoding);
            }

            if (length <= ushort.MaxValue)
            {
                return WriteString16(buffer, chars, encoding);
            }

            return WriteString32(buffer, chars, encoding);
        }

        /// <summary>
        /// Tries to write <paramref name="chars"/> into <paramref name="buffer"/>.
        /// </summary>
        /// <remarks>
        /// This method is copy-pasted from methods above, because GetByteCount can be slow as hell.
        /// </remarks>
        /// <param name="buffer">Buffer to write.</param>
        /// <param name="chars">String</param>
        /// <param name="wroteSize">Count of bytes, written to <paramref name="buffer"/>.</param>
        /// <param name="encoding">Encoding. If not passed, utf8 without bom will be used.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="buffer"/> is too small.
        /// </returns>
        public static bool TryWriteString(Span<byte> buffer, ReadOnlySpan<char> chars, out int wroteSize, Encoding encoding = null)
        {
            wroteSize = 0;
            if (chars.Length > buffer.Length) return false;

            var length = (encoding ?? _defaultEncoding).GetByteCount(chars);
            if (length <= DataLengths.FixStringMaxLength)
            {
                if (chars.Length > length + DataLengths.FixStringHeader) return false;
                if (TryWriteFixStringHeader(buffer, (byte) length, out wroteSize))
                {
                    wroteSize += (encoding ?? _defaultEncoding).GetBytes(chars, buffer.Slice(wroteSize));
                    return true;
                }

                return false;
            }

            if (length <= byte.MaxValue)
            {
                if (chars.Length > length + DataLengths.String8Header) return false;
                if (TryWriteString8Header(buffer, (byte) length, out wroteSize))
                {
                    wroteSize += (encoding ?? _defaultEncoding).GetBytes(chars, buffer.Slice(wroteSize));
                    return true;
                }

                return false;
            }

            if (length <= ushort.MaxValue)
            {
                if (chars.Length > length + DataLengths.String16Header) return false;
                if (TryWriteString16Header(buffer, (ushort) length, out wroteSize))
                {
                    wroteSize += (encoding ?? _defaultEncoding).GetBytes(chars, buffer.Slice(wroteSize));
                    return true;
                }

                return false;
            }

            if (chars.Length > length + DataLengths.String32Header) return false;
            if (TryWriteString32Header(buffer, (uint) length, out wroteSize))
            {
                wroteSize += (encoding ?? _defaultEncoding).GetBytes(chars, buffer.Slice(wroteSize));
                return true;
            }

            return false;
        }

        /// <summary>
        /// Reads FixString from <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="buffer"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <param name="encoding">Encoding. If not passed, utf8 without bom will be used.</param>
        /// <returns>Returns string.</returns>
        public static string ReadFixString(ReadOnlySpan<byte> buffer, out int readSize, Encoding encoding = null)
        {
            var length = ReadFixStringHeader(buffer, out readSize);
            return ReadString(buffer, length, ref readSize, encoding);
        }

        /// <summary>
        /// Tries to read FixString from <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="value">Length of string.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="buffer"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <param name="encoding">Encoding. If not passed, utf8 without bom will be used.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="buffer"/> is too small.</returns>
        public static bool TryReadFixString(ReadOnlySpan<byte> buffer, out string value, out int readSize, Encoding encoding = null)
        {
            value = null;

            return TryReadFixStringHeader(buffer, out var length, out readSize)
                && TryReadStringImpl(buffer.Slice(readSize, length), out value, ref readSize, encoding);
        }

        /// <summary>
        /// Reads <see cref="String8"/> from <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="buffer"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <param name="encoding">Encoding. If not passed, utf8 without bom will be used.</param>
        /// <returns>Returns string.</returns>
        public static string ReadString8(ReadOnlySpan<byte> buffer, out int readSize, Encoding encoding = null)
        {
            var length = ReadString8Header(buffer, out readSize);
            return ReadString(buffer, length, ref readSize, encoding);
        }

        /// <summary>
        /// Tries to read <see cref="String8"/> from <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="value">Length of string.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="buffer"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <param name="encoding">Encoding. If not passed, utf8 without bom will be used.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="buffer"/> is too small.</returns>
        public static bool TryReadString8(ReadOnlySpan<byte> buffer, out string value, out int readSize, Encoding encoding = null)
        {
            value = null;

            return TryReadString8Header(buffer, out var length, out readSize)
                && TryReadStringImpl(buffer.Slice(readSize, length), out value, ref readSize, encoding);
        }

        /// <summary>
        /// Reads <see cref="String16"/> from <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="buffer"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <param name="encoding">Encoding. If not passed, utf8 without bom will be used.</param>
        /// <returns>Returns string.</returns>
        public static string ReadString16(ReadOnlySpan<byte> buffer, out int readSize, Encoding encoding = null)
        {
            var length = ReadString16Header(buffer, out readSize);
            return ReadString(buffer, length, ref readSize, encoding);
        }

        /// <summary>
        /// Tries to read <see cref="String16"/> from <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="value">Length of string.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="buffer"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <param name="encoding">Encoding. If not passed, utf8 without bom will be used.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="buffer"/> is too small.</returns>
        public static bool TryReadString16(ReadOnlySpan<byte> buffer, out string value, out int readSize, Encoding encoding = null)
        {
            value = null;

            return TryReadString16Header(buffer, out var length, out readSize)
                   && TryReadStringImpl(buffer.Slice(readSize, length), out value, ref readSize, encoding);
        }

        /// <summary>
        /// Reads <see cref="String32"/> from <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="buffer"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <param name="encoding">Encoding. If not passed, utf8 without bom will be used.</param>
        /// <returns>Returns string.</returns>
        public static string ReadString32(ReadOnlySpan<byte> buffer, out int readSize, Encoding encoding = null)
        {
            var length = ReadString32Header(buffer, out readSize);
            if (length > int.MaxValue) ThrowDataIsTooLarge(length);
            return ReadString(buffer, (int) length, ref readSize, encoding);
        }

        /// <summary>
        /// Tries to read <see cref="String32"/> from <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="value">Length of string.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="buffer"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <param name="encoding">Encoding. If not passed, utf8 without bom will be used.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="buffer"/> is too small.</returns>
        public static bool TryReadString32(ReadOnlySpan<byte> buffer, out string value, out int readSize, Encoding encoding = null)
        {
            value = null;

            return TryReadString32Header(buffer, out var length, out readSize)
                   && length <= int.MaxValue
                   && TryReadStringImpl(buffer.Slice(readSize, (int) length), out value, ref readSize, encoding);
        }

        /// <summary>
        /// Reads string from <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="buffer"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <param name="encoding">Encoding. If not passed, utf8 without bom will be used.</param>
        /// <returns>Returns string.</returns>
        public static string ReadString(ReadOnlySpan<byte> buffer, out int readSize, Encoding encoding = null)
        {
            var length = ReadStringHeader(buffer, out readSize);
            return ReadString(buffer, length, ref readSize, encoding);
        }

        /// <summary>
        /// Tries to read string from <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="value">Length of string.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="buffer"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <param name="encoding">Encoding. If not passed, utf8 without bom will be used.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="buffer"/> is too small.</returns>
        public static bool TryReadString(ReadOnlySpan<byte> buffer, out string value, out int readSize, Encoding encoding = null)
        {
            return TryReadString8(buffer, out value, out readSize, encoding)
                || TryReadFixString(buffer, out value, out readSize, encoding)
                || TryReadString16(buffer, out value, out readSize, encoding)
                || TryReadString32(buffer, out value, out readSize, encoding);
        }

        private static string ReadString(ReadOnlySpan<byte> buffer, int length, ref int readSize, Encoding encoding)
        {
            var result = (encoding ?? _defaultEncoding).GetString(buffer.Slice(readSize, length));
            readSize += length;
            return result;
        }

        private static bool TryReadStringImpl(ReadOnlySpan<byte> buffer, out string value, ref int readSize, Encoding encoding)
        {
            value = (encoding ?? _defaultEncoding).GetString(buffer);
            readSize += buffer.Length;
            return true;
        }

#if NETSTANDARD2_0 || NETSTANDARD1_4
        private static unsafe int GetBytes(this Encoding encoding, ReadOnlySpan<char> chars, Span<byte> bytes)
        {
            if (chars.IsEmpty) return 0;

            fixed (char* charsPtr = &chars.GetPinnableReference())
            fixed (byte* bytesPtr = &bytes.GetPinnableReference())
            {
                return encoding.GetBytes(charsPtr, chars.Length, bytesPtr, bytes.Length);
            }
        }

        private static unsafe int GetByteCount(this Encoding encoding, ReadOnlySpan<char> chars)
        {
            if (chars.IsEmpty) return 0;

            fixed (char* charsPtr = &chars.GetPinnableReference())
            {
                return encoding.GetByteCount(charsPtr, chars.Length);
            }
        }

        private static unsafe string GetString(this Encoding encoding, ReadOnlySpan<byte> bytes)
        {
            if (bytes.IsEmpty) return string.Empty;

            fixed (byte* bytesPtr = &bytes.GetPinnableReference())
            {
                return encoding.GetString(bytesPtr, bytes.Length);
            }
        }
#endif

#if NET45 || NET46
        private static int GetBytes(this Encoding encoding, ReadOnlySpan<char> chars, Span<byte> bytes)
        {
            if (chars.IsEmpty) return 0;

            var array = ArrayPool<byte>.Shared.Rent(bytes.Length);
            try
            {
                var result = encoding.GetBytes(chars.ToArray(), 0, chars.Length, array, 0);
                new Span<byte>(array, 0, result).CopyTo(bytes);
                return result;
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(array);
            }
        }

        private static unsafe int GetByteCount(this Encoding encoding, ReadOnlySpan<char> chars)
        {
            if (chars.IsEmpty) return 0;

            fixed (char* charsPtr = &chars.GetPinnableReference())
            {
                return encoding.GetByteCount(charsPtr, chars.Length);
            }
        }

        private static string GetString(this Encoding encoding, ReadOnlySpan<byte> bytes)
        {
            if (bytes.IsEmpty) return string.Empty;

            var array = ArrayPool<byte>.Shared.Rent(bytes.Length);

            try
            {
                bytes.CopyTo(array);
                return encoding.GetString(array, 0, bytes.Length);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(array);
            }
        }
#endif
    }
}
