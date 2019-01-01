using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Text;

using static System.Buffers.Binary.BinaryPrimitives;
using static ProGaudi.MsgPack.DataCodes;

namespace ProGaudi.MsgPack
{
    /// <summary>
    /// Methods for working with strings
    /// </summary>
    public static partial class MsgPackSpec
    {
        public static readonly Encoding DefaultEncoding = new UTF8Encoding(false, true);

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
        /// <param name="encoder">Encoder. If not passed, utf8 without bom will be used.</param>
        /// <returns>Count of bytes, written to <paramref name="buffer"/>.</returns>
        public static int WriteFixString(Span<byte> buffer, ReadOnlySpan<char> chars, Encoder encoder = null)
        {
            if (chars.Length > DataLengths.FixStringMaxLength)
                return ThrowDataIsTooLarge(chars.Length, DataLengths.FixStringMaxLength, "string", FixStringMin, FixStringMax);

            var (success, length) = WriteString(chars, buffer.Slice(DataLengths.FixStringHeader), encoder);
            if (!success || length > DataLengths.FixStringMaxLength)
                return ThrowDataIsTooLarge(length, DataLengths.FixStringMaxLength, "string", FixStringMin, FixStringMax);

            var result = WriteFixStringHeader(buffer, (byte) length);

            return result + length;
        }

        /// <summary>
        /// Tries to write <paramref name="chars"/> into <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to write.</param>
        /// <param name="chars">String</param>
        /// <param name="wroteSize">Count of bytes, written to <paramref name="buffer"/>.</param>
        /// <param name="encoder">Encoder. If not passed, utf8 without bom will be used.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if:
        /// <list type="bullet">
        ///     <item><description><paramref name="buffer"/> is too small or</description></item>
        ///     <item><description><paramref name="chars"/> is greater <see cref="DataLengths.FixArrayMaxLength"/>.</description></item>
        /// </list>
        /// </returns>
        public static bool TryWriteFixString(Span<byte> buffer, ReadOnlySpan<char> chars, out int wroteSize, Encoder encoder = null)
        {
            wroteSize = 0;
            if (chars.Length > DataLengths.FixStringMaxLength) return false;
            if (chars.Length > buffer.Length + DataLengths.FixStringHeader) return false;

            var (success, length) = WriteString(chars, buffer.Slice(DataLengths.FixStringHeader), encoder);
            if (!success || length > DataLengths.FixStringMaxLength)
                return false;

            wroteSize = length + WriteFixStringHeader(buffer, (byte) length);

            return true;
        }

        /// <summary>
        /// Writes string (up to <see cref="byte.MaxValue"/>) into <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to write.</param>
        /// <param name="chars">String</param>
        /// <param name="encoder">Encoder. If not passed, utf8 without bom will be used.</param>
        /// <returns>Count of bytes, written to <paramref name="buffer"/>.</returns>
        public static int WriteString8(Span<byte> buffer, ReadOnlySpan<char> chars, Encoder encoder = null)
        {
            if (chars.Length > byte.MaxValue)
                return ThrowDataIsTooLarge(chars.Length, byte.MaxValue, "string", String8);

            var (success, length) = WriteString(chars, buffer.Slice(DataLengths.String8Header), encoder);
            if (!success || length > byte.MaxValue)
                return ThrowDataIsTooLarge(length, byte.MaxValue, "string", String8);

            var result = WriteString8Header(buffer, (byte) length);

            return result + length;
        }

        /// <summary>
        /// Tries to write <paramref name="chars"/> into <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to write.</param>
        /// <param name="chars">String</param>
        /// <param name="wroteSize">Count of bytes, written to <paramref name="buffer"/>.</param>
        /// <param name="encoder">Encoder. If not passed, utf8 without bom will be used.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if:
        /// <list type="bullet">
        ///     <item><description><paramref name="buffer"/> is too small or</description></item>
        ///     <item><description><paramref name="chars"/> is greater <see cref="byte.MaxValue"/>.</description></item>
        /// </list>
        /// </returns>
        public static bool TryWriteString8(Span<byte> buffer, ReadOnlySpan<char> chars, out int wroteSize, Encoder encoder = null)
        {
            wroteSize = 0;
            if (chars.Length > byte.MaxValue) return false;
            if (chars.Length > buffer.Length + DataLengths.String8Header) return false;

            var (success, length) = WriteString(chars, buffer.Slice(DataLengths.String8Header), encoder);
            if (!success || length > byte.MaxValue)
                return false;

            wroteSize = length + WriteString8Header(buffer, (byte) length);

            return true;
        }

