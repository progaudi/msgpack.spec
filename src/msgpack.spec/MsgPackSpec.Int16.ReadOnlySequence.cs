using System;
using System.Buffers;
using System.Buffers.Binary;

namespace ProGaudi.MsgPack
{
    /// <summary>
    /// Methods for working with signed int 16
    /// </summary>
    public static partial class MsgPackSpec
    {
        /// <summary>
        /// Reads int16 from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">Buffer to read from</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/></param>
        /// <returns>Read value</returns>
        public static short ReadFixInt16(ReadOnlySequence<byte> sequence, out int readSize)
        {
            const int length = DataLengths.Int16;

            if (sequence.First.Length >= length)
                return ReadFixInt16(sequence.First.Span, out readSize);

            var sequenceLength = sequence.Length;
            if (sequenceLength < length)
                throw GetReadOnlySequenceIsTooShortException(length, sequenceLength);

            Span<byte> buffer = stackalloc byte[length];
            return sequence.TryRead(buffer)
                ? ReadFixInt16(buffer, out readSize)
                : throw GetInvalidStateReadOnlySequenceException();
        }

        /// <summary>
        /// Tries to read from <paramref name="sequence"/>
        /// </summary>
        /// <param name="sequence">Buffer to read from.</param>
        /// <param name="value">Value, read from <paramref name="sequence"/>. If return value is false, value is unspecified.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>. If return value is false, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="sequence"/> is too small or <paramref name="sequence"/>[0] is not <see cref="DataCodes.Int16"/>.</returns>
        public static bool TryReadFixInt16(ReadOnlySequence<byte> sequence, out short value, out int readSize)
        {
            const int length = DataLengths.Int16;

            if (sequence.First.Length >= length)
                return TryReadFixInt16(sequence.First.Span, out value, out readSize);

            var sequenceLength = sequence.Length;
            if (sequenceLength < length)
            {
                value = default;
                readSize = default;
                return false;
            }

            Span<byte> buffer = stackalloc byte[length];
            return sequence.TryRead(buffer)
                ? TryReadFixInt16(buffer, out value, out readSize)
                : throw GetInvalidStateReadOnlySequenceException();
        }

        /// <summary>
        /// Read <see cref="short"/> values from <paramref name="sequence"/>
        /// </summary>
        public static short ReadInt16(ReadOnlySequence<byte> sequence, out int readSize)
        {
            if (sequence.IsEmpty) ThrowCantReadEmptyBufferException();
            var code = sequence.GetFirst();

            switch (code)
            {
                case DataCodes.Int16:
                    return ReadFixInt16(sequence, out readSize);

                case DataCodes.Int8:
                    return ReadFixInt8(sequence, out readSize);

                case DataCodes.UInt16:
                    var value = ReadFixUInt16(sequence, out readSize);
                    if (value > short.MaxValue) ThrowValueIsTooLargeException(value, short.MaxValue);
                    return (short) value;

                case DataCodes.UInt8:
                    return ReadFixUInt8(sequence, out readSize);
            }

            if (TryReadPositiveFixInt(sequence, out var positive, out readSize))
            {
                return positive;
            }

            if (TryReadNegativeFixInt(sequence, out var negative, out readSize))
            {
                return negative;
            }

            return ThrowWrongIntCodeException(code, DataCodes.Int8, DataCodes.Int16, DataCodes.UInt8, DataCodes.UInt16);
        }

        /// <summary>
        /// Tries to read <see cref="short"/> value from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">Buffer to read from.</param>
        /// <param name="value">Value, read from <paramref name="sequence"/>. If return value is false, value is unspecified.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>. If return value is false, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="sequence"/> is too small or <paramref name="sequence"/>[0] is not ok.</returns>
        public static bool TryReadInt16(ReadOnlySequence<byte> sequence, out short value, out int readSize)
        {
            if (sequence.IsEmpty)
            {
                value = default;
                readSize = default;
                return false;
            }

            var code = sequence.GetFirst();
            bool result;

            switch (code)
            {
                case DataCodes.Int16:
                    return TryReadFixInt16(sequence, out value, out readSize);

                case DataCodes.Int8:
                    result = TryReadFixInt8(sequence, out var int8, out readSize);
                    value = int8;
                    return result;

                case DataCodes.UInt16:
                    result = TryReadFixUInt16(sequence, out var uint16, out readSize) && uint16 <= short.MaxValue;
                    value = result ? (short)uint16 : default;
                    return result;

                case DataCodes.UInt8:
                    result = TryReadFixUInt8(sequence, out var uint8, out readSize);
                    value = uint8;
                    return result;
            }

            if (TryReadPositiveFixInt(sequence, out var positive, out readSize))
            {
                value = positive;
                return true;
            }

            if (TryReadNegativeFixInt(sequence, out var negative, out readSize))
            {
                value = negative;
                return true;
            }

            value = default;
            return false;
        }
    }
}
