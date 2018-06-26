using System;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;

namespace ProGaudi.MsgPack.Light
{
    /// <summary>
    /// Methods for working with unsigned int 32
    /// </summary>
    public static partial class MsgPackBinary
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteFixUInt32(in Span<byte> buffer, uint value) => TryWriteFixUInt32(buffer, value, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteFixUInt32(in Span<byte> buffer, uint value, out int wroteSize)
        {
            wroteSize = 5;
            buffer[0] = DataCodes.UInt32;
            return BinaryPrimitives.TryWriteUInt32BigEndian(buffer.Slice(1), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ReadFixUInt32(in Span<byte> buffer, out int readSize) => TryReadFixUInt32(buffer, out var result, out readSize)
            ? result
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadFixUInt32(in Span<byte> buffer, out uint value, out int readSize)
        {
            readSize = 5;
            var result = buffer[0] == DataCodes.UInt32;
            return BinaryPrimitives.TryReadUInt32BigEndian(buffer.Slice(1), out value) && result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteUInt32(in Span<byte> buffer, uint value) => TryWriteUInt32(buffer, value, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteUInt32(in Span<byte> buffer, uint value, out int wroteSize)
        {
            if (value > ushort.MaxValue) return TryWriteFixUInt32(buffer, value, out wroteSize);
            return TryWriteUInt16(buffer, (ushort)value, out wroteSize);
        }
    }
}
