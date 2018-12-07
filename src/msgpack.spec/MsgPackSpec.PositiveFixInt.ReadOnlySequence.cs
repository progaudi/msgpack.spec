using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace ProGaudi.MsgPack
{
    /// <summary>
    /// Methods for working with PositiveFixInt
    /// </summary>
    public static partial class MsgPackSpec
    {
        /// <summary>
        /// Reads positive fix int from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">Buffer to read from</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/></param>
        /// <returns>Read value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ReadPositiveFixInt(ReadOnlySequence<byte> sequence, out int readSize)
        {
            const int length = DataLengths.PositiveFixInt;

            if (sequence.First.Length >= length)
                return ReadPositiveFixInt(sequence.First.Span, out readSize);

            Span<byte> buffer = stackalloc byte[length];
            var index = 0;
            foreach (var memory in sequence)
            {
                for (var i = 0; i < memory.Length; i++)
                {
                    buffer[index++] = memory.Span[i];
                    if (index == length)
                        return ReadPositiveFixInt(buffer, out readSize);
                }
            }
            throw new IndexOutOfRangeException();
        }

        /// <summary>
        /// Tries to read from <paramref name="sequence"/>
        /// </summary>
        /// <param name="sequence">Buffer to read from.</param>
        /// <param name="value">Value, read from <paramref name="sequence"/>. If return value is false, value is unspecified.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>. If return value is false, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="sequence"/> is too small or <paramref name="sequence"/>[0] is greater than <see cref="DataCodes.FixPositiveMax"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadPositiveFixInt(ReadOnlySequence<byte> sequence, out byte value, out int readSize)
        {
            const int length = DataLengths.PositiveFixInt;

            if (sequence.First.Length >= length)
                return TryReadPositiveFixInt(sequence.First.Span, out value, out readSize);

            Span<byte> buffer = stackalloc byte[length];
            var index = 0;
            foreach (var memory in sequence)
            {
                for (var i = 0; i < memory.Length; i++)
                {
                    buffer[index++] = memory.Span[i];
                    if (index == length)
                        return TryReadPositiveFixInt(buffer, out value, out readSize);
                }
            }
            throw new IndexOutOfRangeException();
        }
    }
}