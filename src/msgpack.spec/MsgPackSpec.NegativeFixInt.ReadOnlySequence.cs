using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace ProGaudi.MsgPack
{
    /// <summary>
    /// Methods for working with NegativeFixInt
    /// </summary>
    public static partial class MsgPackSpec
    {
        /// <summary>
        /// Reads uint32 from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">Sequence to read from</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/></param>
        /// <returns>Read value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte ReadNegativeFixInt(ReadOnlySequence<byte> sequence, out int readSize)
        {
            if (sequence.IsSingleSegment) return ReadNegativeFixInt(sequence.First.Span, out readSize);
            foreach (var memory in sequence)
            {
                if (memory.Length <= DataLengths.NegativeFixInt)
                    return ReadNegativeFixInt(memory.Span, out readSize);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadNegativeFixInt(ReadOnlySequence<byte> sequence, out sbyte value, out int readSize)
        {
            if (sequence.IsSingleSegment) return TryReadNegativeFixInt(sequence.First.Span, out value, out readSize);
            foreach (var memory in sequence)
            {
                if (memory.Length <= DataLengths.NegativeFixInt)
                    return TryReadNegativeFixInt(memory.Span, out value, out readSize);
            }
            throw new IndexOutOfRangeException();
        }
    }
}
