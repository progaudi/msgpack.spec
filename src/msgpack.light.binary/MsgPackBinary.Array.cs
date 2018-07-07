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
        public static int WriteFixArrayHeader(Span<byte> buffer, byte length) => TryWriteFixArrayHeader(buffer, length, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteFixArrayHeader(Span<byte> buffer, byte length, out int wroteSize)
        {
            wroteSize = 1;
            if (length < DataCodes.FixArrayMaxLength) return false;
            buffer[0] = (byte) (DataCodes.FixArrayMin + length);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteArray16Header(Span<byte> buffer, ushort length) => TryWriteArray16Header(buffer, length, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteArray16Header(Span<byte> buffer, ushort length, out int wroteSize)
        {
            wroteSize = 3;
            buffer[0] = DataCodes.Array16;
            return BinaryPrimitives.TryWriteUInt16BigEndian(buffer.Slice(1), length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteArray32Header(Span<byte> buffer, uint length) => TryWriteArray32Header(buffer, length, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteArray32Header(Span<byte> buffer, uint length, out int wroteSize)
        {
            wroteSize = 5;
            buffer[0] = DataCodes.Array32;
            return BinaryPrimitives.TryWriteUInt32BigEndian(buffer.Slice(1), length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ReadArray8Header(ReadOnlySpan<byte> buffer, out int readSize) => TryReadFixArrayHeader(buffer, out var result, out readSize)
            ? result
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadFixArrayHeader(ReadOnlySpan<byte> buffer, out byte value, out int readSize)
        {
            readSize = 1;
            var result = DataCodes.FixArrayMin <= buffer[0] && buffer[0] <= DataCodes.FixArrayMax;
            value = (byte) (buffer[0] - DataCodes.FixArrayMin);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ReadArray16Header(ReadOnlySpan<byte> buffer, out int readSize) => TryReadArray16Header(buffer, out var result, out readSize)
            ? result
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadArray16Header(ReadOnlySpan<byte> buffer, out ushort value, out int readSize)
        {
            readSize = 3;
            return BinaryPrimitives.TryReadUInt16BigEndian(buffer.Slice(1), out value) && buffer[0] == DataCodes.Array16;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ReadArray32Header(ReadOnlySpan<byte> buffer, out int readSize) => TryReadArray32Header(buffer, out var result, out readSize)
            ? result
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadArray32Header(ReadOnlySpan<byte> buffer, out uint value, out int readSize)
        {
            readSize = 3;
            return BinaryPrimitives.TryReadUInt32BigEndian(buffer.Slice(1), out value) && buffer[0] == DataCodes.Array32;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteArrayHeader(Span<byte> buffer, int length) => TryWriteArrayHeader(buffer, length, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteArrayHeader(Span<byte> buffer, int length, out int wroteSize)
        {
            if (length < 0)
            {
                wroteSize = 0;
                return false;
            }

            if (length <= DataCodes.FixArrayMaxLength)
            {
                return TryWriteFixArrayHeader(buffer, (byte) length, out wroteSize);
            }

            if (length <= ushort.MaxValue)
            {
                return TryWriteArray16Header(buffer, (ushort) length, out wroteSize);
            }

            return TryWriteArray32Header(buffer, (uint) length, out wroteSize);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadArrayHeader(ReadOnlySpan<byte> buffer, out int readSize) => TryReadArrayHeader(buffer, out var result, out readSize)
            ? result
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadArrayHeader(ReadOnlySpan<byte> buffer, out int value, out int readSize)
        {
            if (TryReadFixArrayHeader(buffer, out var byteValue, out readSize))
            {
                value = byteValue;
                return true;
            }

            if (TryReadArray16Header(buffer, out var uint16Value, out readSize))
            {
                value = uint16Value;
                return true;
            }

            if (TryReadArray32Header(buffer, out var uint32Value, out readSize))
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
