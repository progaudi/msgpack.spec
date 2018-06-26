using System;
using System.Runtime.CompilerServices;

namespace ProGaudi.MsgPack.Light
{
    /// <summary>
    /// Methods for working with boolean
    /// </summary>
    public static partial class MsgPackBinary
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteBoolean(Span<byte> buffer, bool value) => TryWriteBoolean(buffer, value, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteBoolean(Span<byte> buffer, bool value, out int wroteSize)
        {
            wroteSize = 1;
            buffer[0] = value ? DataCodes.True : DataCodes.False;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadBoolean(ReadOnlySpan<byte> buffer, out int readSize) => TryReadBoolean(buffer, out var result, out readSize)
            ? result
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadBoolean(ReadOnlySpan<byte> buffer, out bool value, out int readSize)
        {
            readSize = 1;
            value = buffer[0] == DataCodes.True;
            return value || buffer[0] == DataCodes.False;
        }
    }
}