        /// <summary>
        /// Writes string (up to <see cref="ushort.MaxValue"/>) into <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to write.</param>
        /// <param name="chars">String</param>
        /// <param name="encoder">Encoding. If not passed, utf8 without bom will be used.</param>
        /// <returns>Count of bytes, written to <paramref name="buffer"/>.</returns>
        public static int WriteString16(Span<byte> buffer, ReadOnlySpan<char> chars, Encoder encoder = null)
        {
            if (chars.Length > ushort.MaxValue)
                return ThrowDataIsTooLarge(chars.Length, ushort.MaxValue, "string", String16);

            var (success, length) = WriteString(chars, buffer.Slice(DataLengths.String16Header), encoder);
            if (!success || length > ushort.MaxValue)
                return ThrowDataIsTooLarge(length, ushort.MaxValue, "string", String16);

            var result = WriteString16Header(buffer, (ushort) length);

            return result + length;
        }

        /// <summary>
        /// Tries to write <paramref name="chars"/> into <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to write.</param>
        /// <param name="chars">String</param>
        /// <param name="wroteSize">Count of bytes, written to <paramref name="buffer"/>.</param>
        /// <param name="encoder">Encoder. If not passed, utf8 without bom will be used.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if:
        /// <list type="bullet">
        ///     <item><description><paramref name="buffer"/> is too small or</description></item>
        ///     <item><description><paramref name="chars"/> is greater <see cref="ushort.MaxValue"/>.</description></item>
        /// </list>
        /// </returns>
        public static bool TryWriteString16(Span<byte> buffer, ReadOnlySpan<char> chars, out int wroteSize, Encoder encoder = null)
        {
            wroteSize = 0;
            if (chars.Length > ushort.MaxValue) return false;
            if (chars.Length > buffer.Length + DataLengths.String16Header) return false;

            var (success, length) = WriteString(chars, buffer.Slice(DataLengths.String16Header), encoder);
            if (!success || length > ushort.MaxValue)
                return false;

            wroteSize = length + WriteString16Header(buffer, (ushort) length);

            return true;
        }

        /// <summary>
        /// Writes string (up to <see cref="int.MaxValue"/>) into <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to write.</param>
        /// <param name="chars">String</param>
        /// <param name="encoder">Encoder. If not passed, utf8 without bom will be used.</param>
        /// <returns>Count of bytes, written to <paramref name="buffer"/>.</returns>
        public static int WriteString32(Span<byte> buffer, ReadOnlySpan<char> chars, Encoder encoder = null)
        {
            var (success, length) = WriteString(chars, buffer.Slice(DataLengths.String32Header), encoder);
            if (!success)
                return ThrowDataIsTooLarge(length, int.MaxValue, "string", String32);

            var result = WriteString32Header(buffer, (uint) length);

            return result + length;
        }

        /// <summary>
        /// Tries to write <paramref name="chars"/> into <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to write.</param>
        /// <param name="chars">String</param>
        /// <param name="wroteSize">Count of bytes, written to <paramref name="buffer"/>.</param>
        /// <param name="encoder">Encoder. If not passed, utf8 without bom will be used.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="buffer"/> is too small.
        /// </returns>
        public static bool TryWriteString32(Span<byte> buffer, ReadOnlySpan<char> chars, out int wroteSize, Encoder encoder = null)
        {
            wroteSize = 0;
            if (chars.Length > buffer.Length + DataLengths.String32Header) return false;

            var (success, length) = WriteString(chars, buffer.Slice(DataLengths.String16Header), encoder);
            wroteSize = length;
            if (!success)
                return false;

            wroteSize += WriteString32Header(buffer, (ushort) length);

            return true;
        }

