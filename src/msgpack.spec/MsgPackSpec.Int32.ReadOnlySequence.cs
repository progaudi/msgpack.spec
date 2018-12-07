using System;
using System.Buffers;
using System.Buffers.Binary;

namespace ProGaudi.MsgPack
{
    /// <summary>
    /// Methods for working with signed int 32
    /// </summary>
    public static partial class MsgPackSpec
    {
        /// <summary>
        /// Reads int32 from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">sequence to read from</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/></param>
        /// <returns>Read value</returns>
        public static int ReadFixInt32(ReadOnlySequence<byte> sequence, out int readSize)
        {
            const int length = DataLengths.Int32;

            if (sequence.First.Length >= length)
                return ReadFixInt32(sequence.First.Span, out readSize);

            Span<byte> buffer = stackalloc byte[length];
            var index = 0;
            foreach (var memory in sequence)
            {
                for (var i = 0; i < memory.Length; i++)
                {
                    buffer[index++] = memory.Span[i];
                    if (index == length)
                        return ReadFixInt32(buffer, out readSize);
                }
            }
            throw new IndexOutOfRangeException();
        }

        /// <summary>
        /// Tries to read from <paramref name="sequence"/>
        /// </summary>
        /// <param name="sequence">sequence to read from.</param>
        /// <param name="value">Value, read from <paramref name="sequence"/>. If return value is false, value is unspecified.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>. If return value is false, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="sequence"/> is too small or <paramref name="sequence"/>[0] is not <see cref="DataCodes.Int32"/>.</returns>
        public static bool TryReadFixInt32(ReadOnlySequence<byte> sequence, out int value, out int readSize)
        {
            const int length = DataLengths.Int32;

            if (sequence.First.Length >= length)
                return TryReadFixInt32(sequence.First.Span, out value, out readSize);

            Span<byte> buffer = stackalloc byte[length];
            var index = 0;
            foreach (var memory in sequence)
            {
                for (var i = 0; i < memory.Length; i++)
                {
                    buffer[index++] = memory.Span[i];
                    if (index == length)
                        return TryReadFixInt32(buffer, out value, out readSize);
                }
            }
            throw new IndexOutOfRangeException();
        }

        /// <summary>
        /// Read <see cref="int"/> values from <paramref name="sequence"/>
        /// </summary>
        public static int ReadInt32(ReadOnlySequence<byte> sequence, out int readSize)
        {
            if (sequence.IsEmpty)
            {
                readSize = 0;
                return ThrowCantReadEmptyBufferException();
            }

            var code = sequence.GetFirst();

            switch (code)
            {
                case DataCodes.Int32:
                    return ReadFixInt32(sequence, out readSize);

                case DataCodes.Int16:
                    return ReadFixInt16(sequence, out readSize);

                case DataCodes.Int8:
                    return ReadFixInt8(sequence, out readSize);

                case DataCodes.UInt32:
                    var value = ReadFixUInt32(sequence, out readSize);
                    if (value > int.MaxValue) return ThrowValueIsTooLargeException(value, int.MaxValue);
                    return (int) value;

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

            return ThrowWrongIntCodeException(code, DataCodes.Int8, DataCodes.Int16, DataCodes.Int32, DataCodes.UInt8, DataCodes.UInt16, DataCodes.UInt32);
        }

        /// <summary>
        /// Tries to read <see cref="int"/> value from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">sequence to read from.</param>
        /// <param name="value">Value, read from <paramref name="sequence"/>. If return value is false, value is unspecified.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>. If return value is false, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="sequence"/> is too small or <paramref name="sequence"/>[0] is not ok.</returns>
        public static bool TryReadInt32(ReadOnlySequence<byte> sequence, out int value, out int readSize)
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
                case DataCodes.Int32:
                    return TryReadFixInt32(sequence, out value, out readSize);

                case DataCodes.Int16:
                    result = TryReadFixInt16(sequence, out var int16, out readSize);
                    value = int16;
                    return result;

                case DataCodes.Int8:
                    result = TryReadFixInt8(sequence, out var int8, out readSize);
                    value = int8;
                    return result;

                case DataCodes.UInt32:
                    result = TryReadFixUInt32(sequence, out var uint32, out readSize) && uint32 <= int.MaxValue;
                    value = result ? (int)uint32 : default;
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
