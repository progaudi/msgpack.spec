using System;
using System.Buffers;

namespace ProGaudi.MsgPack
{
    /// <summary>
    /// Methods for working with boolean
    /// </summary>
    public static partial class MsgPackSpec
    {
        /// <summary>
        /// Read boolean value from sequence.
        /// </summary>
        /// <param name="sequence">Sequence to read from</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>.</param>
        /// <returns>Boolean value.</returns>
        public static bool ReadBoolean(ReadOnlySequence<byte> sequence, out int readSize)
        {
            const int length = DataLengths.Boolean;

            if (sequence.First.Length >= length)
                return ReadBoolean(sequence.First.Span, out readSize);

            Span<byte> buffer = stackalloc byte[length];
            var index = 0;
            foreach (var memory in sequence)
            {
                for (var i = 0; i < memory.Length; i++)
                {
                    buffer[index++] = memory.Span[i];
                    if (index == length)
                        return ReadBoolean(buffer, out readSize);
                }
            }
            throw new IndexOutOfRangeException();
        }

        /// <summary>
        /// Tries to read boolean value into <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">Sequence to read form.</param>
        /// <param name="value">Result. If return false is <c>false</c>, value is unspecified.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="sequence"/> is too small or <paramref name="sequence"/>[0] is not <see cref="DataCodes.True"/> or <see cref="DataCodes.False"/>.</returns>
        public static bool TryReadBoolean(ReadOnlySequence<byte> sequence, out bool value, out int readSize)
        {
            const int length = DataLengths.Boolean;

            if (sequence.First.Length >= length)
                return TryReadBoolean(sequence.First.Span, out value, out readSize);

            Span<byte> buffer = stackalloc byte[length];
            var index = 0;
            foreach (var memory in sequence)
            {
                for (var i = 0; i < memory.Length; i++)
                {
                    buffer[index++] = memory.Span[i];
                    if (index == length)
                        return TryReadBoolean(buffer, out value, out readSize);
                }
            }
            throw new IndexOutOfRangeException();
        }
    }
}
