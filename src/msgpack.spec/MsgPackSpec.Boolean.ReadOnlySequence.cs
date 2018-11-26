using System;
using System.Buffers;
using System.Runtime.CompilerServices;

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadBoolean(ReadOnlySequence<byte> sequence, out int readSize)
        {
            if (sequence.IsSingleSegment) return ReadBoolean(sequence.First.Span, out readSize);
            foreach (var memory in sequence)
            {
                if (memory.Length <= DataLengths.Boolean)
                    return ReadBoolean(memory.Span, out readSize);
            }
            throw new IndexOutOfRangeException();
        }

        /// <summary>
        /// Tries to write boolean value into <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">Sequence to read form.</param>
        /// <param name="value">Result. If return false is <c>false</c>, value is unspecified.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="sequence"/> is too small or <paramref name="sequence"/>[0] is not <see cref="DataCodes.True"/> or <see cref="DataCodes.False"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadBoolean(ReadOnlySequence<byte> sequence, out bool value, out int readSize)
        {
            if (sequence.IsSingleSegment) return TryReadBoolean(sequence.First.Span, out value, out readSize);
            foreach (var memory in sequence)
            {
                if (memory.Length <= DataLengths.Boolean)
                    return TryReadBoolean(memory.Span, out value, out readSize);
            }
            throw new IndexOutOfRangeException();
        }
    }
}
