using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace ProGaudi.MsgPack
{
    /// <summary>
    /// Methods for working with double
    /// </summary>
    public static partial class MsgPackSpec
    {
        /// <summary>
        /// Reads double value from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>.</param>
        /// <returns>Double value.</returns>
        public static double ReadFixFloat64(ReadOnlySequence<byte> sequence, out int readSize)
        {
            const int length = DataLengths.Float64;

            if (sequence.First.Length >= length)
                return ReadFixFloat64(sequence.First.Span, out readSize);

            var sequenceLength = sequence.Length;
            if (sequenceLength < length)
                throw GetReadOnlySequenceIsTooShortException(length, sequenceLength);

            Span<byte> buffer = stackalloc byte[length];
            return sequence.TryRead(buffer)
                ? ReadFixFloat64(buffer, out readSize)
                : throw GetInvalidStateReadOnlySequenceException();
        }

        /// <summary>
        /// Tries to read from <paramref name="sequence"/>
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="value">Value, read from <paramref name="sequence"/>. If return value is false, value is unspecified.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>. If return value is false, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="sequence"/> is too small or <paramref name="sequence"/>[0] is not <see cref="DataCodes.Float64"/>.</returns>
        public static bool TryReadFixFloat64(ReadOnlySequence<byte> sequence, out double value, out int readSize)
        {
            const int length = DataLengths.Float64;

            if (sequence.First.Length >= length)
                return TryReadFixFloat64(sequence.First.Span, out value, out readSize);

            var sequenceLength = sequence.Length;
            if (sequenceLength < length)
            {
                value = default;
                readSize = default;
                return false;
            }

            Span<byte> buffer = stackalloc byte[length];
            return sequence.TryRead(buffer)
                ? TryReadFixFloat64(buffer, out value, out readSize)
                : throw GetInvalidStateReadOnlySequenceException();
        }

        /// <summary>
        /// Read double from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>.</param>
        /// <returns>Double value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ReadDouble(ReadOnlySequence<byte> sequence, out int readSize)
        {
            var code = sequence.GetFirst();
            switch (code)
            {
                case DataCodes.Float32:
                    return ReadFixFloat32(sequence, out readSize);
                case DataCodes.Float64:
                    return ReadFixFloat64(sequence, out readSize);
                default:
                    readSize = 0;
                    return ThrowWrongCodeException(code, DataCodes.Float64, DataCodes.Float32);
            }
        }

        /// <summary>
        /// Tries to read double value from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="value">Value read from <paramref name="sequence"/>. If return value is false, value is unspecified.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>. If return value is false, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok. <c>false</c> if sequence is too small, or <paramref name="sequence[0]"/> is not <see cref="DataCodes.Float32"/> or <see cref="DataCodes.Float64"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadDouble(ReadOnlySequence<byte> sequence, out double value, out int readSize)
        {
            if (sequence.Length < 1)
            {
                value = default;
                readSize = default;
                return false;
            }

            var code = sequence.GetFirst();
            switch (code)
            {
                case DataCodes.Float32:
                    var result = TryReadFixFloat32(sequence, out var floatValue, out readSize);
                    value = floatValue;
                    return result;
                case DataCodes.Float64:
                    return TryReadFixFloat64(sequence, out value, out readSize);
                default:
                    value = default;
                    readSize = default;
                    return false;
            }
        }
    }
}
