using System;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ProGaudi.MsgPack.Light
{
    /// <summary>
    /// Methods for working with unsigned int 16
    /// </summary>
    public static partial class MsgPackBinary
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteFixUInt64(in Span<byte> buffer, ulong value) => TryWriteFixUInt64(buffer, value, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteFixUInt64(in Span<byte> buffer, ulong value, out int wroteSize)
        {
            wroteSize = 9;
            buffer[0] = DataCodes.UInt64;
            return BinaryPrimitives.TryWriteUInt64BigEndian(buffer.Slice(1), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteUInt64(in Span<byte> buffer, ulong value) => TryWriteUInt64(buffer, value, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteUInt64(in Span<byte> buffer, ulong value, out int wroteSize)
        {
            if (value > uint.MaxValue) return TryWriteFixUInt64(buffer, value, out wroteSize);
            return TryWriteUInt32(buffer, (uint)value, out wroteSize);
        }
    }
}