        /// <summary>
        /// Writes string into <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to write.</param>
        /// <param name="chars">String</param>
        /// <param name="encoder">Encoder. If not passed, utf8 without bom will be used.</param>
        /// <returns>Count of bytes, written to <paramref name="buffer"/>.</returns>
        public static int WriteString(Span<byte> buffer, ReadOnlySpan<char> chars, Encoder encoder = null)
        {
            var length = GetPerThreadEncoder(encoder).GetByteCount(chars, true);
            if (length <= DataLengths.FixStringMaxLength)
            {
                return WriteFixString(buffer, chars, encoder);
            }

            if (length <= byte.MaxValue)
            {
                return WriteString8(buffer, chars, encoder);
            }

            if (length <= ushort.MaxValue)
            {
                return WriteString16(buffer, chars, encoder);
            }

            return WriteString32(buffer, chars, encoder);
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
        /// <param name="encoder">Encoder. If not passed, utf8 without bom will be used.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="buffer"/> is too small.
        /// </returns>
        public static bool TryWriteString(Span<byte> buffer, ReadOnlySpan<char> chars, out int wroteSize, Encoder encoder = null)
        {
            wroteSize = 0;
            if (chars.Length > buffer.Length) return false;

            var length = GetPerThreadEncoder(encoder).GetByteCount(chars, true);
            if (length <= DataLengths.FixStringMaxLength)
            {
                if (chars.Length > length + DataLengths.FixStringHeader) return false;
                if (TryWriteFixStringHeader(buffer, (byte) length, out wroteSize))
                {
                    var (success, actualLength) = WriteString(chars, buffer.Slice(wroteSize), encoder);
                    wroteSize += actualLength;
                    return success;
                }

                return false;
            }

            if (length <= byte.MaxValue)
            {
                if (chars.Length > length + DataLengths.String8Header) return false;
                if (TryWriteString8Header(buffer, (byte) length, out wroteSize))
                {
                    var (success, actualLength) = WriteString(chars, buffer.Slice(wroteSize), encoder);
                    wroteSize += actualLength;
                    return success;
                }

                return false;
            }

            if (length <= ushort.MaxValue)
            {
                if (chars.Length > length + DataLengths.String16Header) return false;
                if (TryWriteString16Header(buffer, (ushort) length, out wroteSize))
                {
                    var (success, actualLength) = WriteString(chars, buffer.Slice(wroteSize), encoder);
                    wroteSize += actualLength;
                    return success;
                }

                return false;
            }

            if (chars.Length > length + DataLengths.String32Header) return false;
            if (TryWriteString32Header(buffer, (uint) length, out wroteSize))
            {
                var (success, actualLength) = WriteString(chars, buffer.Slice(wroteSize), encoder);
                wroteSize += actualLength;
                return success;
            }

            return false;
        }

        /// <summary>
        /// Reads FixString from <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="buffer"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <param name="decoder">Decoder. If not passed, utf8 without bom will be used.</param>
        /// <returns>Returns string.</returns>
        public static string ReadFixString(ReadOnlySpan<byte> buffer, out int readSize, Decoder decoder = null)
        {
            var length = ReadFixStringHeader(buffer, out readSize);
            readSize += length;
            return ReadString(buffer.Slice(readSize, length), decoder);
        }

        /// <summary>
        /// Tries to read FixString from <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="value">Length of string.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="buffer"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <param name="decoder">Decoder. If not passed, utf8 without bom will be used.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="buffer"/> is too small.</returns>
        public static bool TryReadFixString(ReadOnlySpan<byte> buffer, out string value, out int readSize, Decoder decoder = null)
        {
            value = null;

            return TryReadFixStringHeader(buffer, out var length, out readSize)
                && TryReadStringImpl(buffer.Slice(readSize, length), out value, ref readSize, decoder);
        }

        /// <summary>
        /// Reads <see cref="String8"/> from <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="buffer"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <param name="decoder">Decoder. If not passed, utf8 without bom will be used.</param>
        /// <returns>Returns string.</returns>
        public static string ReadString8(ReadOnlySpan<byte> buffer, out int readSize, Decoder decoder = null)
        {
            var length = ReadString8Header(buffer, out readSize);
            readSize += length;
            return ReadString(buffer.Slice(readSize, length), decoder);
        }

        /// <summary>
        /// Tries to read <see cref="String8"/> from <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="value">Length of string.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="buffer"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <param name="decoder">Decoder. If not passed, utf8 without bom will be used.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="buffer"/> is too small.</returns>
        public static bool TryReadString8(ReadOnlySpan<byte> buffer, out string value, out int readSize, Decoder decoder = null)
        {
            value = null;

            return TryReadString8Header(buffer, out var length, out readSize)
                && TryReadStringImpl(buffer.Slice(readSize, length), out value, ref readSize, decoder);
        }

        /// <summary>
        /// Reads <see cref="String16"/> from <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="buffer"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <param name="decoder">Decoder. If not passed, utf8 without bom will be used.</param>
        /// <returns>Returns string.</returns>
        public static string ReadString16(ReadOnlySpan<byte> buffer, out int readSize, Decoder decoder = null)
        {
            var length = ReadString16Header(buffer, out readSize);
            readSize += length;
            return ReadString(buffer.Slice(readSize, length), decoder);
        }

        /// <summary>
        /// Tries to read <see cref="String16"/> from <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="value">Length of string.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="buffer"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <param name="decoder">Decoder. If not passed, utf8 without bom will be used.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="buffer"/> is too small.</returns>
        public static bool TryReadString16(ReadOnlySpan<byte> buffer, out string value, out int readSize, Decoder decoder = null)
        {
            value = null;

            return TryReadString16Header(buffer, out var length, out readSize)
                   && TryReadStringImpl(buffer.Slice(readSize, length), out value, ref readSize, decoder);
        }

        /// <summary>
        /// Reads <see cref="String32"/> from <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="buffer"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <param name="decoder">Decoder. If not passed, utf8 without bom will be used.</param>
        /// <returns>Returns string.</returns>
        public static string ReadString32(ReadOnlySpan<byte> buffer, out int readSize, Decoder decoder = null)
        {
            var length = ReadString32Header(buffer, out readSize);
            if (length > int.MaxValue) ThrowDataIsTooLarge(length);
            var intLength = (int)length;
            readSize += intLength;
            return ReadString(buffer.Slice(readSize, intLength), decoder);
        }

        /// <summary>
        /// Tries to read <see cref="String32"/> from <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="value">Length of string.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="buffer"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <param name="decoder">Decoder. If not passed, utf8 without bom will be used.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="buffer"/> is too small.</returns>
        public static bool TryReadString32(ReadOnlySpan<byte> buffer, out string value, out int readSize, Decoder decoder = null)
        {
            value = null;

            return TryReadString32Header(buffer, out var length, out readSize)
                   && length <= int.MaxValue
                   && TryReadStringImpl(buffer.Slice(readSize, (int) length), out value, ref readSize, decoder);
        }

        /// <summary>
        /// Reads string from <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="buffer"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <param name="decoder">Decoder. If not passed, utf8 without bom will be used.</param>
        /// <returns>Returns string.</returns>
        public static string ReadString(ReadOnlySpan<byte> buffer, out int readSize, Decoder decoder = null)
        {
            var length = ReadStringHeader(buffer, out var offset);
            readSize = offset + length;
            return ReadString(buffer.Slice(offset, length), decoder);
        }

        /// <summary>
        /// Tries to read string from <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="value">Length of string.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="buffer"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <param name="decoder">Decoder. If not passed, utf8 without bom will be used.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="buffer"/> is too small.</returns>
        public static bool TryReadString(ReadOnlySpan<byte> buffer, out string value, out int readSize, Decoder decoder = null)
        {
            return TryReadString8(buffer, out value, out readSize, decoder)
                || TryReadFixString(buffer, out value, out readSize, decoder)
                || TryReadString16(buffer, out value, out readSize, decoder)
                || TryReadString32(buffer, out value, out readSize, decoder);
        }

        private static (bool success, int bytesUsed) WriteString(ReadOnlySpan<char> str, Span<byte> buffer, Encoder encoder)
        {
            if (str.Length == 0)
            {
                return (true, 0);
            }
            
            encoder = GetPerThreadEncoder(encoder);
            encoder.Convert(str, buffer, true, out var charsUsed, out var bytesUsed, out var completed);
            return (completed && charsUsed == str.Length, bytesUsed);
        }

        private static string ReadString(ReadOnlySpan<byte> buffer, Decoder decoder)
        {
            var safeDecoder = GetPerThreadDecoder(decoder);
            using (var chars = MemoryPool<char>.Shared.Rent(buffer.Length))
            {
                var span = chars.Memory.Span;
                var length = safeDecoder.GetChars(buffer, span, true);
                return span.Slice(0, length).ToString();
            }
        }

        private static bool TryReadStringImpl(ReadOnlySpan<byte> buffer, out string value, ref int readSize, Decoder decoder)
        {
            value = ReadString(buffer, decoder);
            readSize += buffer.Length;
            return true;
        }

        [ThreadStatic]
        private static Encoder _perThreadEncoder;
        internal static Encoder GetPerThreadEncoder(Encoder nonDefault)
        {
            if (nonDefault != null)
            {
                nonDefault.Reset();
                return nonDefault;
            }

            var encoder = _perThreadEncoder;
            if (encoder == null)
            {
                _perThreadEncoder = encoder = DefaultEncoding.GetEncoder();
            }
            else
            {
                encoder.Reset();
            }
            return encoder;
        }

        [ThreadStatic]
        private static Decoder _perThreadDecoder;
        internal static Decoder GetPerThreadDecoder(Decoder nonDefault)
        {
            if (nonDefault != null)
            {
                nonDefault.Reset();
                return nonDefault;
            }

            var decoder = _perThreadDecoder;
            if (decoder == null)
            {
                _perThreadDecoder = decoder = DefaultEncoding.GetDecoder();
            }
            else
            {
                decoder.Reset();
            }
            return decoder;
        }

#if NETSTANDARD2_0 || NET45 || NET46
        private static unsafe void Convert(
            this Encoder encoder,
            ReadOnlySpan<char> chars,
            Span<byte> bytes,
            bool flush,
            out int charsUsed,
            out int bytesUsed,
            out bool completed)
        {
            fixed (char* charsPtr = chars)
            fixed (byte* bytesPtr = bytes)
                encoder.Convert(charsPtr, chars.Length, bytesPtr, bytes.Length, flush, out charsUsed, out bytesUsed, out completed);
        }

        private static unsafe int GetByteCount(this Encoder encoder, ReadOnlySpan<char> chars, bool flush)
        {
            if (chars.IsEmpty) return 0;

            fixed (char* charsPtr = chars)
            {
                return encoder.GetByteCount(charsPtr, chars.Length, flush);
            }
        }

        private static unsafe int GetChars(this Decoder decoder, ReadOnlySpan<byte> bytes, Span<char> chars, bool flush)
        {
            if (bytes.IsEmpty) return 0;

            fixed (byte* bytesPtr = bytes)
            fixed (char* charsPtr = chars)
            {
                return decoder.GetChars(bytesPtr, bytes.Length, charsPtr, chars.Length, flush);
            }
        }
#endif

#if NETSTANDARD1_4
        private static void Convert(
            this Encoder encoder,
            ReadOnlySpan<char> chars,
            Span<byte> bytes,
            bool flush,
            out int charsUsed,
            out int bytesUsed,
            out bool completed)
        {
            var (byteArray, charArray) = Allocate(bytes.Length, chars.Length);

            try
            {
                chars.CopyTo(charArray);
                encoder.Convert(charArray, 0, chars.Length, byteArray, 0, bytes.Length, flush, out charsUsed, out bytesUsed, out completed);
                var span = new Span<byte>(byteArray, 0, bytesUsed);
                span.CopyTo(bytes);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(byteArray);
                ArrayPool<char>.Shared.Return(charArray);
            }
        }

        private static int GetByteCount(this Encoder encoder, ReadOnlySpan<char> chars, bool flush)
        {
            if (chars.IsEmpty) return 0;

            var charArray = ArrayPool<char>.Shared.Rent(chars.Length);
            try
            {
                chars.CopyTo(charArray);
                return encoder.GetByteCount(charArray, 0, chars.Length, flush);
            }
            finally
            {
                ArrayPool<char>.Shared.Return(charArray);
            }
        }

        private static int GetChars(this Decoder decoder, ReadOnlySpan<byte> bytes, Span<char> chars, bool flush)
        {
            if (bytes.IsEmpty) return 0;

            var (byteArray, charArray) = Allocate(bytes.Length, chars.Length);
            try
            {
                bytes.CopyTo(byteArray);
                var length = decoder.GetChars(byteArray, 0, bytes.Length, charArray, 0, flush);
                var span = new Span<char>(charArray, 0, length);
                span.CopyTo(chars);
                return length;
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(byteArray);
                ArrayPool<char>.Shared.Return(charArray);
            }
        }

        private static (byte[], char[]) Allocate(int bytesLength, int charsLength)
        {
            var bytes = ArrayPool<byte>.Shared.Rent(bytesLength);
            char[] chars;
            try
            {
                chars = ArrayPool<char>.Shared.Rent(charsLength);
            }
            catch
            {
                ArrayPool<byte>.Shared.Return(bytes);
                throw;
            }

            return (bytes, chars);
        }
#endif
    }
}
