using System;
using System.Buffers;

namespace ProGaudi.MsgPack
{
    /// <summary>
    /// Methods for working with NegativeFixInt
    /// </summary>
    public static partial class MsgPackSpec
    {
        /// <summary>
        /// Reads negative fix int from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">Sequence to read from</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/></param>
        /// <returns>Read value</returns>
        public static sbyte ReadNegativeFixInt(ReadOnlySequence<byte> sequence, out int readSize)
        {
            const int length = DataLengths.NegativeFixInt;

            if (sequence.First.Length >= length)
                return ReadNegativeFixInt(sequence.First.Span, out readSize);

            Span<byte> buffer = stackalloc byte[length];
            var index = 0;
            foreach (var memory in sequence)
            {
                for (var i = 0; i < memory.Length; i++)
                {
                    buffer[index++] = memory.Span[i];
                    if (index == length)
                        return ReadNegativeFixInt(buffer, out readSize);
                }
            }
            throw new IndexOutOfRangeException();
        }

        /// <summary>
        /// Tries to read from <paramref name="sequence"/>
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="value">Value, read from <paramref name="sequence"/>. If return value is false, value is unspecified.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>. If return value is false, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="sequence"/> is too small or <paramref name="sequence"/>[0]
        /// is not between <see cref="DataCodes.FixNegativeMinSByte"/> and <see cref="DataCodes.FixNegativeMaxSByte"/>.</returns>
        public static bool TryReadNegativeFixInt(ReadOnlySequence<byte> sequence, out sbyte value, out int readSize)
        {
            const int length = DataLengths.NegativeFixInt;

            if (sequence.First.Length >= length)
                return TryReadNegativeFixInt(sequence.First.Span, out value, out readSize);

            Span<byte> buffer = stackalloc byte[length];
            var index = 0;
            foreach (var memory in sequence)
            {
                for (var i = 0; i < memory.Length; i++)
                {
                    buffer[index++] = memory.Span[i];
                    if (index == length)
                        return TryReadNegativeFixInt(buffer, out value, out readSize);
                }
            }
            throw new IndexOutOfRangeException();
        }
    }
}
