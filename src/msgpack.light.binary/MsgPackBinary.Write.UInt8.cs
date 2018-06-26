using System;
using System.Runtime.CompilerServices;

namespace ProGaudi.MsgPack.Light
{
    /// <summary>
    /// Methods for working with unsigned int 8
    /// </summary>
    public static partial class MsgPackBinary
    {
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public static int WriteFixUInt8(in Span<byte> buffer, byte value)
        //{
        //    EnsureCapacity(buffer, 2);
        //    buffer[0] = DataCodes.UInt8;
        //    buffer[1] = value;
        //    return 2;
        //}

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public static int WriteUInt8(in Span<byte> buffer, byte value)
        //{
        //    if (value <= DataCodes.FixPositiveMax) return WritePositiveFixInt(buffer, value);
        //    return WriteFixUInt8(buffer, value);
        //}

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public static byte ReadFixUInt8(in Span<byte> buffer, out int readSize)
        //{
        //    EnsureCapacity(buffer, 2);
        //    if (buffer[0] != DataCodes.UInt8) throw new InvalidOperationException();
        //    readSize = 2;
        //    return buffer[1];
        //}

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public static byte ReadUInt8(in Span<byte> buffer, out int readSize)
        //{
        //    if (buffer[0] != DataCodes.UInt8) throw new InvalidOperationException();
        //    readSize = 2;
        //    return buffer[1];
        //}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteFixUInt8(in Span<byte> buffer, byte value) => TryWriteFixUInt8(buffer, value, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteFixUInt8(in Span<byte> buffer, byte value, out int wroteSize)
        {
            wroteSize = 2;
            buffer[0] = DataCodes.UInt8;
            buffer[1] = value;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ReadFixUInt8(in Span<byte> buffer, out int readSize) => TryReadFixUInt8(buffer, out var result, out readSize)
            ? result
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadFixUInt8(in Span<byte> buffer, out byte value, out int readSize)
        {
            readSize = 2;
            value = buffer[1];
            return buffer[0] == DataCodes.UInt8;
        }

        // https://github.com/msgpack/msgpack/issues/164
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteUInt8(in Span<byte> buffer, byte value) => TryWriteUInt8(buffer, value, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteUInt8(in Span<byte> buffer, byte value, out int wroteSize)
        {
            if (TryWritePositiveFixInt(buffer, value, out wroteSize))
                return true;
            return TryWriteFixUInt8(buffer, value, out wroteSize);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ReadUInt8(in Span<byte> buffer, out int readSize) => TryReadUInt8(buffer, out var value, out readSize)
            ? value
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadUInt8(in Span<byte> buffer, out byte value, out int readSize)
        {
            if (TryReadFixUInt8(buffer, out value, out readSize))
                return true;
            return TryReadPositiveFixInt(buffer, out value, out readSize);
        }
    }
}
