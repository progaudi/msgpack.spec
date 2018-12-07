using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace ProGaudi.MsgPack
{
    /// <summary>
    /// Methods for working with unsigned int 8
    /// </summary>
    public static partial class MsgPackSpec
    {
        /// <summary>
        /// Reads uint8 from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">sequence to read from</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/></param>
        /// <returns>Read value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ReadFixUInt8(ReadOnlySequence<byte> sequence, out int readSize)
        {
            const int length = DataLengths.UInt8;

            if (sequence.First.Length >= length)
                return ReadFixUInt8(sequence.First.Span, out readSize);

            var sequenceLength = sequence.Length;
            if (sequenceLength < length)
                throw GetReadOnlySequenceIsTooShortException(length, sequenceLength);

            Span<byte> buffer = stackalloc byte[length];
            return sequence.TryRead(buffer)
                ? ReadFixUInt8(buffer, out readSize)
                : throw GetInvalidStateReadOnlySequenceException();
        }

        /// <summary>
        /// Tries to read from <paramref name="sequence"/>
        /// </summary>
        /// <param name="sequence">sequence to read from.</param>
        /// <param name="value">Value, read from <paramref name="sequence"/>. If return value is false, value is unspecified.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>. If return value is false, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="sequence"/> is too small or <paramref name="sequence"/>[0] is not <see cref="DataCodes.UInt8"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadFixUInt8(ReadOnlySequence<byte> sequence, out byte value, out int readSize)
        {
            const int length = DataLengths.UInt8;

            if (sequence.First.Length >= length)
                return TryReadFixUInt8(sequence.First.Span, out value, out readSize);

            var sequenceLength = sequence.Length;
            if (sequenceLength < length)
            {
                value = default;
                readSize = default;
                return false;
            }

            Span<byte> buffer = stackalloc byte[length];
            return sequence.TryRead(buffer)
                ? TryReadFixUInt8(buffer, out value, out readSize)
                : throw GetInvalidStateReadOnlySequenceException();
        }

        /// <summary>
        /// Read <see cref="byte"/> values from <paramref name="sequence"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ReadUInt8(ReadOnlySequence<byte> sequence, out int readSize)
        {
            if (sequence.GetFirst() == DataCodes.UInt8) return ReadFixUInt8(sequence, out readSize);
            return ReadPositiveFixInt(sequence, out readSize);
        }

        /// <summary>
        /// Tries to read <see cref="byte"/> value from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">sequence to read from.</param>
        /// <param name="value">Value, read from <paramref name="sequence"/>. If return value is false, value is unspecified.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>. If return value is false, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="sequence"/> is too small or <paramref name="sequence"/>[0] is not ok.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadUInt8(ReadOnlySequence<byte> sequence, out byte value, out int readSize)
        {
            if (TryReadFixUInt8(sequence, out value, out readSize))
                return true;
            return TryReadPositiveFixInt(sequence, out value, out readSize);
        }
    }
}
