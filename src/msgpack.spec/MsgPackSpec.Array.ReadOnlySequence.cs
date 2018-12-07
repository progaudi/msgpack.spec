using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;

using static ProGaudi.MsgPack.DataCodes;

namespace ProGaudi.MsgPack
{
    /// <summary>
    /// Methods for working with binary blobs
    /// </summary>
    public static partial class MsgPackSpec
    {
        /// <summary>
        /// Reads FixArray header and return length of that array.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/></param>
        /// <returns>Length of array</returns>
        public static byte ReadFixArrayHeader(ReadOnlySequence<byte> sequence, out int readSize)
        {
            const int length = DataLengths.FixArrayHeader;
            if (sequence.First.Length >= length)
                return ReadFixArrayHeader(sequence.First.Span, out readSize);

            readSize = length;
            var code = sequence.GetFirst();
            if (FixArrayMin <= code && code <= FixArrayMax)
                return (byte) (code - FixArrayMin);

            return ThrowWrongRangeCodeException(code, FixArrayMin, FixArrayMax);
        }

        /// <summary>
        /// Tries to read FixArray header from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">Sequence to read.</param>
        /// <param name="length">Length of array. Should be less or equal to <see cref="DataLengths.FixArrayMaxLength"/>.</param>
        /// <param name="readSize">Count of bytes read from <paramref name="sequence"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if:
        /// <list type="bullet">
        ///     <item><description><paramref name="sequence"/> is too small or</description></item>
        ///     <item><description><paramref name="sequence"/>[0] contains wrong data code or</description></item>
        ///     <item><description><paramref name="length"/> is greater <see cref="DataLengths.FixArrayMaxLength"/>.</description></item>
        /// </list>
        /// </returns>
        public static bool TryReadFixArrayHeader(ReadOnlySequence<byte> sequence, out byte length, out int readSize)
        {
            const int size = DataLengths.FixArrayHeader;
            if (sequence.First.Length >= size)
                return TryReadFixArrayHeader(sequence.First.Span, out length, out readSize);

            readSize = size;
            length = 0;
            if (sequence.Length < readSize) return false;
            var code = sequence.GetFirst();
            var result = FixArrayMin <= code && code <= FixArrayMax;
            length = (byte) (code - FixArrayMin);
            return result;
        }

        /// <summary>
        /// Reads Array16 header and return length of that array.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/></param>
        /// <returns>Length of array</returns>
        public static ushort ReadArray16Header(ReadOnlySequence<byte> sequence, out int readSize)
        {
            const int length = DataLengths.Array16Header;
            if (sequence.First.Length >= length)
                return ReadArray16Header(sequence.First.Span, out readSize);

            readSize = length;
            Span<byte> buffer = stackalloc byte[length];
            if (!sequence.TryRead(buffer))
                throw GetReadOnlySequenceIsTooShortException(length, sequence.Length);

            var code = buffer[0];
            if (code == Array16)
                return BinaryPrimitives.ReadUInt16BigEndian(buffer.Slice(1));
            return ThrowWrongCodeException(code, Array16);
        }

        /// <summary>
        /// Tries to read Array16 header from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">Sequence to read.</param>
        /// <param name="length">Length of array.</param>
        /// <param name="readSize">Count of bytes read from <paramref name="sequence"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if:
        /// <list type="bullet">
        ///     <item><description><paramref name="sequence"/> is too small or</description></item>
        ///     <item><description><paramref name="sequence"/>[0] contains wrong data code.</description></item>
        /// </list>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadArray16Header(ReadOnlySequence<byte> sequence, out ushort length, out int readSize)
        {
            const int size = DataLengths.Array16Header;
            if (sequence.First.Length >= size)
                return TryReadArray16Header(sequence.First.Span, out length, out readSize);

            readSize = size;
            length = 0;
            Span<byte> buffer = stackalloc byte[size];
            return sequence.TryRead(buffer) && buffer[0] == Array16 && BinaryPrimitives.TryReadUInt16BigEndian(buffer.Slice(1), out length);
        }

