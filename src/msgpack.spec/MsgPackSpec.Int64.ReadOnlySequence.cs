using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;

namespace ProGaudi.MsgPack
{
    /// <summary>
    /// Methods for working with signed int 64
    /// </summary>
    public static partial class MsgPackSpec
    {
        /// <summary>
        /// Reads int64 from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">sequence to read from</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/></param>
        /// <returns>Read value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ReadFixInt64(ReadOnlySequence<byte> sequence, out int readSize)
        {
            const int length = DataLengths.Int64;

            if (sequence.First.Length >= length)
                return ReadFixInt64(sequence.First.Span, out readSize);

            var sequenceLength = sequence.Length;
            if (sequenceLength < length)
                throw GetReadOnlySequenceIsTooShortException(length, sequenceLength);

            Span<byte> buffer = stackalloc byte[length];
            return sequence.TryRead(buffer)
                ? ReadFixInt64(buffer, out readSize)
                : throw GetInvalidStateReadOnlySequenceException();
        }

        /// <summary>
        /// Tries to read from <paramref name="sequence"/>
        /// </summary>
        /// <param name="sequence">sequence to read from.</param>
        /// <param name="value">Value, read from <paramref name="sequence"/>. If return value is false, value is unspecified.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>. If return value is false, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="sequence"/> is too small or <paramref name="sequence"/>[0] is not <see cref="DataCodes.Int64"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadFixInt64(ReadOnlySequence<byte> sequence, out long value, out int readSize)
        {
            const int length = DataLengths.Int64;

            if (sequence.First.Length >= length)
                return TryReadFixInt64(sequence.First.Span, out value, out readSize);

            var sequenceLength = sequence.Length;
            if (sequenceLength < length)
            {
                value = default;
                readSize = default;
                return false;
            }

            Span<byte> buffer = stackalloc byte[length];
            return sequence.TryRead(buffer)
                ? TryReadFixInt64(buffer, out value, out readSize)
                : throw GetInvalidStateReadOnlySequenceException();
        }

        /// <summary>
        /// Read <see cref="long"/> values from <paramref name="sequence"/>
        /// </summary>
        public static long ReadInt64(ReadOnlySequence<byte> sequence, out int readSize)
        {
            if (sequence.IsEmpty) ThrowCantReadEmptyBufferException();
            var code = sequence.GetFirst();

            switch (code)
            {
                case DataCodes.Int64:
                    return ReadFixInt64(sequence, out readSize);

                case DataCodes.Int32:
                    return ReadFixInt32(sequence, out readSize);

                case DataCodes.Int16:
                    return ReadFixInt16(sequence, out readSize);

                case DataCodes.Int8:
                    return ReadFixInt8(sequence, out readSize);

                case DataCodes.UInt64:
                    var value = ReadFixUInt64(sequence, out readSize);
                    if (value > long.MaxValue) ThrowValueIsTooLargeException(value, long.MaxValue);
                    return (long) value;

                case DataCodes.UInt32:
                    return ReadFixUInt32(sequence, out readSize);

                case DataCodes.UInt16:
                    return ReadFixUInt16(sequence, out readSize);

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

            return ThrowWrongIntCodeException(code, DataCodes.Int8, DataCodes.Int16, DataCodes.Int32, DataCodes.Int64, DataCodes.UInt8, DataCodes.UInt16, DataCodes.UInt32, DataCodes.UInt64);
        }

        /// <summary>
        /// Tries to read <see cref="long"/> value from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">sequence to read from.</param>
        /// <param name="value">Value, read from <paramref name="sequence"/>. If return value is false, value is unspecified.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>. If return value is false, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="sequence"/> is too small or <paramref name="sequence"/>[0] is not ok.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadInt64(ReadOnlySequence<byte> sequence, out long value, out int readSize)
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
                case DataCodes.Int64:
                    return TryReadFixInt64(sequence, out value, out readSize);

                case DataCodes.Int32:
                    result = TryReadFixInt32(sequence, out var int32, out readSize);
                    value = int32;
                    return result;

                case DataCodes.Int16:
                    result = TryReadFixInt16(sequence, out var int16, out readSize);
                    value = int16;
                    return result;

                case DataCodes.Int8:
                    result = TryReadFixInt8(sequence, out var int8, out readSize);
                    value = int8;
                    return result;

                case DataCodes.UInt64:
                    result = TryReadFixUInt64(sequence, out var uint64, out readSize) && uint64 <= long.MaxValue;
                    value = result ? (long)uint64 : default;
                    return result;

                case DataCodes.UInt32:
                    result = TryReadFixUInt32(sequence, out var uint32, out readSize);
                    value = uint32;
                    return result;

                case DataCodes.UInt16:
                    result = TryReadFixUInt16(sequence, out var uint16, out readSize);
                    value = uint16;
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
