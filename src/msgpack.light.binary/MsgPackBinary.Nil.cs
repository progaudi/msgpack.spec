using System;
using System.Runtime.CompilerServices;

namespace ProGaudi.MsgPack.Light
{
    /// <summary>
    /// Methods for working with nil
    /// </summary>
    public static partial class MsgPackBinary
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteNil(in Span<byte> buffer) => TryWriteNil(buffer, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteNil(in Span<byte> buffer, out int wroteSize)
        {
            wroteSize = 1;
            buffer[0] = DataCodes.Nil;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadNil(in ReadOnlySpan<byte> buffer, out int readSize)
        {
            if (!TryReadNil(buffer, out readSize))
                throw new InvalidOperationException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadNil(in ReadOnlySpan<byte> buffer, out int readSize)
        {
            readSize = 1;
            return buffer[0] == DataCodes.Nil;
        }
    }
}
