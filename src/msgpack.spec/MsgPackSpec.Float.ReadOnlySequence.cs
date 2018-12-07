using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ProGaudi.MsgPack
{
    /// <summary>
    /// Methods for working with float
    /// </summary>
    public static partial class MsgPackSpec
    {
        /// <summary>
        /// Reads float value from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>.</param>
        /// <returns>Float value.</returns>
        public static float ReadFixFloat32(ReadOnlySequence<byte> sequence, out int readSize)
        {
            const int length = DataLengths.Float32;

            if (sequence.First.Length >= length)
                return ReadFixFloat32(sequence.First.Span, out readSize);

            var sequenceLength = sequence.Length;
            if (sequenceLength < length)
                throw GetReadOnlySequenceIsTooShortException(length, sequenceLength);

            Span<byte> buffer = stackalloc byte[length];
            return sequence.TryRead(buffer)
                ? ReadFixFloat32(buffer, out readSize)
                : throw GetInvalidStateReadOnlySequenceException();
        }

        /// <summary>
        /// Tries to read from <paramref name="sequence"/>
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="value">Value, read from <paramref name="sequence"/>. If return value is false, value is unspecified.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>. If return value is false, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="sequence"/> is too small or <paramref name="sequence"/>[0] is not <see cref="DataCodes.Float32"/>.</returns>
        public static bool TryReadFixFloat32(ReadOnlySequence<byte> sequence, out float value, out int readSize)
        {
            const int length = DataLengths.Float32;

            if (sequence.First.Length >= length)
                return TryReadFixFloat32(sequence.First.Span, out value, out readSize);

            var sequenceLength = sequence.Length;
            if (sequenceLength < length)
            {
                value = default;
                readSize = default;
                return false;
            }

            Span<byte> buffer = stackalloc byte[length];
            return sequence.TryRead(buffer)
                ? TryReadFixFloat32(buffer, out value, out readSize)
                : throw GetInvalidStateReadOnlySequenceException();
        }

        /// <summary>
        /// Calls <see cref="ReadFixFloat32"/>. Provided for consistency with other types.
        /// </summary>
        public static float ReadFloat(ReadOnlySequence<byte> sequence, out int readSize) => ReadFixFloat32(sequence, out readSize);

        /// <summary>
        /// Calls <see cref="TryReadFixFloat32"/>. Provided for consistency with other types.
        /// </summary>
        public static bool TryReadFloat(ReadOnlySequence<byte> sequence, out float value, out int readSize) => TryReadFixFloat32(sequence, out value, out readSize);
    }
}
