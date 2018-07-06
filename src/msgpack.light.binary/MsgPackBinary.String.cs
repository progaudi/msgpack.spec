using System;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Text;

namespace ProGaudi.MsgPack.Light
{
    /// <summary>
    /// Methods for working with strings
    /// </summary>
    public static partial class MsgPackBinary
    {
        private static readonly Encoding DefaultEncoding = new UTF8Encoding(false, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteFixStringHeader(Span<byte> buffer, byte length) => TryWriteFixStringHeader(buffer, length, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteFixStringHeader(Span<byte> buffer, byte length, out int wroteSize)
        {
            wroteSize = 1;
            buffer[0] = (byte) (DataCodes.FixStringMin + length);
            return length < DataCodes.FixStringMaxLength;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteString8Header(Span<byte> buffer, byte length) => TryWriteString8Header(buffer, length, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteString8Header(Span<byte> buffer, byte length, out int wroteSize)
        {
            wroteSize = 2;
            buffer[0] = DataCodes.String8;
            buffer[1] = length;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteString16Header(Span<byte> buffer, ushort length) => TryWriteString16Header(buffer, length, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteString16Header(Span<byte> buffer, ushort length, out int wroteSize)
        {
            wroteSize = 3;
            buffer[0] = DataCodes.String16;
            return BinaryPrimitives.TryWriteUInt16BigEndian(buffer.Slice(1), length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteString32Header(Span<byte> buffer, uint length) => TryWriteString32Header(buffer, length, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteString32Header(Span<byte> buffer, uint length, out int wroteSize)
        {
            wroteSize = 5;
            buffer[0] = DataCodes.String32;
            return BinaryPrimitives.TryWriteUInt32BigEndian(buffer.Slice(1), length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ReadFixStringHeader(ReadOnlySpan<byte> buffer, out int readSize) => TryReadFixStringHeader(buffer, out var result, out readSize)
            ? result
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadFixStringHeader(ReadOnlySpan<byte> buffer, out byte value, out int readSize)
        {
            readSize = 1;
            value = buffer[1];
            var result = DataCodes.FixStringMin <= value && value <= DataCodes.FixStringMax;
            if (result)
            {
                value -= DataCodes.FixStringMin;
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ReadString8Header(ReadOnlySpan<byte> buffer, out int readSize) => TryReadString8Header(buffer, out var result, out readSize)
            ? result
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadString8Header(ReadOnlySpan<byte> buffer, out byte value, out int readSize)
        {
            readSize = 2;
            value = buffer[1];
            return buffer[0] == DataCodes.String8;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ReadString16Header(ReadOnlySpan<byte> buffer, out int readSize) => TryReadString16Header(buffer, out var result, out readSize)
            ? result
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadString16Header(ReadOnlySpan<byte> buffer, out ushort value, out int readSize)
        {
            readSize = 3;
            return BinaryPrimitives.TryReadUInt16BigEndian(buffer.Slice(1), out value) && buffer[0] == DataCodes.String16;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ReadString32Header(ReadOnlySpan<byte> buffer, out int readSize) => TryReadString32Header(buffer, out var result, out readSize)
            ? result
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadString32Header(ReadOnlySpan<byte> buffer, out uint value, out int readSize)
        {
            readSize = 3;
            return BinaryPrimitives.TryReadUInt32BigEndian(buffer.Slice(1), out value) && buffer[0] == DataCodes.String32;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteStringHeader(Span<byte> buffer, int length) => TryWriteStringHeader(buffer, length, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteStringHeader(Span<byte> buffer, int length, out int wroteSize)
        {
            if (length < 0)
            {
                wroteSize = 0;
                return false;
            }

            if (length <= DataCodes.FixStringMaxLength)
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadStringHeader(ReadOnlySpan<byte> buffer, out int readSize) => TryReadStringHeader(buffer, out var result, out readSize)
            ? result
            : throw new InvalidOperationException();

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

        public static int WriteFixString(Span<byte> buffer, ReadOnlySpan<char> chars, Encoding encoding = null)
        {
            if (chars.Length > DataCodes.FixStringMaxLength) throw new InvalidOperationException();

            var length = (encoding ?? DefaultEncoding).GetBytes(chars, buffer.Slice(1));

            var result = WriteFixStringHeader(buffer, (byte) length);

            return result + length;
        }

        public static bool TryWriteFixString(Span<byte> buffer, ReadOnlySpan<char> chars, out int wroteSize, Encoding encoding = null)
        {
            wroteSize = 0;
            if (chars.Length > DataCodes.FixStringMaxLength) return false;
            if (chars.Length > buffer.Length + 1) return false;

            var length = (encoding ?? DefaultEncoding).GetByteCount(chars);
            if (!TryWriteFixStringHeader(buffer, (byte) length, out wroteSize)) return false;

            var stringBuffer = buffer.Slice(wroteSize);
            if (length > stringBuffer.Length) return false;

            wroteSize += (encoding ?? DefaultEncoding).GetBytes(chars, stringBuffer);

            return true;
        }

        public static int WriteString8(Span<byte> buffer, ReadOnlySpan<char> chars, Encoding encoding = null)
        {
            if (chars.Length > byte.MaxValue) throw new InvalidOperationException();

            var length = (encoding ?? DefaultEncoding).GetBytes(chars, buffer.Slice(2));

            var result = WriteString8Header(buffer, (byte) length);

            return result + length;
        }

        public static bool TryWriteString8(Span<byte> buffer, ReadOnlySpan<char> chars, out int wroteSize, Encoding encoding = null)
        {
            wroteSize = 0;
            if (chars.Length > byte.MaxValue) return false;
            if (chars.Length > buffer.Length + 2) return false;

            var length = (encoding ?? DefaultEncoding).GetByteCount(chars);
            if (!TryWriteString8Header(buffer, (byte) length, out wroteSize)) return false;

            var stringBuffer = buffer.Slice(wroteSize);
            if (length > stringBuffer.Length) return false;

            wroteSize += (encoding ?? DefaultEncoding).GetBytes(chars, stringBuffer);

            return true;
        }

        public static int WriteString16(Span<byte> buffer, ReadOnlySpan<char> chars, Encoding encoding = null)
        {
            if (chars.Length > ushort.MaxValue) throw new InvalidOperationException();

            var length = (encoding ?? DefaultEncoding).GetBytes(chars, buffer.Slice(2));

            var result = WriteString16Header(buffer, (ushort) length);

            return result + length;
        }

        public static bool TryWriteString16(Span<byte> buffer, ReadOnlySpan<char> chars, out int wroteSize, Encoding encoding = null)
        {
            wroteSize = 0;
            if (chars.Length > ushort.MaxValue) return false;
            if (chars.Length > buffer.Length + 3) return false;

            var length = (encoding ?? DefaultEncoding).GetByteCount(chars);
            if (!TryWriteString16Header(buffer, (ushort) length, out wroteSize)) return false;

            var stringBuffer = buffer.Slice(wroteSize);
            if (length > stringBuffer.Length) return false;

            wroteSize += (encoding ?? DefaultEncoding).GetBytes(chars, stringBuffer);

            return true;
        }

        public static int WriteString32(Span<byte> buffer, ReadOnlySpan<char> chars, Encoding encoding = null)
        {
            var length = (encoding ?? DefaultEncoding).GetBytes(chars, buffer.Slice(2));

            var result = WriteString32Header(buffer, (uint) length);

            return result + length;
        }

        public static bool TryWriteString32(Span<byte> buffer, ReadOnlySpan<char> chars, out int wroteSize, Encoding encoding = null)
        {
            wroteSize = 0;
            if (chars.Length > buffer.Length + 5) return false;

            var length = (encoding ?? DefaultEncoding).GetByteCount(chars);
            if (!TryWriteString32Header(buffer, (uint) length, out wroteSize)) return false;

            var stringBuffer = buffer.Slice(wroteSize);
            if (length > stringBuffer.Length) return false;

            wroteSize += (encoding ?? DefaultEncoding).GetBytes(chars, stringBuffer);

            return true;
        }

        public static int WriteString(Span<byte> buffer, ReadOnlySpan<char> chars, Encoding encoding = null)
        {
            var length = (encoding ?? DefaultEncoding).GetByteCount(chars);
            if (length <= DataCodes.FixStringMaxLength)
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

        // This method is copy-pasted from methods above, because GetByteCount can be slow as hell.
        public static bool TryWriteString(Span<byte> buffer, ReadOnlySpan<char> chars, out int wroteSize, Encoding encoding = null)
        {
            wroteSize = 0;
            if (chars.Length > buffer.Length) return false;

            var length = (encoding ?? DefaultEncoding).GetByteCount(chars);
            if (length <= DataCodes.FixStringMaxLength)
            {
                if (chars.Length > length + 1) return false;
                if (TryWriteFixStringHeader(buffer, (byte) length, out wroteSize))
                {
                    wroteSize += (encoding ?? DefaultEncoding).GetBytes(chars, buffer.Slice(wroteSize));
                    return true;
                }

                return false;
            }

            if (length <= byte.MaxValue)
            {
                if (chars.Length > length + 2) return false;
                if (TryWriteString8Header(buffer, (byte) length, out wroteSize))
                {
                    wroteSize += (encoding ?? DefaultEncoding).GetBytes(chars, buffer.Slice(wroteSize));
                    return true;
                }

                return false;
            }

            if (length <= ushort.MaxValue)
            {
                if (chars.Length > length + 3) return false;
                if (TryWriteString16Header(buffer, (ushort) length, out wroteSize))
                {
                    wroteSize += (encoding ?? DefaultEncoding).GetBytes(chars, buffer.Slice(wroteSize));
                    return true;
                }

                return false;
            }

            if (chars.Length > length + 5) return false;
            if (TryWriteString32Header(buffer, (uint) length, out wroteSize))
            {
                wroteSize += (encoding ?? DefaultEncoding).GetBytes(chars, buffer.Slice(wroteSize));
                return true;
            }

            return false;
        }

        public static string ReadFixString(ReadOnlySpan<byte> buffer, out int readSize, Encoding encoding = null) => TryReadFixString(buffer, out var result, out readSize, encoding)
            ? result
            : throw new InvalidOperationException();

        public static bool TryReadFixString(ReadOnlySpan<byte> buffer, out string value, out int readSize, Encoding encoding = null)
        {
            if (TryReadFixStringHeader(buffer, out var length, out readSize))
            {
                value = (encoding ?? DefaultEncoding).GetString(buffer.Slice(readSize, length));
                return true;
            }

            value = null;
            return false;
        }

        public static string ReadString8(ReadOnlySpan<byte> buffer, out int readSize, Encoding encoding = null) => TryReadString8(buffer, out var result, out readSize, encoding)
            ? result
            : throw new InvalidOperationException();

        public static bool TryReadString8(ReadOnlySpan<byte> buffer, out string value, out int readSize, Encoding encoding = null)
        {
            if (TryReadString8Header(buffer, out var length, out readSize))
            {
                value = (encoding ?? DefaultEncoding).GetString(buffer.Slice(readSize, length));
                return true;
            }

            value = null;
            return false;
        }

        public static string ReadString16(ReadOnlySpan<byte> buffer, out int readSize, Encoding encoding = null) => TryReadString16(buffer, out var result, out readSize, encoding)
            ? result
            : throw new InvalidOperationException();

        public static bool TryReadString16(ReadOnlySpan<byte> buffer, out string value, out int readSize, Encoding encoding = null)
        {
            if (TryReadString16Header(buffer, out var length, out readSize))
            {
                value = (encoding ?? DefaultEncoding).GetString(buffer.Slice(readSize, length));
                return true;
            }

            value = null;
            return false;
        }

        public static string ReadString32(ReadOnlySpan<byte> buffer, out int readSize, Encoding encoding = null) => TryReadString32(buffer, out var result, out readSize, encoding)
            ? result
            : throw new InvalidOperationException();

        public static bool TryReadString32(ReadOnlySpan<byte> buffer, out string value, out int readSize, Encoding encoding = null)
        {
            value = null;
            if (TryReadString32Header(buffer, out var uintLength, out readSize))
            {
                if (uintLength > int.MaxValue) return false;
                var length = (int) uintLength;
                value = (encoding ?? DefaultEncoding).GetString(buffer.Slice(readSize, length));
                return true;
            }

            return false;
        }

        public static string ReadString(ReadOnlySpan<byte> buffer, out int readSize, Encoding encoding = null) => TryReadString(buffer, out var result, out readSize, encoding)
            ? result
            : throw new InvalidOperationException();

        public static bool TryReadString(ReadOnlySpan<byte> buffer, out string value, out int readSize, Encoding encoding = null)
        {
            return TryReadString8(buffer, out value, out readSize, encoding)
                || TryReadFixString(buffer, out value, out readSize, encoding)
                || TryReadString16(buffer, out value, out readSize, encoding)
                || TryReadString32(buffer, out value, out readSize, encoding);
        }

#if !NETCOREAPP2_1
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
    }
}
