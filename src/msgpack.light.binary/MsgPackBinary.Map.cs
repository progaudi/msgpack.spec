using System;
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
        public static int WriteFixMapHeader(Span<byte> buffer, byte length) => TryWriteFixMapHeader(buffer, length, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteFixMapHeader(Span<byte> buffer, byte length, out int wroteSize)
        {
            wroteSize = 1;
            if (length < DataCodes.FixMapMaxLength) return false;
            buffer[0] = (byte) (DataCodes.FixMapMin + length);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteMap16Header(Span<byte> buffer, ushort length) => TryWriteMap16Header(buffer, length, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteMap16Header(Span<byte> buffer, ushort length, out int wroteSize)
        {
            wroteSize = 3;
            buffer[0] = DataCodes.Map16;
            return BinaryPrimitives.TryWriteUInt16BigEndian(buffer.Slice(1), length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteMap32Header(Span<byte> buffer, uint length) => TryWriteMap32Header(buffer, length, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteMap32Header(Span<byte> buffer, uint length, out int wroteSize)
        {
            wroteSize = 5;
            buffer[0] = DataCodes.Map32;
            return BinaryPrimitives.TryWriteUInt32BigEndian(buffer.Slice(1), length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ReadMap8Header(ReadOnlySpan<byte> buffer, out int readSize) => TryReadFixMapHeader(buffer, out var result, out readSize)
            ? result
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadFixMapHeader(ReadOnlySpan<byte> buffer, out byte value, out int readSize)
        {
            readSize = 1;
            var result = DataCodes.FixMapMin <= buffer[0] && buffer[0] <= DataCodes.FixMapMax;
            value = (byte) (buffer[0] - DataCodes.FixMapMin);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ReadMap16Header(ReadOnlySpan<byte> buffer, out int readSize) => TryReadMap16Header(buffer, out var result, out readSize)
            ? result
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadMap16Header(ReadOnlySpan<byte> buffer, out ushort value, out int readSize)
        {
            readSize = 3;
            return BinaryPrimitives.TryReadUInt16BigEndian(buffer.Slice(1), out value) && buffer[0] == DataCodes.Map16;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ReadMap32Header(ReadOnlySpan<byte> buffer, out int readSize) => TryReadMap32Header(buffer, out var result, out readSize)
            ? result
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadMap32Header(ReadOnlySpan<byte> buffer, out uint value, out int readSize)
        {
            readSize = 3;
            return BinaryPrimitives.TryReadUInt32BigEndian(buffer.Slice(1), out value) && buffer[0] == DataCodes.Map32;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteMapHeader(Span<byte> buffer, int length) => TryWriteMapHeader(buffer, length, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteMapHeader(Span<byte> buffer, int length, out int wroteSize)
        {
            if (length < 0)
            {
                wroteSize = 0;
                return false;
            }

            if (length <= DataCodes.FixMapMaxLength)
            {
                return TryWriteFixMapHeader(buffer, (byte) length, out wroteSize);
            }

            if (length <= ushort.MaxValue)
            {
                return TryWriteMap16Header(buffer, (ushort) length, out wroteSize);
            }

            return TryWriteMap32Header(buffer, (uint) length, out wroteSize);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadMapHeader(ReadOnlySpan<byte> buffer, out int readSize) => TryReadMapHeader(buffer, out var result, out readSize)
            ? result
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadMapHeader(ReadOnlySpan<byte> buffer, out int value, out int readSize)
        {
            if (TryReadFixMapHeader(buffer, out var byteValue, out readSize))
            {
                value = byteValue;
                return true;
            }

            if (TryReadMap16Header(buffer, out var uint16Value, out readSize))
            {
                value = uint16Value;
                return true;
            }

            if (TryReadMap32Header(buffer, out var uint32Value, out readSize))
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
    }
}
