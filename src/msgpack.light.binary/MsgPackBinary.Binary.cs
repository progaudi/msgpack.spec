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
        public static int WriteBinary8Header(Span<byte> buffer, byte length) => TryWriteBinary8Header(buffer, length, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteBinary8Header(Span<byte> buffer, byte length, out int wroteSize)
        {
            wroteSize = 2;
            buffer[0] = DataCodes.Binary8;
            buffer[1] = length;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteBinary16Header(Span<byte> buffer, ushort length) => TryWriteBinary16Header(buffer, length, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteBinary16Header(Span<byte> buffer, ushort length, out int wroteSize)
        {
            wroteSize = 3;
            buffer[0] = DataCodes.Binary16;
            return BinaryPrimitives.TryWriteUInt16BigEndian(buffer.Slice(1), length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteBinary32Header(Span<byte> buffer, uint length) => TryWriteBinary32Header(buffer, length, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteBinary32Header(Span<byte> buffer, uint length, out int wroteSize)
        {
            wroteSize = 5;
            buffer[0] = DataCodes.Binary32;
            return BinaryPrimitives.TryWriteUInt32BigEndian(buffer.Slice(1), length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ReadBinary8Header(ReadOnlySpan<byte> buffer, out int readSize) => TryReadBinary8Header(buffer, out var result, out readSize)
            ? result
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadBinary8Header(ReadOnlySpan<byte> buffer, out byte value, out int readSize)
        {
            readSize = 2;
            value = buffer[1];
            return buffer[0] == DataCodes.Binary8;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ReadBinary16Header(ReadOnlySpan<byte> buffer, out int readSize) => TryReadBinary16Header(buffer, out var result, out readSize)
            ? result
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadBinary16Header(ReadOnlySpan<byte> buffer, out ushort value, out int readSize)
        {
            readSize = 3;
            return BinaryPrimitives.TryReadUInt16BigEndian(buffer.Slice(1), out value) && buffer[0] == DataCodes.Binary16;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ReadBinary32Header(ReadOnlySpan<byte> buffer, out int readSize) => TryReadBinary32Header(buffer, out var result, out readSize)
            ? result
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadBinary32Header(ReadOnlySpan<byte> buffer, out uint value, out int readSize)
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
                return TryWriteBinary8Header(buffer, (byte) length, out wroteSize);
            }

            if (length <= ushort.MaxValue)
            {
                return TryWriteBinary16Header(buffer, (ushort) length, out wroteSize);
            }

            return TryWriteBinary32Header(buffer, (uint) length, out wroteSize);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadBinaryHeader(ReadOnlySpan<byte> buffer, out int readSize) => TryReadBinaryHeader(buffer, out var result, out readSize)
            ? result
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadBinaryHeader(ReadOnlySpan<byte> buffer, out int value, out int readSize)
        {
            if (TryReadBinary8Header(buffer, out var byteValue, out readSize))
            {
                value = byteValue;
                return true;
            }

            if (TryReadBinary16Header(buffer, out var uint16Value, out readSize))
            {
                value = uint16Value;
                return true;
            }

            if (TryReadBinary32Header(buffer, out var uint32Value, out readSize))
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
        public static int WriteBinary8(Span<byte> buffer, ReadOnlySpan<byte> binary) => TryWriteBinary8(buffer, binary, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteBinary8(Span<byte> buffer, ReadOnlySpan<byte> binary, out int wroteSize)
        {
            var length = binary.Length;
            if (length > byte.MaxValue)
            {
                wroteSize = 0;
                return false;
            }

            if (TryWriteBinary8Header(buffer, (byte)length, out wroteSize))
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
        public static int WriteBinary16(Span<byte> buffer, ReadOnlySpan<byte> binary) => TryWriteBinary16(buffer, binary, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteBinary16(Span<byte> buffer, ReadOnlySpan<byte> binary, out int wroteSize)
        {
            var length = binary.Length;
            if (length > ushort.MaxValue)
            {
                wroteSize = 0;
                return false;
            }

            if (TryWriteBinary16Header(buffer, (ushort) length, out wroteSize))
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
        public static int WriteBinary32(Span<byte> buffer, ReadOnlySpan<byte> binary) => TryWriteBinary32(buffer, binary, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteBinary32(Span<byte> buffer, ReadOnlySpan<byte> binary, out int wroteSize)
        {
            var length = binary.Length;
            if (TryWriteBinary32Header(buffer, (uint) length, out wroteSize))
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
            return TryWriteBinary8(buffer, binary, out wroteSize)
                || TryWriteBinary16(buffer, binary, out wroteSize)
                || TryWriteBinary32(buffer, binary, out wroteSize);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IMemoryOwner<byte> ReadBinary8(ReadOnlySpan<byte> buffer, out int readSize) => TryReadBinary8(buffer, out var result, out readSize)
            ? result
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadBinary8(ReadOnlySpan<byte> buffer, out IMemoryOwner<byte> value, out int readSize)
        {
            if (TryReadBinary8Header(buffer, out var length, out readSize))
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
        public static IMemoryOwner<byte> ReadBinary16(ReadOnlySpan<byte> buffer, out int readSize) => TryReadBinary16(buffer, out var result, out readSize)
            ? result
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadBinary16(ReadOnlySpan<byte> buffer, out IMemoryOwner<byte> value, out int readSize)
        {
            if (TryReadBinary16Header(buffer, out var length, out readSize))
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
        public static IMemoryOwner<byte> ReadBinary32(ReadOnlySpan<byte> buffer, out int readSize) => TryReadBinary32(buffer, out var result, out readSize)
            ? result
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadBinary32(ReadOnlySpan<byte> buffer, out IMemoryOwner<byte> value, out int readSize)
        {
            if (TryReadBinary32Header(buffer, out var uint32Length, out readSize))
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
            return TryReadBinary8(buffer, out value, out readSize)
                || TryReadBinary16(buffer, out value, out readSize)
                || TryReadBinary32(buffer, out value, out readSize);
        }
    }
}
