using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;

namespace ProGaudi.MsgPack
{
    /// <summary>
    /// Methods for working with unsigned int 32
    /// </summary>
    public static partial class MsgPackSpec
    {
        /// <summary>
        /// Reads uint32 from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">sequence to read from</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/></param>
        /// <returns>Read value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ReadFixUInt32(ReadOnlySequence<byte> sequence, out int readSize)
        {
            const int length = DataLengths.UInt32;

            if (sequence.First.Length >= length)
                return ReadFixUInt32(sequence.First.Span, out readSize);

            var sequenceLength = sequence.Length;
            if (sequenceLength < length)
                throw GetReadOnlySequenceIsTooShortException(length, sequenceLength);

            Span<byte> buffer = stackalloc byte[length];
            return sequence.TryRead(buffer)
                ? ReadFixUInt32(buffer, out readSize)
                : throw GetInvalidStateReadOnlySequenceException();
        }

        /// <summary>
        /// Tries to read from <paramref name="sequence"/>
        /// </summary>
        /// <param name="sequence">sequence to read from.</param>
        /// <param name="value">Value, read from <paramref name="sequence"/>. If return value is false, value is unspecified.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>. If return value is false, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="sequence"/> is too small or <paramref name="sequence"/>[0] is not <see cref="DataCodes.UInt32"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadFixUInt32(ReadOnlySequence<byte> sequence, out uint value, out int readSize)
        {
            const int length = DataLengths.UInt32;

            if (sequence.First.Length >= length)
                return TryReadFixUInt32(sequence.First.Span, out value, out readSize);

            var sequenceLength = sequence.Length;
            if (sequenceLength < length)
            {
                value = default;
                readSize = default;
                return false;
            }

            Span<byte> buffer = stackalloc byte[length];
            return sequence.TryRead(buffer)
                ? TryReadFixUInt32(buffer, out value, out readSize)
                : throw GetInvalidStateReadOnlySequenceException();
        }

        /// <summary>
        /// Read <see cref="uint"/> values from <paramref name="sequence"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ReadUInt32(ReadOnlySequence<byte> sequence, out int readSize)
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
                    var int32 = ReadFixInt32(sequence, out readSize);
                    if (int32 < 0) return ThrowUnsignedIntException(int32);
                    return (uint) int32;

                case DataCodes.Int16:
                    var int16 = ReadFixInt32(sequence, out readSize);
                    if (int16 < 0) return ThrowUnsignedIntException(int16);
                    return (uint) int16;

                case DataCodes.Int8:
                    var int8 = ReadFixInt32(sequence, out readSize);
                    if (int8 < 0) return ThrowUnsignedIntException(int8);
                    return (uint) int8;

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

            return ThrowWrongUIntCodeException(code, DataCodes.Int8, DataCodes.Int16, DataCodes.Int32, DataCodes.UInt8, DataCodes.UInt16, DataCodes.UInt32);
        }

        /// <summary>
        /// Tries to read <see cref="uint"/> value from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">sequence to read from.</param>
        /// <param name="value">Value, read from <paramref name="sequence"/>. If return value is false, value is unspecified.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>. If return value is false, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="sequence"/> is too small or <paramref name="sequence"/>[0] is not ok.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadUInt32(ReadOnlySequence<byte> sequence, out uint value, out int readSize)
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
                case DataCodes.UInt32:
                    return TryReadFixUInt32(sequence, out value, out readSize);

                case DataCodes.UInt16:
                    result = TryReadFixUInt16(sequence, out var uint16, out readSize);
                    value = uint16;
                    return result;

                case DataCodes.UInt8:
                    result = TryReadFixUInt8(sequence, out var uint8, out readSize);
                    value = uint8;
                    return result;

                case DataCodes.Int32:
                    result = TryReadFixInt32(sequence, out var int32, out readSize) && int32 >= 0;
                    value = result ? (uint)int32 : default;
                    return result;

                case DataCodes.Int16:
                    result = TryReadFixInt16(sequence, out var int16, out readSize) && int16 >= 0;
                    value = result ? (uint)int16 : default;
                    return result;

                case DataCodes.Int8:
                    result = TryReadFixInt8(sequence, out var int8, out readSize) && int8 >= 0;
                    value = result ? (uint)int8 : default;
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
