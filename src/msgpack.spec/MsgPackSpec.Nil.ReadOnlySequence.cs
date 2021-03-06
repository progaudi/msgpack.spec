using System;
using System.Buffers;
using static ProGaudi.MsgPack.DataCodes;

namespace ProGaudi.MsgPack
{
    /// <summary>
    /// Methods for working with nil
    /// </summary>
    public static partial class MsgPackSpec
    {
        /// <summary>
        /// Reads <see cref="Nil"/> from <paramref name="sequence"/>.
        /// </summary>
        /// <returns>Count of bytes, read from <paramref name="sequence"/>.</returns>
        public static void ReadNil(ReadOnlySequence<byte> sequence, out int readSize)
        {
            const int length = DataLengths.Nil;

            if (sequence.First.Length >= length)
            {
                ReadNil(sequence.First.Span, out readSize);
                return;
            }

            Span<byte> buffer = stackalloc byte[length];
            if (!sequence.TryFillSpan(buffer)) throw GetReadOnlySequenceIsTooShortException(length, sequence.Length);
            ReadNil(buffer, out readSize);
        }

        /// <summary>
        /// Tries to read <see cref="Nil"/> from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="sequence"/> is too small.</returns>
        public static bool TryReadNil(ReadOnlySequence<byte> sequence, out int readSize)
        {
            const int length = DataLengths.Nil;

            if (sequence.First.Length >= length)
                return TryReadNil(sequence.First.Span, out readSize);

            readSize = default;

            Span<byte> buffer = stackalloc byte[length];
            return sequence.TryFillSpan(buffer) && TryReadNil(buffer, out readSize);
        }
    }
}
