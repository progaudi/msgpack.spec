using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace ProGaudi.MsgPack
{
    /// <summary>
    /// Methods for working with signed int 8
    /// </summary>
    public static partial class MsgPackSpec
    {
        /// <summary>
        /// Reads int8 from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">sequence to read from</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/></param>
        /// <returns>Read value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte ReadFixInt8(ReadOnlySequence<byte> sequence, out int readSize)
        {
            const int length = DataLengths.Int8;

            if (sequence.First.Length >= length)
                return ReadFixInt8(sequence.First.Span, out readSize);

            Span<byte> buffer = stackalloc byte[length];
            var index = 0;
            foreach (var memory in sequence)
            {
                for (var i = 0; i < memory.Length; i++)
                {
                    buffer[index++] = memory.Span[i];
                    if (index == length)
                        return ReadFixInt8(buffer, out readSize);
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
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="sequence"/> is too small or <paramref name="sequence"/>[0] is not <see cref="DataCodes.Int8"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadFixInt8(ReadOnlySequence<byte> sequence, out sbyte value, out int readSize)
        {
            const int length = DataLengths.Int8;

            if (sequence.First.Length >= length)
                return TryReadFixInt8(sequence.First.Span, out value, out readSize);

            Span<byte> buffer = stackalloc byte[length];
            var index = 0;
            foreach (var memory in sequence)
            {
                for (var i = 0; i < memory.Length; i++)
                {
                    buffer[index++] = memory.Span[i];
                    if (index == length)
                        return TryReadFixInt8(buffer, out value, out readSize);
                }
            }
            throw new IndexOutOfRangeException();
        }

        /// <summary>
        /// Read <see cref="sbyte"/> values from <paramref name="sequence"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte ReadInt8(ReadOnlySequence<byte> sequence, out int readSize)
        {
            if (sequence.IsEmpty) ThrowCantReadEmptyBufferException();
            var code = sequence.GetFirst();

            switch (code)
            {
                case DataCodes.Int8:
                    return ReadFixInt8(sequence, out readSize);

                case DataCodes.UInt8:
                    var value = ReadFixUInt8(sequence, out readSize);
                    if (value > sbyte.MaxValue) ThrowValueIsTooLargeException(value, short.MaxValue);
                    return unchecked((sbyte) value);
            }

            if (TryReadPositiveFixInt(sequence, out var positive, out readSize))
            {
                return unchecked((sbyte) positive);
            }

            if (TryReadNegativeFixInt(sequence, out var negative, out readSize))
            {
                return negative;
            }

            ThrowWrongIntCodeException(code, DataCodes.Int8, DataCodes.UInt8);
            return default;
        }

        /// <summary>
        /// Tries to read <see cref="sbyte"/> value from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">sequence to read from.</param>
        /// <param name="value">Value, read from <paramref name="sequence"/>. If return value is false, value is unspecified.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>. If return value is false, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="sequence"/> is too small or <paramref name="sequence"/>[0] is not ok.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadInt8(ReadOnlySequence<byte> sequence, out sbyte value, out int readSize)
        {
            if (TryReadFixUInt8(sequence, out var byteResult, out readSize))
            {
                value = unchecked((sbyte)byteResult);
                return true;
            }

            if (TryReadNegativeFixInt(sequence, out value, out readSize))
            {
                return true;
            }

            if (TryReadFixInt8(sequence, out value, out readSize))
                return true;

            if (TryReadPositiveFixInt(sequence, out byteResult, out readSize))
            {
                value = unchecked((sbyte)byteResult);
                return true;
            }

            return false;
        }
    }
}
