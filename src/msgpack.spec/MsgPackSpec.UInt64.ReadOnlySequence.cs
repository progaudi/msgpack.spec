using System;
using System.Buffers;

namespace ProGaudi.MsgPack
{
    /// <summary>
    /// Methods for working with unsigned int 64
    /// </summary>
    public static partial class MsgPackSpec
    {
        /// <summary>
        /// Reads uint64 from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">sequence to read from</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/></param>
        /// <returns>Read value</returns>
        public static ulong ReadFixUInt64(ReadOnlySequence<byte> sequence, out int readSize)
        {
            const int length = DataLengths.UInt64;

            if (sequence.First.Length >= length)
                return ReadFixUInt64(sequence.First.Span, out readSize);

            Span<byte> buffer = stackalloc byte[length];
            return sequence.TryRead(buffer)
                ? ReadFixUInt64(buffer, out readSize)
                : throw GetReadOnlySequenceIsTooShortException(length, sequence.Length);
        }

        /// <summary>
        /// Tries to read from <paramref name="sequence"/>
        /// </summary>
        /// <param name="sequence">sequence to read from.</param>
        /// <param name="value">Value, read from <paramref name="sequence"/>. If return value is false, value is unspecified.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>. If return value is false, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="sequence"/> is too small or <paramref name="sequence"/>[0] is not <see cref="DataCodes.UInt64"/>.</returns>
        public static bool TryReadFixUInt64(ReadOnlySequence<byte> sequence, out ulong value, out int readSize)
        {
            const int length = DataLengths.UInt64;

            if (sequence.First.Length >= length)
                return TryReadFixUInt64(sequence.First.Span, out value, out readSize);

            value = default;
            readSize = default;

            Span<byte> buffer = stackalloc byte[length];
            return sequence.TryRead(buffer) && TryReadFixUInt64(buffer, out value, out readSize);
        }

        /// <summary>
        /// Read <see cref="ulong"/> values from <paramref name="sequence"/>
        /// </summary>
        public static ulong ReadUInt64(ReadOnlySequence<byte> sequence, out int readSize)
        {
            if (sequence.IsEmpty)
            {
                readSize = 0;
                return ThrowCantReadEmptyBufferException();
            }

            var code = sequence.GetFirst();

            switch (code)
            {
                case DataCodes.Int64:
                    var int64 = ReadFixInt64(sequence, out readSize);
                    if (int64 < 0) return ThrowUnsignedIntException(int64);
                    return (ulong) int64;

                case DataCodes.Int32:
                    var int32 = ReadFixInt32(sequence, out readSize);
                    if (int32 < 0) return ThrowUnsignedIntException(int32);
                    return (ulong) int32;

                case DataCodes.Int16:
                    var int16 = ReadFixInt32(sequence, out readSize);
                    if (int16 < 0) return ThrowUnsignedIntException(int16);
                    return (ulong) int16;

                case DataCodes.Int8:
                    var int8 = ReadFixInt32(sequence, out readSize);
                    if (int8 < 0) return ThrowUnsignedIntException(int8);
                    return (ulong) int8;

                case DataCodes.UInt64:
                    return ReadFixUInt64(sequence, out readSize);

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

            return ThrowWrongUIntCodeException(code, DataCodes.Int8, DataCodes.Int16, DataCodes.Int32, DataCodes.Int64, DataCodes.UInt8, DataCodes.UInt16, DataCodes.UInt32, DataCodes.UInt64);
        }

        /// <summary>
        /// Tries to read <see cref="ulong"/> value from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">sequence to read from.</param>
        /// <param name="value">Value, read from <paramref name="sequence"/>. If return value is false, value is unspecified.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>. If return value is false, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="sequence"/> is too small or <paramref name="sequence"/>[0] is not ok.</returns>
        public static bool TryReadUInt64(ReadOnlySequence<byte> sequence, out ulong value, out int readSize)
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
                case DataCodes.UInt64:
                    return TryReadFixUInt64(sequence, out value, out readSize);

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

                case DataCodes.Int64:
                    result = TryReadFixInt64(sequence, out var int64, out readSize) && int64 >= 0;
                    value = result ? (ulong)int64 : default;
                    return result;

                case DataCodes.Int32:
                    result = TryReadFixInt32(sequence, out var int32, out readSize) && int32 >= 0;
                    value = result ? (ulong)int32 : default;
                    return result;

                case DataCodes.Int16:
                    result = TryReadFixInt16(sequence, out var int16, out readSize) && int16 >= 0;
                    value = result ? (ulong)int16 : default;
                    return result;

                case DataCodes.Int8:
                    result = TryReadFixInt8(sequence, out var int8, out readSize) && int8 >= 0;
                    value = result ? (ulong)int8 : default;
                    return result;
            }

            if (TryReadPositiveFixInt(sequence, out var positive, out readSize))
            {
                value = positive;
                return true;
            }

            value = default;
            return false;
        }
    }
}
