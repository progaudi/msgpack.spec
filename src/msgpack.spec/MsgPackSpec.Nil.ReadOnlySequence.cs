using System;
using System.Buffers;
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
        /// Reads <see cref="Nil"/> from <paramref name="sequence"/>.
        /// </summary>
        /// <returns>Count of bytes, read from <paramref name="sequence"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadNil(ReadOnlySequence<byte> sequence, out int readSize)
        {
            if (sequence.IsSingleSegment)
            {
                ReadNil(sequence.First.Span, out readSize);
            }

            foreach (var memory in sequence)
            {
                if (memory.Length <= DataLengths.Nil)
                {
                    ReadNil(memory.Span, out readSize);
                }
            }
            throw new IndexOutOfRangeException();
        }

        /// <summary>
        /// Tries to read <see cref="Nil"/> from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="sequence"/> is too small.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadNil(ReadOnlySequence<byte> sequence, out int readSize)
        {
            if (sequence.IsSingleSegment) return TryReadNil(sequence.First.Span, out readSize);
            foreach (var memory in sequence)
            {
                if (memory.Length <= DataLengths.Nil)
                    return TryReadNil(memory.Span, out readSize);
            }
            throw new IndexOutOfRangeException();
        }
    }
}
