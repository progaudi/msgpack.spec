using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;

namespace ProGaudi.MsgPack
{
    /// <summary>
    /// Methods for working with unsigned int 16
    /// </summary>
    public static partial class MsgPackSpec
    {
        /// <summary>
        /// Reads uint16 from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">sequence to read from</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/></param>
        /// <returns>Read value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ReadFixUInt16(ReadOnlySequence<byte> sequence, out int readSize)
        {
            const int length = DataLengths.UInt16;

            if (sequence.First.Length >= length)
                return ReadFixUInt16(sequence.First.Span, out readSize);

            var sequenceLength = sequence.Length;
            if (sequenceLength < length)
                throw GetReadOnlySequenceIsTooShortException(length, sequenceLength);

            Span<byte> buffer = stackalloc byte[length];
            return sequence.TryRead(buffer)
                ? ReadFixUInt16(buffer, out readSize)
                : throw GetInvalidStateReadOnlySequenceException();
        }

        /// <summary>
        /// Tries to read from <paramref name="sequence"/>
        /// </summary>
        /// <param name="sequence">sequence to read from.</param>
        /// <param name="value">Value, read from <paramref name="sequence"/>. If return value is false, value is unspecified.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>. If return value is false, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="sequence"/> is too small or <paramref name="sequence"/>[0] is not <see cref="DataCodes.UUInt16"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadFixUInt16(ReadOnlySequence<byte> sequence, out ushort value, out int readSize)
        {
            const int length = DataLengths.UInt16;

            if (sequence.First.Length >= length)
                return TryReadFixUInt16(sequence.First.Span, out value, out readSize);

            var sequenceLength = sequence.Length;
            if (sequenceLength < length)
            {
                value = default;
                readSize = default;
                return false;
            }

            Span<byte> buffer = stackalloc byte[length];
            return sequence.TryRead(buffer)
                ? TryReadFixUInt16(buffer, out value, out readSize)
                : throw GetInvalidStateReadOnlySequenceException();
        }

        /// <summary>
        /// Read <see cref="ushort"/> values from <paramref name="sequence"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ReadUInt16(ReadOnlySequence<byte> sequence, out int readSize)
        {
            if (sequence.IsEmpty) ThrowCantReadEmptyBufferException();
            var code = sequence.GetFirst();

            switch (code)
            {
                case DataCodes.Int16:
                    var int16 = ReadFixInt32(sequence, out readSize);
                    if (int16 < 0) ThrowUnsignedIntException(int16);
                    return (ushort) int16;

                case DataCodes.Int8:
                    var int8 = ReadFixInt32(sequence, out readSize);
                    if (int8 < 0) ThrowUnsignedIntException(int8);
                    return (ushort) int8;

                case DataCodes.UInt16:
                    return ReadFixUInt16(sequence, out readSize);

                case DataCodes.UInt8:
                    return ReadFixUInt8(sequence, out readSize);
            }

            if (TryReadPositiveFixInt(sequence, out var positive, out readSize))
            {
                return positive;
            }

            return ThrowWrongUIntCodeException(code, DataCodes.Int8, DataCodes.Int16, DataCodes.UInt8, DataCodes.UInt16, DataCodes.UInt32);
        }

        /// <summary>
        /// Tries to read <see cref="ushort"/> value from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">sequence to read from.</param>
        /// <param name="value">Value, read from <paramref name="sequence"/>. If return value is false, value is unspecified.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>. If return value is false, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="sequence"/> is too small or <paramref name="sequence"/>[0] is not ok.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadUInt16(ReadOnlySequence<byte> sequence, out ushort value, out int readSize)
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
                case DataCodes.UInt16:
                    return TryReadFixUInt16(sequence, out value, out readSize);

                case DataCodes.UInt8:
                    result = TryReadFixUInt8(sequence, out var uint8, out readSize);
                    value = uint8;
                    return result;

                case DataCodes.Int16:
                    result = TryReadFixInt16(sequence, out var int16, out readSize) && int16 >= 0;
                    value = result ? (ushort)int16 : default;
                    return result;

                case DataCodes.Int8:
                    result = TryReadFixInt8(sequence, out var int8, out readSize) && int8 >= 0;
                    value = result ? (ushort)int8 : default;
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
