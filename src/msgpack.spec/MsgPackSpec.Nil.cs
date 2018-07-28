using System;
using System.Runtime.CompilerServices;
using static ProGaudi.MsgPack.DataCodes;

namespace ProGaudi.MsgPack
{
    /// <summary>
    /// Methods for working with nil
    /// </summary>
    public static partial class MsgPackSpec
    {
        /// <summary>
        /// Writes <see cref="Nil"/> to <paramref name="buffer"/>.
        /// </summary>
        /// <returns>Count of bytes, written to <paramref name="buffer"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteNil(Span<byte> buffer)
        {
            buffer[0] = Nil;
            return 1;
        }

        /// <summary>
        /// Tries to write <see cref="Nil"/> into <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to write to.</param>
        /// <param name="wroteSize">Count of bytes, written to <paramref name="buffer"/>.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="buffer"/> is too small.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteNil(Span<byte> buffer, out int wroteSize)
        {
            wroteSize = 1;
            if (buffer.Length < wroteSize) return false;
            buffer[0] = Nil;
            return true;
        }

        /// <summary>
        /// Reads <see cref="Nil"/> from <paramref name="buffer"/>.
        /// </summary>
        /// <returns>Count of bytes, read from <paramref name="buffer"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadNil(ReadOnlySpan<byte> buffer, out int readSize)
        {
            readSize = 1;
            if (buffer[0] != Nil)
                throw WrongCodeException(buffer[0], Nil);
        }

        /// <summary>
        /// Tries to read <see cref="Nil"/> from <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="buffer"/>.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="buffer"/> is too small.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadNil(ReadOnlySpan<byte> buffer, out int readSize)
        {
            readSize = 1;
            return buffer.Length > 0 && buffer[0] == Nil;
        }
    }
}
