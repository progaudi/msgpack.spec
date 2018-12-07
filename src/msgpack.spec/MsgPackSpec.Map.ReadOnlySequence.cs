using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;

using static System.Buffers.Binary.BinaryPrimitives;
using static ProGaudi.MsgPack.DataCodes;

namespace ProGaudi.MsgPack
{
    /// <summary>
    /// Methods for working with maps (dictionaries)
    /// </summary>
    public static partial class MsgPackSpec
    {
        /// <summary>
        /// Reads FixMap header and return length of that map.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/></param>
        /// <returns>Length of map</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ReadFixMapHeader(ReadOnlySequence<byte> sequence, out int readSize)
        {
            readSize = DataLengths.FixMapHeader;
            var code = sequence.GetFirst();
            if (FixMapMin <= code && code <= FixMapMax)
                return (byte) (code - FixMapMin);

            return ThrowWrongRangeCodeException(code, FixMapMin, FixMapMax);
        }

        /// <summary>
        /// Tries to read FixMap header from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">Sequence to read.</param>
        /// <param name="length">Length of map. Should be less or equal to <see cref="DataLengths.FixMapMaxLength"/>.</param>
        /// <param name="readSize">Count of bytes read from <paramref name="sequence"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if:
        /// <list type="bullet">
        ///     <item><description><paramref name="sequence"/> is too small or</description></item>
        ///     <item><description><paramref name="sequence"/>[0] contains wrong data code or</description></item>
        ///     <item><description><paramref name="length"/> is greater <see cref="DataLengths.FixMapMaxLength"/>.</description></item>
        /// </list>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadFixMapHeader(ReadOnlySequence<byte> sequence, out byte length, out int readSize)
        {
            readSize = DataLengths.FixMapHeader;
            length = default;
            if (sequence.Length < readSize) return false;
            var code = sequence.GetFirst();
            var result = FixMapMin <= code && code <= FixMapMax;
            length = (byte) (code - FixMapMin);
            return result;
        }

        /// <summary>
        /// Reads Map16 header and return length of that map.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/></param>
        /// <returns>Length of map</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ReadMap16Header(ReadOnlySequence<byte> sequence, out int readSize)
        {
            const int length = DataLengths.Map16Header;
            readSize = length;
            Span<byte> buffer = stackalloc byte[length];
            if (!sequence.TryRead(buffer))
                throw GetReadOnlySequenceIsTooShortException(length, sequence.Length);

            var code = buffer[0];
            if (code == Map16)
                return ReadUInt16BigEndian(buffer.Slice(1));
            return ThrowWrongCodeException(code, Map16);
        }

        /// <summary>
        /// Tries to read Map16 header from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">Sequence to read.</param>
        /// <param name="length">Length of map.</param>
        /// <param name="readSize">Count of bytes read from <paramref name="sequence"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if:
        /// <list type="bullet">
        ///     <item><description><paramref name="sequence"/> is too small or</description></item>
        ///     <item><description><paramref name="sequence"/>[0] contains wrong data code.</description></item>
        /// </list>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadMap16Header(ReadOnlySequence<byte> sequence, out ushort length, out int readSize)
        {
            readSize = DataLengths.Map16Header;
            length = 0;
            Span<byte> buffer = stackalloc byte[DataLengths.Map16Header];
            return sequence.TryRead(buffer) && buffer[0] == Map16 && TryReadUInt16BigEndian(buffer.Slice(1), out length);
        }

        /// <summary>
        /// Reads Map32 header and return length of that map.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/></param>
        /// <returns>Length of map</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ReadMap32Header(ReadOnlySequence<byte> sequence, out int readSize)
        {
            const int length = DataLengths.Map32Header;
            readSize = length;
            Span<byte> buffer = stackalloc byte[length];
            if (!sequence.TryRead(buffer))
                throw GetReadOnlySequenceIsTooShortException(length, sequence.Length);

            var code = buffer[0];
            if (code == Map32)
                return ReadUInt32BigEndian(buffer.Slice(1));
            return ThrowWrongCodeException(code, Map32);
        }

        /// <summary>
        /// Tries to read Map32 header from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">Sequence to read.</param>
        /// <param name="length">Length of map.</param>
        /// <param name="readSize">Count of bytes read from <paramref name="sequence"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if:
        /// <list type="bullet">
        ///     <item><description><paramref name="sequence"/> is too small or</description></item>
        ///     <item><description><paramref name="sequence"/>[0] contains wrong data code.</description></item>
        /// </list>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadMap32Header(ReadOnlySequence<byte> sequence, out uint length, out int readSize)
        {
            readSize = DataLengths.Map32Header;
            length = 0;
            Span<byte> buffer = stackalloc byte[DataLengths.Map32Header];
            return sequence.TryRead(buffer) && buffer[0] == Map32 && TryReadUInt32BigEndian(buffer.Slice(1), out length);
        }

        /// <summary>
        /// Reads map header and return length of that map.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/></param>
        /// <returns>Length of map</returns>
        public static uint ReadMapHeader(ReadOnlySequence<byte> sequence, out int readSize)
        {
            var code = sequence.GetFirst();
            switch (code)
            {
                case Map16:
                    return ReadMap16Header(sequence, out readSize);
                case Map32:
                    return ReadMap32Header(sequence, out readSize);
            }

            if (FixMapMin <= code && code <= FixMapMax)
                return ReadFixMapHeader(sequence, out readSize);

            ThrowWrongMapHeader(code);
            readSize = 0;
            return default;
        }

        /// <summary>
        /// Tries to read map header from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">Sequence to read.</param>
        /// <param name="length">Length of map.</param>
        /// <param name="readSize">Count of bytes read from <paramref name="sequence"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if:
        /// <list type="bullet">
        ///     <item><description><paramref name="sequence"/> is too small or</description></item>
        ///     <item><description><paramref name="sequence"/>[0] contains wrong data code or</description></item>
        ///     <item><description>length of map is greater than <see cref="int.MaxValue"/></description></item>
        /// </list>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadMapHeader(ReadOnlySequence<byte> sequence, out int length, out int readSize)
        {
            if (TryReadFixMapHeader(sequence, out var byteValue, out readSize))
            {
                length = byteValue;
                return true;
            }

            if (TryReadMap16Header(sequence, out var uint16Value, out readSize))
            {
                length = uint16Value;
                return true;
            }

            if (TryReadMap32Header(sequence, out var uint32Value, out readSize))
            {
                // .net array size limitation
                // https://blogs.msdn.microsoft.com/joshwil/2005/08/10/bigarrayt-getting-around-the-2gb-map-size-limit/
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
