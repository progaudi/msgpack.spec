using System;
using System.Runtime.CompilerServices;

namespace ProGaudi.MsgPack.Light
{
    /// <summary>
    /// Methods for working with PositiveFixInt
    /// </summary>
    public static partial class MsgPackBinary
    {
        /// <summary>
        /// Write fix positive int <paramref name="value"/> into <paramref name="buffer"/>.
        /// </summary>
        /// <returns>Count of bytes, written to <paramref name="buffer"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WritePositiveFixInt(Span<byte> buffer, byte value)
        {
            if (value > DataCodes.FixPositiveMax) throw WrongRangeCodeException(value, DataCodes.FixPositiveMin, DataCodes.FixPositiveMax);

            buffer[0] = value;
            return 1;
        }

        /// <summary>
        /// Tries to write fix positive int <paramref name="value"/> into <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to write.</param>
        /// <param name="value">Value to write</param>
        /// <param name="wroteSize">Count of bytes, written to <paramref name="buffer"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="buffer"/> is too small.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWritePositiveFixInt(Span<byte> buffer, byte value, out int wroteSize)
        {
            wroteSize = 1;
            if (buffer.Length < wroteSize) return false;
            if (value > DataCodes.FixPositiveMax) return false;

            buffer[0] = value;
            return true;
        }

        /// <summary>
        /// Reads uint32 from <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to read from</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="buffer"/></param>
        /// <returns>Read value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ReadPositiveFixInt(ReadOnlySpan<byte> buffer, out int readSize)
        {
            readSize = 1;
            if (buffer[0] > DataCodes.FixPositiveMax) throw WrongRangeCodeException(buffer[0], DataCodes.FixPositiveMin, DataCodes.FixPositiveMax);
            return buffer[0];
        }

        /// <summary>
        /// Tries to read from <paramref name="buffer"/>
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="value">Value, read from <paramref name="buffer"/>. If return value is false, value is unspecified.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="buffer"/>. If return value is false, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="buffer"/> is too small or <paramref name="buffer"/>[0] is greater than <see cref="DataCodes.FixPositiveMax"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadPositiveFixInt(ReadOnlySpan<byte> buffer, out byte value, out int readSize)
        {
            value = default;
            readSize = 1;
            return buffer.Length >= readSize && (value = buffer[0]) <= DataCodes.FixPositiveMax;
        }
    }
}
