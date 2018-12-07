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
            var index = 0;
            foreach (var memory in sequence)
            {
                for (var i = 0; i < memory.Length; i++)
                {
                    buffer[index++] = memory.Span[i];
                    if (index == length)
                    {
                        ReadNil(buffer, out readSize);
                        return;
                    }
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
        public static bool TryReadNil(ReadOnlySequence<byte> sequence, out int readSize)
        {
            const int length = DataLengths.Nil;

            if (sequence.First.Length >= length)
                return TryReadNil(sequence.First.Span, out readSize);

            Span<byte> buffer = stackalloc byte[length];
            var index = 0;
            foreach (var memory in sequence)
            {
                for (var i = 0; i < memory.Length; i++)
                {
                    buffer[index++] = memory.Span[i];
                    if (index == length)
                        return TryReadNil(buffer, out readSize);
                }
            }
            throw new IndexOutOfRangeException();
        }
    }
}
