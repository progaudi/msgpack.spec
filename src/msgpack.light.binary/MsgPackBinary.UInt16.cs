using System;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;

namespace ProGaudi.MsgPack.Light
{
    /// <summary>
    /// Methods for working with unsigned int 16
    /// </summary>
    public static partial class MsgPackBinary
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteFixUInt16(in Span<byte> buffer, ushort value) => TryWriteFixUInt16(buffer, value, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteFixUInt16(in Span<byte> buffer, ushort value, out int wroteSize)
        {
            wroteSize = 3;
            buffer[0] = DataCodes.UInt16;
            BinaryPrimitives.WriteUInt16BigEndian(buffer.Slice(1), value);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteUInt16(in Span<byte> buffer, ushort value) => TryWriteUInt16(buffer, value, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteUInt16(in Span<byte> buffer, ushort value, out int wroteSize)
        {
            if (value > byte.MaxValue) return TryWriteFixUInt16(buffer, value, out wroteSize);
            return TryWriteUInt8(buffer, (byte)value, out wroteSize);
        }
    }
}