        /// <summary>
        /// Reads Array32 header and return length of that array.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/></param>
        /// <returns>Length of array</returns>
        public static uint ReadArray32Header(ReadOnlySequence<byte> sequence, out int readSize)
        {
            const int length = DataLengths.Array32Header;
            if (sequence.First.Length >= length)
                return ReadArray32Header(sequence.First.Span, out readSize);

            readSize = length;
            Span<byte> buffer = stackalloc byte[length];
            if (!sequence.TryRead(buffer))
                throw GetReadOnlySequenceIsTooShortException(length, sequence.Length);

            var code = buffer[0];
            if (code == Array16)
                return BinaryPrimitives.ReadUInt32BigEndian(buffer.Slice(1));
            return ThrowWrongCodeException(code, Array32);
        }

        /// <summary>
        /// Tries to read Array32 header from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">Sequence to read.</param>
        /// <param name="length">Length of array.</param>
        /// <param name="readSize">Count of bytes read from <paramref name="sequence"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if:
        /// <list type="bullet">
        ///     <item><description><paramref name="sequence"/> is too small or</description></item>
        ///     <item><description><paramref name="sequence"/>[0] contains wrong data code.</description></item>
        /// </list>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadArray32Header(ReadOnlySequence<byte> sequence, out uint length, out int readSize)
        {
            const int size = DataLengths.Array32Header;
            if (sequence.First.Length >= size)
                return TryReadArray32Header(sequence.First.Span, out length, out readSize);

            readSize = size;
            length = 0;
            Span<byte> buffer = stackalloc byte[size];
            return sequence.TryRead(buffer) && buffer[0] == Array32 && BinaryPrimitives.TryReadUInt32BigEndian(buffer.Slice(1), out length);
        }

        /// <summary>
        /// Reads array header and return length of that array.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/></param>
        /// <returns>Length of array</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadArrayHeader(ReadOnlySequence<byte> sequence, out int readSize)
        {
            var code = sequence.GetFirst();
            if (FixArrayMin <= code && code <= FixArrayMax)
            {
                return ReadFixArrayHeader(sequence, out readSize);
            }

            if (code == Array16)
            {
                return ReadArray16Header(sequence, out readSize);
            }

            if (code == Array32)
            {
                var uint32Value = ReadArray32Header(sequence, out readSize);
                if (uint32Value <= int.MaxValue)
                {
                    return (int) uint32Value;
                }

                return ThrowDataIsTooLarge(uint32Value);
            }

            readSize = 0;
            return ThrowWrongArrayHeader(code);
        }

        /// <summary>
        /// Tries to read array header from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">Sequence to read.</param>
        /// <param name="length">Length of array.</param>
        /// <param name="readSize">Count of bytes read from <paramref name="sequence"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if:
        /// <list type="bullet">
        ///     <item><description><paramref name="sequence"/> is too small or</description></item>
        ///     <item><description><paramref name="sequence"/>[0] contains wrong data code or</description></item>
        ///     <item><description>length of array is greater than <see cref="int.MaxValue"/></description></item>
        /// </list>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadArrayHeader(ReadOnlySequence<byte> sequence, out int length, out int readSize)
        {
            if (TryReadFixArrayHeader(sequence, out var byteValue, out readSize))
            {
                length = byteValue;
                return true;
            }

            if (TryReadArray16Header(sequence, out var uint16Value, out readSize))
            {
                length = uint16Value;
                return true;
            }

            if (TryReadArray32Header(sequence, out var uint32Value, out readSize))
            {
                // .net array size limitation
                // https://blogs.msdn.microsoft.com/joshwil/2005/08/10/bigarrayt-getting-around-the-2gb-array-size-limit/
                if (uint32Value <= int.MaxValue)
                {
                    length = (int)uint32Value;
                    return true;
                }
            }

            length = 0;
            return false;
        }
    }
}
