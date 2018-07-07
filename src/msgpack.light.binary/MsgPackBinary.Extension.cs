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
        public static int WriteFixExtension1Header(Span<byte> buffer, byte type) => WriteExtensionHeader(buffer, DataCodes.FixExtension1, type);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteFixExtension1(Span<byte> buffer, byte type, byte extension)
        {
            buffer[2] = extension;
            return WriteFixExtension1Header(buffer, type) + 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteFixExtension2Header(Span<byte> buffer, byte type) => WriteExtensionHeader(buffer, DataCodes.FixExtension2, type);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteFixExtension2(Span<byte> buffer, byte type, ReadOnlySpan<byte> extension)
        {
            buffer[2] = extension[0];
            buffer[3] = extension[1];
            return WriteFixExtension2Header(buffer, type) + 2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteFixExtension4Header(Span<byte> buffer, byte type) => WriteExtensionHeader(buffer, DataCodes.FixExtension4, type);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteFixExtension4(Span<byte> buffer, byte type, ReadOnlySpan<byte> extension)
        {
            var result = WriteFixExtension4Header(buffer, type);
            const int extLength = 4;
            extension.Slice(0, extLength).CopyTo(buffer.Slice(result));
            return result + extLength;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteFixExtension8Header(Span<byte> buffer, byte type) => WriteExtensionHeader(buffer, DataCodes.FixExtension8, type);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteFixExtension8(Span<byte> buffer, byte type, ReadOnlySpan<byte> extension)
        {
            var result = WriteFixExtension8Header(buffer, type);
            const int extLength = 8;
            extension.Slice(0, extLength).CopyTo(buffer.Slice(result));
            return result + extLength;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteFixExtension16Header(Span<byte> buffer, byte type) => WriteExtensionHeader(buffer, DataCodes.FixExtension16, type);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteFixExtension16(Span<byte> buffer, byte type, ReadOnlySpan<byte> extension)
        {
            var result = WriteFixExtension16Header(buffer, type);
            const int extLength = 16;
            extension.Slice(0, extLength).CopyTo(buffer.Slice(result));
            return result + extLength;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteExtension8Header(Span<byte> buffer, byte type, byte length)
        {
            WriteExtensionHeader(buffer, DataCodes.Extension8, type);
            buffer[2] = length;
            return 3;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteExtension8(Span<byte> buffer, byte type, ReadOnlySpan<byte> extension)
        {
            if (extension.Length > byte.MaxValue) throw new InvalidOperationException();
            var result = WriteExtension8Header(buffer, type, (byte)extension.Length);
            extension.CopyTo(buffer.Slice(result));
            return result + extension.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteExtension16Header(Span<byte> buffer, byte type, ushort length)
        {
            WriteExtensionHeader(buffer, DataCodes.Extension16, type);
            BinaryPrimitives.WriteUInt16BigEndian(buffer.Slice(2), length);
            return 4;
        }

        public static int WriteExtension16(Span<byte> buffer, byte type, ReadOnlySpan<byte> extension)
        {
            if (extension.Length > ushort.MaxValue) throw new InvalidOperationException();
            var result = WriteExtension16Header(buffer, type, (ushort)extension.Length);
            extension.CopyTo(buffer.Slice(result));
            return result + extension.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteExtension32Header(Span<byte> buffer, byte type, uint length)
        {
            WriteExtensionHeader(buffer, DataCodes.Extension32, type);
            BinaryPrimitives.WriteUInt32BigEndian(buffer.Slice(2), length);
            return 6;
        }

        public static int WriteExtension32(Span<byte> buffer, byte type, ReadOnlySpan<byte> extension)
        {
            var result = WriteExtension32Header(buffer, type, (ushort)extension.Length);
            extension.CopyTo(buffer.Slice(result));
            return result + extension.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteExtensionHeader(Span<byte> buffer, byte type, uint length)
        {
            switch (length)
            {
                case 1:
                    return WriteFixExtension1Header(buffer, type);
                case 2:
                    return WriteFixExtension2Header(buffer, type);
                case 4:
                    return WriteFixExtension4Header(buffer, type);
                case 8:
                    return WriteFixExtension8Header(buffer, type);
                case 16:
                    return WriteFixExtension16Header(buffer, type);
            }

            if (length <= byte.MaxValue)
                return WriteExtension8Header(buffer, type, (byte) length);

            if (length <= ushort.MaxValue)
                return WriteExtension16Header(buffer, type, (ushort) length);

            return WriteExtension32Header(buffer, type, length);
        }

        public static int WriteExtension(Span<byte> buffer, byte type, ReadOnlySpan<byte> extension)
        {
            var result = WriteExtensionHeader(buffer, type, (uint) extension.Length);
            extension.CopyTo(buffer.Slice(result));
            return result + extension.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteFixExtension1Header(Span<byte> buffer, byte type, out int wroteSize) => TryWriteExtensionHeader(buffer, DataCodes.FixExtension1, type, out wroteSize);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteFixExtension1(Span<byte> buffer, byte type, byte extension, out int wroteSize)
        {
            wroteSize = 0;
            const int size = 3;
            if (buffer.Length < size) return false;

            WriteFixExtension1(buffer, type, extension);
            wroteSize = size;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteFixExtension2Header(Span<byte> buffer, byte type, out int wroteSize) => TryWriteExtensionHeader(buffer, DataCodes.FixExtension2, type, out wroteSize);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteFixExtension2(Span<byte> buffer, byte type, ReadOnlySpan<byte> extension, out int wroteSize)
        {
            wroteSize = 0;
            if (extension.Length != 2) return false;
            const int size = 3;
            if (buffer.Length < size) return false;

            WriteFixExtension2(buffer, type, extension);
            wroteSize = size;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteFixExtension4Header(Span<byte> buffer, byte type, out int wroteSize) => TryWriteExtensionHeader(buffer, DataCodes.FixExtension4, type, out wroteSize);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteFixExtension4(Span<byte> buffer, byte type, ReadOnlySpan<byte> extension, out int wroteSize)
        {
            wroteSize = 0;
            if (extension.Length != 4) return false;
            const int size = 6;
            if (buffer.Length < size) return false;

            WriteFixExtension4(buffer, type, extension);
            wroteSize = size;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteFixExtension8Header(Span<byte> buffer, byte type, out int wroteSize) => TryWriteExtensionHeader(buffer, DataCodes.FixExtension8, type, out wroteSize);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteFixExtension8(Span<byte> buffer, byte type, ReadOnlySpan<byte> extension, out int wroteSize)
        {
            wroteSize = 0;
            if (extension.Length != 8) return false;
            const int size = 10;
            if (buffer.Length < size) return false;

            WriteFixExtension8(buffer, type, extension);
            wroteSize = size;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteFixExtension16Header(Span<byte> buffer, byte type, out int wroteSize) => TryWriteExtensionHeader(buffer, DataCodes.FixExtension16, type, out wroteSize);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteFixExtension16(Span<byte> buffer, byte type, ReadOnlySpan<byte> extension, out int wroteSize)
        {
            wroteSize = 0;
            if (extension.Length != 16) return false;
            const int size = 18;
            if (buffer.Length < size) return false;

            WriteFixExtension16(buffer, type, extension);
            wroteSize = size;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteExtension8Header(Span<byte> buffer, byte type, byte length, out int wroteSize)
        {
            wroteSize = 0;
            const int size = 3;
            if (buffer.Length < size) return false;

            WriteExtension8Header(buffer, type, length);
            wroteSize = size;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteExtension8(Span<byte> buffer, byte type, ReadOnlySpan<byte> extension, out int wroteSize)
        {
            wroteSize = 0;
            if (extension.Length > byte.MaxValue) return false;
            if (buffer.Length < extension.Length + 3) return false;

            wroteSize = WriteExtension8(buffer, type, extension);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteExtension16Header(Span<byte> buffer, byte type, ushort length, out int wroteSize)
        {
            wroteSize = 0;
            const int size = 4;
            if (buffer.Length < size) return false;

            WriteExtension16Header(buffer, type, length);
            wroteSize = size;
            return true;
        }

        public static bool TryWriteExtension16(Span<byte> buffer, byte type, ReadOnlySpan<byte> extension, out int wroteSize)
        {
            wroteSize = 0;
            if (extension.Length > ushort.MaxValue) return false;
            if (buffer.Length < extension.Length + 4) return false;

            wroteSize = WriteExtension16(buffer, type, extension);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteExtension32Header(Span<byte> buffer, byte type, uint length, out int wroteSize)
        {
            wroteSize = 0;
            const int size = 6;
            if (buffer.Length < size) return false;

            WriteExtension32Header(buffer, type, length);
            wroteSize = size;
            return true;
        }

        public static bool TryWriteExtension32(Span<byte> buffer, byte type, ReadOnlySpan<byte> extension, out int wroteSize)
        {
            wroteSize = 0;
            if (buffer.Length < extension.Length + 6) return false;

            wroteSize = WriteExtension32(buffer, type, extension);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteExtensionHeader(Span<byte> buffer, byte type, uint length, out int wroteSize)
        {
            switch (length)
            {
                case 1:
                    return TryWriteFixExtension1Header(buffer, type, out wroteSize);
                case 2:
                    return TryWriteFixExtension2Header(buffer, type, out wroteSize);
                case 4:
                    return TryWriteFixExtension4Header(buffer, type, out wroteSize);
                case 8:
                    return TryWriteFixExtension8Header(buffer, type, out wroteSize);
                case 16:
                    return TryWriteFixExtension16Header(buffer, type, out wroteSize);
            }

            if (length <= byte.MaxValue)
                return TryWriteExtension8Header(buffer, type, (byte) length, out wroteSize);

            if (length <= ushort.MaxValue)
                return TryWriteExtension16Header(buffer, type, (ushort) length, out wroteSize);

            return TryWriteExtension32Header(buffer, type, length, out wroteSize);
        }

        public static bool TryWriteExtension(Span<byte> buffer, byte type, ReadOnlySpan<byte> extension, out int wroteSize)
        {
            if (!TryWriteExtensionHeader(buffer, type, (uint) extension.Length, out wroteSize)) return false;
            if (wroteSize + extension.Length > buffer.Length) return false;
            extension.CopyTo(buffer.Slice(wroteSize));
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ReadFixExtension1Header(ReadOnlySpan<byte> buffer, out int readSize)
        {
            readSize = 2;
            if (buffer[0] != DataCodes.FixExtension1) throw new InvalidOperationException();
            return buffer[1];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (byte type, byte extension) ReadFixExtension1(ReadOnlySpan<byte> buffer, out int readSize)
        {
            readSize = 3;
            return (ReadFixExtension1Header(buffer, out _), buffer[2]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ReadFixExtension2Header(ReadOnlySpan<byte> buffer, out int readSize)
        {
            readSize = 2;
            if (buffer[0] != DataCodes.FixExtension2) throw new InvalidOperationException();
            return buffer[1];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (byte type, IMemoryOwner<byte> extension) ReadFixExtension2(ReadOnlySpan<byte> buffer, out int readSize)
        {
            const int size = 2;
            readSize = size + 2;
            var owner = MemoryPool<byte>.Shared.Rent(size);
            buffer.Slice(2, size).CopyTo(owner.Memory.Span);
            return (ReadFixExtension2Header(buffer, out _), owner);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ReadFixExtension4Header(ReadOnlySpan<byte> buffer, out int readSize)
        {
            readSize = 2;
            if (buffer[0] != DataCodes.FixExtension4) throw new InvalidOperationException();
            return buffer[1];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (byte type, IMemoryOwner<byte> extension) ReadFixExtension4(ReadOnlySpan<byte> buffer, out int readSize)
        {
            const int size = 4;
            readSize = size + 2;
            var owner = MemoryPool<byte>.Shared.Rent(size);
            buffer.Slice(2, size).CopyTo(owner.Memory.Span);
            return (ReadFixExtension4Header(buffer, out _), owner);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ReadFixExtension8Header(ReadOnlySpan<byte> buffer, out int readSize)
        {
            readSize = 2;
            if (buffer[0] != DataCodes.FixExtension8) throw new InvalidOperationException();
            return buffer[1];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (byte type, IMemoryOwner<byte> extension) ReadFixExtension8(ReadOnlySpan<byte> buffer, out int readSize)
        {
            const int size = 8;
            readSize = size + 2;
            var owner = MemoryPool<byte>.Shared.Rent(size);
            buffer.Slice(2, size).CopyTo(owner.Memory.Span);
            return (ReadFixExtension8Header(buffer, out _), owner);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ReadFixExtension16Header(ReadOnlySpan<byte> buffer, out int readSize)
        {
            readSize = 2;
            if (buffer[0] != DataCodes.FixExtension16) throw new InvalidOperationException();
            return buffer[1];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (byte type, IMemoryOwner<byte> extension) ReadFixExtension16(ReadOnlySpan<byte> buffer, out int readSize)
        {
            const int size = 16;
            readSize = size + 2;
            var owner = MemoryPool<byte>.Shared.Rent(size);
            buffer.Slice(2, size).CopyTo(owner.Memory.Span);
            return (ReadFixExtension8Header(buffer, out _), owner);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (byte type, byte length) ReadExtension8Header(ReadOnlySpan<byte> buffer, out int readSize)
        {
            readSize = 3;
            if (buffer[0] != DataCodes.Extension8) throw new InvalidOperationException();
            return (buffer[1], buffer[2]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (byte type, IMemoryOwner<byte> extension) ReadExtension8(ReadOnlySpan<byte> buffer, out int readSize)
        {
            var (type, size) = ReadExtension8Header(buffer, out readSize);
            var owner = MemoryPool<byte>.Shared.Rent(size);
            buffer.Slice(readSize, size).CopyTo(owner.Memory.Span);
            readSize += size;
            return (type, owner);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (byte type, ushort length) ReadExtension16Header(ReadOnlySpan<byte> buffer, out int readSize)
        {
            readSize = 4;
            if (buffer[0] != DataCodes.Extension16) throw new InvalidOperationException();
            return (buffer[1], BinaryPrimitives.ReadUInt16BigEndian(buffer.Slice(2)));
        }

        public static (byte type, IMemoryOwner<byte> extension) ReadExtension16(ReadOnlySpan<byte> buffer, out int readSize)
        {
            var (type, size) = ReadExtension16Header(buffer, out readSize);
            var owner = MemoryPool<byte>.Shared.Rent(size);
            buffer.Slice(readSize, size).CopyTo(owner.Memory.Span);
            readSize += size;
            return (type, owner);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (byte type, uint length) ReadExtension32Header(ReadOnlySpan<byte> buffer, out int readSize)
        {
            readSize = 6;
            if (buffer[0] != DataCodes.Extension16) throw new InvalidOperationException();
            return (buffer[1], BinaryPrimitives.ReadUInt32BigEndian(buffer.Slice(2)));
        }

        public static (byte type, IMemoryOwner<byte> extension) ReadExtension32(ReadOnlySpan<byte> buffer, out int readSize)
        {
            var (type, uintSize) = ReadExtension32Header(buffer, out readSize);
            if (uintSize > int.MaxValue) throw new InvalidOperationException();
            var size = (int) uintSize;
            var owner = MemoryPool<byte>.Shared.Rent(size);
            buffer.Slice(readSize, size).CopyTo(owner.Memory.Span);
            readSize += size;
            return (type, owner);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadFixExtension1Header(ReadOnlySpan<byte> buffer, out byte type, out int readSize) => TryReadExtensionHeader(buffer, DataCodes.FixExtension1, out type, out readSize);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadFixExtension1(ReadOnlySpan<byte> buffer, out byte type, out byte extension, out int readSize)
        {
            extension = 0;
            if (!TryReadFixExtension1Header(buffer, out type, out readSize)) return false;
            var slice = buffer.Slice(2);
            if (slice.IsEmpty) return false;
            readSize += 1;
            extension = slice[0];
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadFixExtension2Header(ReadOnlySpan<byte> buffer, out byte type, out int readSize) => TryReadExtensionHeader(buffer, DataCodes.FixExtension2, out type, out readSize);

        public static bool TryReadFixExtension2(ReadOnlySpan<byte> buffer, out byte type, out IMemoryOwner<byte> extension, out int readSize)
        {
            extension = null;
            return TryReadFixExtension2Header(buffer, out type, out readSize) && TryReadExtension(buffer, 2, ref extension, ref readSize);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadFixExtension4Header(ReadOnlySpan<byte> buffer, out byte type, out int readSize) => TryReadExtensionHeader(buffer, DataCodes.FixExtension4, out type, out readSize);

        public static bool TryReadFixExtension4(ReadOnlySpan<byte> buffer, out byte type, out IMemoryOwner<byte> extension, out int readSize)
        {
            extension = null;
            return TryReadFixExtension4Header(buffer, out type, out readSize) && TryReadExtension(buffer, 4, ref extension, ref readSize);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadFixExtension8Header(ReadOnlySpan<byte> buffer, out byte type, out int readSize) => TryReadExtensionHeader(buffer, DataCodes.FixExtension8, out type, out readSize);

        public static bool TryReadFixExtension8(ReadOnlySpan<byte> buffer, out byte type, out IMemoryOwner<byte> extension, out int readSize)
        {
            extension = null;
            return TryReadFixExtension8Header(buffer, out type, out readSize) && TryReadExtension(buffer, 8, ref extension, ref readSize);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadFixExtension16Header(ReadOnlySpan<byte> buffer, out byte type, out int readSize) => TryReadExtensionHeader(buffer, DataCodes.FixExtension16, out type, out readSize);

        public static bool TryReadFixExtension16(ReadOnlySpan<byte> buffer, out byte type, out IMemoryOwner<byte> extension, out int readSize)
        {
            extension = null;
            return TryReadFixExtension16Header(buffer, out type, out readSize) && TryReadExtension(buffer, 16, ref extension, ref readSize);
        }

        public static bool TryReadExtension8Header(ReadOnlySpan<byte> buffer, out byte type, out byte length, out int readSize)
        {
            length = 0;
            if (!TryReadExtensionHeader(buffer, DataCodes.Extension8, out type, out readSize)) return false;
            if (buffer.Length < 3) return false;
            readSize = 3;
            length = buffer[2];
            return true;
        }

        public static bool TryReadExtension8(ReadOnlySpan<byte> buffer, out byte type, out IMemoryOwner<byte> extension, out int readSize)
        {
            extension = null;
            return TryReadExtension8Header(buffer, out type, out var length, out readSize)
                && TryReadExtension(buffer, length, ref extension, ref readSize);
        }

        public static bool TryReadExtension16Header(ReadOnlySpan<byte> buffer, out byte type, out ushort length, out int readSize)
        {
            length = 0;
            if (!TryReadExtensionHeader(buffer, DataCodes.Extension16, out type, out readSize)) return false;
            if (buffer.Length < 4) return false;
            length = BinaryPrimitives.ReadUInt16BigEndian(buffer.Slice(readSize));
            readSize = 4;
            return true;
        }

        public static bool TryReadExtension16(ReadOnlySpan<byte> buffer, out byte type, out IMemoryOwner<byte> extension, out int readSize)
        {
            extension = null;
            return TryReadExtension16Header(buffer, out type, out var length, out readSize)
                && TryReadExtension(buffer, length, ref extension, ref readSize);
        }

        public static bool TryReadExtension32Header(ReadOnlySpan<byte> buffer, out byte type, out uint length, out int readSize)
        {
            length = 0;
            if (!TryReadExtensionHeader(buffer, DataCodes.Extension8, out type, out readSize)) return false;
            if (buffer.Length < 6) return false;
            length = BinaryPrimitives.ReadUInt32BigEndian(buffer.Slice(readSize));
            readSize = 6;
            return true;
        }

        public static bool TryReadExtension32(ReadOnlySpan<byte> buffer, out byte type, out IMemoryOwner<byte> extension, out int readSize)
        {
            extension = null;
            return TryReadExtension32Header(buffer, out type, out var length, out readSize)
                && length <= int.MaxValue
                && TryReadExtension(buffer, (int) length, ref extension, ref readSize);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int WriteExtensionHeader(Span<byte> buffer, byte code, byte type)
        {
            buffer[0] = code;
            buffer[1] = type;
            return 2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryWriteExtensionHeader(Span<byte> buffer, byte code, byte type, out int wroteSize)
        {
            wroteSize = 0;
            const int size = 2;
            if (buffer.Length < size) return false;

            WriteExtensionHeader(buffer, code, type);
            wroteSize = size;
            return true;
        }

        private static bool TryReadExtension(ReadOnlySpan<byte> buffer, int size, ref IMemoryOwner<byte> extension, ref int readSize)
        {
            if (buffer.Length - readSize < size) return false;

            extension = MemoryPool<byte>.Shared.Rent(size);
            if (buffer.Slice(readSize).TryCopyTo(extension.Memory.Span))
            {
                readSize += size;
                return true;
            }

            return false;
        }

        private static bool TryReadExtensionHeader(ReadOnlySpan<byte> buffer, byte code, out byte type, out int readSize)
        {
            type = 0;
            readSize = 2;
            if (buffer.Length < 2) return false;
            if (buffer[0] != code) return false;
            type = buffer[1];
            return true;
        }
    }
}
