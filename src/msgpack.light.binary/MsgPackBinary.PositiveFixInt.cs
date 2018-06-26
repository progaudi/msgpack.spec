using System;
using System.Runtime.CompilerServices;

namespace ProGaudi.MsgPack.Light
{
    /// <summary>
    /// Methods for working with PositiveFixInt
    /// </summary>
    public static partial class MsgPackBinary
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WritePositiveFixInt(Span<byte> buffer, byte value) => TryWritePositiveFixInt(buffer, value, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWritePositiveFixInt(Span<byte> buffer, byte value, out int wroteSize)
        {
            wroteSize = 1;
            if (value > DataCodes.FixPositiveMax) return false;

            buffer[0] = value;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ReadPositiveFixInt(ReadOnlySpan<byte> buffer, out int readSize) => TryReadPositiveFixInt(buffer, out var result, out readSize)
            ? result
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadPositiveFixInt(ReadOnlySpan<byte> buffer, out byte value, out int readSize)
        {
            readSize = 1;
            return (value = buffer[0]) <= DataCodes.FixPositiveMax;
        }
    }
}
