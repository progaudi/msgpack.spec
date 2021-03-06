using System;
using System.Buffers;

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
        public static byte ReadPositiveFixInt(ReadOnlySequence<byte> sequence, out int readSize)
        {
            const int length = DataLengths.PositiveFixInt;

            if (sequence.First.Length >= length)
                return ReadPositiveFixInt(sequence.First.Span, out readSize);

            Span<byte> buffer = stackalloc byte[length];
            return sequence.TryFillSpan(buffer)
                ? ReadPositiveFixInt(buffer, out readSize)
                : throw GetReadOnlySequenceIsTooShortException(length, sequence.Length);
        }

        /// <summary>
        /// Tries to read from <paramref name="sequence"/>
        /// </summary>
        /// <param name="sequence">Buffer to read from.</param>
        /// <param name="value">Value, read from <paramref name="sequence"/>. If return value is false, value is unspecified.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>. If return value is false, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="sequence"/> is too small or <paramref name="sequence"/>[0] is greater than <see cref="DataCodes.FixPositiveMax"/>.</returns>
        public static bool TryReadPositiveFixInt(ReadOnlySequence<byte> sequence, out byte value, out int readSize)
        {
            const int length = DataLengths.PositiveFixInt;

            if (sequence.First.Length >= length)
                return TryReadPositiveFixInt(sequence.First.Span, out value, out readSize);

            value = default;
            readSize = default;

            Span<byte> buffer = stackalloc byte[length];
            return sequence.TryFillSpan(buffer) && TryReadPositiveFixInt(buffer, out value, out readSize);
        }
    }
}
