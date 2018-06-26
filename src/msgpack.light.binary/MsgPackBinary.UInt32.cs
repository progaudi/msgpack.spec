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
            BinaryPrimitives.WriteUInt32BigEndian(buffer.Slice(1), value);
            return true;
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
