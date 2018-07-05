using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;

namespace ProGaudi.MsgPack.Light
{
    /// <summary>
    /// Methods for working with binary blobs
    /// </summary>
    public static partial class MsgPackBinary
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteFixBinary8Header(Span<byte> buffer, byte length) => TryWriteFixBinary8Header(buffer, length, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteFixBinary8Header(Span<byte> buffer, byte length, out int wroteSize)
        {
            wroteSize = 2;
            buffer[0] = DataCodes.Binary8;
            buffer[1] = length;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteFixBinary16Header(Span<byte> buffer, ushort length) => TryWriteFixBinary16Header(buffer, length, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteFixBinary16Header(Span<byte> buffer, ushort length, out int wroteSize)
        {
            wroteSize = 3;
            buffer[0] = DataCodes.Binary16;
            return BinaryPrimitives.TryWriteUInt16BigEndian(buffer.Slice(1), length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteFixBinary32Header(Span<byte> buffer, uint length) => TryWriteFixBinary32Header(buffer, length, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteFixBinary32Header(Span<byte> buffer, uint length, out int wroteSize)
        {
            wroteSize = 5;
            buffer[0] = DataCodes.Binary32;
            return BinaryPrimitives.TryWriteUInt32BigEndian(buffer.Slice(1), length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ReadFixBinary8Header(ReadOnlySpan<byte> buffer, out int readSize) => TryReadFixBinary8Header(buffer, out var result, out readSize)
            ? result
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadFixBinary8Header(ReadOnlySpan<byte> buffer, out byte value, out int readSize)
        {
            readSize = 2;
            value = buffer[1];
            return buffer[0] == DataCodes.Binary8;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ReadFixBinary16Header(ReadOnlySpan<byte> buffer, out int readSize) => TryReadFixBinary16Header(buffer, out var result, out readSize)
            ? result
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadFixBinary16Header(ReadOnlySpan<byte> buffer, out ushort value, out int readSize)
        {
            readSize = 3;
            return BinaryPrimitives.TryReadUInt16BigEndian(buffer.Slice(1), out value) && buffer[0] == DataCodes.Binary16;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ReadFixBinary32Header(ReadOnlySpan<byte> buffer, out int readSize) => TryReadFixBinary32Header(buffer, out var result, out readSize)
            ? result
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadFixBinary32Header(ReadOnlySpan<byte> buffer, out uint value, out int readSize)
        {
            readSize = 3;
            return BinaryPrimitives.TryReadUInt32BigEndian(buffer.Slice(1), out value) && buffer[0] == DataCodes.Binary32;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteBinaryHeader(Span<byte> buffer, int length) => TryWriteBinaryHeader(buffer, length, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteBinaryHeader(Span<byte> buffer, int length, out int wroteSize)
        {
            if (length < 0)
            {
                wroteSize = 0;
                return false;
            }

            if (length <= byte.MaxValue)
            {
                return TryWriteFixBinary8Header(buffer, (byte) length, out wroteSize);
            }

            if (length <= ushort.MaxValue)
            {
                return TryWriteFixBinary16Header(buffer, (ushort) length, out wroteSize);
            }

            return TryWriteFixBinary32Header(buffer, (uint) length, out wroteSize);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadBinaryHeader(ReadOnlySpan<byte> buffer, out int readSize) => TryReadBinaryHeader(buffer, out var result, out readSize)
            ? result
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadBinaryHeader(ReadOnlySpan<byte> buffer, out int value, out int readSize)
        {
            if (TryReadFixBinary8Header(buffer, out var byteValue, out readSize))
            {
                value = byteValue;
                return true;
            }

            if (TryReadFixBinary16Header(buffer, out var uint16Value, out readSize))
            {
                value = uint16Value;
                return true;
            }

            if (TryReadFixBinary32Header(buffer, out var uint32Value, out readSize))
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteFixBinary8(Span<byte> buffer, ReadOnlySpan<byte> binary) => TryWriteFixBinary8(buffer, binary, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteFixBinary8(Span<byte> buffer, ReadOnlySpan<byte> binary, out int wroteSize)
        {
            var length = binary.Length;
            if (length > byte.MaxValue)
            {
                wroteSize = 0;
                return false;
            }

            if (TryWriteFixBinary8Header(buffer, (byte)length, out wroteSize))
            {
                if (binary.TryCopyTo(buffer.Slice(wroteSize)))
                {
                    wroteSize += length;
                    return true;
                }
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteFixBinary16(Span<byte> buffer, ReadOnlySpan<byte> binary) => TryWriteFixBinary16(buffer, binary, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteFixBinary16(Span<byte> buffer, ReadOnlySpan<byte> binary, out int wroteSize)
        {
            var length = binary.Length;
            if (length > ushort.MaxValue)
            {
                wroteSize = 0;
                return false;
            }

            if (TryWriteFixBinary16Header(buffer, (ushort) length, out wroteSize))
            {
                if (binary.TryCopyTo(buffer.Slice(wroteSize)))
                {
                    wroteSize += length;
                    return true;
                }
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteFixBinary32(Span<byte> buffer, ReadOnlySpan<byte> binary) => TryWriteFixBinary32(buffer, binary, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteFixBinary32(Span<byte> buffer, ReadOnlySpan<byte> binary, out int wroteSize)
        {
            var length = binary.Length;
            if (TryWriteFixBinary32Header(buffer, (uint) length, out wroteSize))
            {
                if (binary.TryCopyTo(buffer.Slice(wroteSize)))
                {
                    wroteSize += length;
                    return true;
                }
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteBinary(Span<byte> buffer, ReadOnlySpan<byte> binary) => TryWriteBinary(buffer, binary, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteBinary(Span<byte> buffer, ReadOnlySpan<byte> binary, out int wroteSize)
        {
            return TryWriteFixBinary8(buffer, binary, out wroteSize)
                || TryWriteFixBinary16(buffer, binary, out wroteSize)
                || TryWriteFixBinary32(buffer, binary, out wroteSize);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IMemoryOwner<byte> ReadFixBinary8(ReadOnlySpan<byte> buffer, out int readSize) => TryReadFixBinary8(buffer, out var result, out readSize)
            ? result
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadFixBinary8(ReadOnlySpan<byte> buffer, out IMemoryOwner<byte> value, out int readSize)
        {
            if (TryReadFixBinary8Header(buffer, out var length, out readSize))
            {
                value = MemoryPool<byte>.Shared.Rent(length);
                if (buffer.Slice(readSize, length).TryCopyTo(value.Memory.Span))
                {
                    readSize += length;
                    return true;
                }
            }

            value = null;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IMemoryOwner<byte> ReadFixBinary16(ReadOnlySpan<byte> buffer, out int readSize) => TryReadFixBinary16(buffer, out var result, out readSize)
            ? result
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadFixBinary16(ReadOnlySpan<byte> buffer, out IMemoryOwner<byte> value, out int readSize)
        {
            if (TryReadFixBinary16Header(buffer, out var length, out readSize))
            {
                value = MemoryPool<byte>.Shared.Rent(length);
                if (buffer.Slice(readSize, length).TryCopyTo(value.Memory.Span))
                {
                    readSize += length;
                    return true;
                }
            }

            value = null;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IMemoryOwner<byte> ReadFixBinary32(ReadOnlySpan<byte> buffer, out int readSize) => TryReadFixBinary32(buffer, out var result, out readSize)
            ? result
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadFixBinary32(ReadOnlySpan<byte> buffer, out IMemoryOwner<byte> value, out int readSize)
        {
            if (TryReadFixBinary32Header(buffer, out var uint32Length, out readSize))
            {
                if (uint32Length <= int.MaxValue)
                {
                    var length = (int)uint32Length;
                    value = MemoryPool<byte>.Shared.Rent(length);
                    if (buffer.Slice(readSize, length).TryCopyTo(value.Memory.Span))
                    {
                        readSize += length;
                        return true;
                    }
                }
            }

            value = null;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IMemoryOwner<byte> ReadBinary(ReadOnlySpan<byte> buffer, out int readSize) => TryReadBinary(buffer, out var result, out readSize)
            ? result
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadBinary(ReadOnlySpan<byte> buffer, out IMemoryOwner<byte> value, out int readSize)
        {
            return TryReadFixBinary8(buffer, out value, out readSize)
                || TryReadFixBinary16(buffer, out value, out readSize)
                || TryReadFixBinary32(buffer, out value, out readSize);
        }
    }
}
