using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;

using ProGaudi.Buffers;

namespace ProGaudi.MsgPack
{
    /// <summary>
    /// Methods for working with binary blobs
    /// </summary>
    public static partial class MsgPackSpec
    {
        /// <summary>
        /// Reads binary8 header.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes read from sequence</param>
        /// <returns>Binary length</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ReadBinary8Header(ReadOnlySequence<byte> sequence, out int readSize)
        {
            const int length = DataLengths.Binary8Header;
            if (sequence.First.Length >= length)
                return ReadBinary8Header(sequence.First.Span, out readSize);

            readSize = length;
            Span<byte> buffer = stackalloc byte[length];
            if (!sequence.TryRead(buffer))
                throw GetReadOnlySequenceIsTooShortException(length, sequence.Length);

            var code = buffer[0];
            if (code == DataCodes.Binary8)
                return buffer[1];
            return ThrowWrongCodeException(code, DataCodes.Binary8);
        }

        /// <summary>
        /// Reads binary8 header.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="length">Binary length.</param>
        /// <param name="readSize">Count of bytes read from sequence.</param>
        /// <returns><c>true</c> if everything is ok, <c>false</c> if
        ///     <list type="bullet">
        ///         <item><description><paramref name="sequence"/>[0] contains wrong data code or</description></item>
        ///         <item><description><paramref name="sequence"/> is too small</description></item>
        ///     </list>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadBinary8Header(ReadOnlySequence<byte> sequence, out byte length, out int readSize)
        {
            const int size = DataLengths.Binary8Header;
            if (sequence.First.Length >= size)
                return TryReadBinary8Header(sequence.First.Span, out length, out readSize);

            readSize = size;
            length = 0;
            Span<byte> buffer = stackalloc byte[size];
            if (!sequence.TryRead(buffer) || buffer[0] != DataCodes.Binary8) return false;
            length = buffer[1];
            return true;
        }

        /// <summary>
        /// Reads binary16 header.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes read from sequence</param>
        /// <returns>Binary length</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ReadBinary16Header(ReadOnlySequence<byte> sequence, out int readSize)
        {
            const int length = DataLengths.Binary16Header;
            if (sequence.First.Length >= length)
                return ReadBinary16Header(sequence.First.Span, out readSize);

            readSize = length;
            Span<byte> buffer = stackalloc byte[length];
            if (!sequence.TryRead(buffer))
                throw GetReadOnlySequenceIsTooShortException(length, sequence.Length);

            var code = buffer[0];
            if (code == DataCodes.Binary16)
                return BinaryPrimitives.ReadUInt16BigEndian(buffer.Slice(1));
            return ThrowWrongCodeException(code, DataCodes.Binary16);
        }

        /// <summary>
        /// Reads binary16 header.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="length">Binary length.</param>
        /// <param name="readSize">Count of bytes read from sequence.</param>
        /// <returns><c>true</c> if everything is ok, <c>false</c> if
        ///     <list type="bullet">
        ///         <item><description><paramref name="sequence"/>[0] contains wrong data code or</description></item>
        ///         <item><description><paramref name="sequence"/> is too small</description></item>
        ///     </list>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadBinary16Header(ReadOnlySequence<byte> sequence, out ushort length, out int readSize)
        {
            const int size = DataLengths.Binary16Header;
            if (sequence.First.Length >= size)
                return TryReadBinary16Header(sequence.First.Span, out length, out readSize);

            readSize = size;
            length = 0;
            Span<byte> buffer = stackalloc byte[size];
            return sequence.TryRead(buffer) && buffer[0] == DataCodes.Binary16 && BinaryPrimitives.TryReadUInt16BigEndian(buffer.Slice(1), out length);
        }

        /// <summary>
        /// Reads binary32 header.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes read from sequence</param>
        /// <returns>Binary length</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ReadBinary32Header(ReadOnlySequence<byte> sequence, out int readSize)
        {
            const int length = DataLengths.Binary32Header;
            if (sequence.First.Length >= length)
                return ReadBinary32Header(sequence.First.Span, out readSize);

            readSize = length;
            Span<byte> buffer = stackalloc byte[length];
            if (!sequence.TryRead(buffer))
                throw GetReadOnlySequenceIsTooShortException(length, sequence.Length);

            var code = buffer[0];
            if (code == DataCodes.Binary32)
                return BinaryPrimitives.ReadUInt16BigEndian(buffer.Slice(1));
            return ThrowWrongCodeException(code, DataCodes.Binary32);
        }

        /// <summary>
        /// Reads binary32 header.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="length">Binary length.</param>
        /// <param name="readSize">Count of bytes read from sequence.</param>
        /// <returns><c>true</c> if everything is ok, <c>false</c> if
        ///     <list type="bullet">
        ///         <item><description><paramref name="sequence"/>[0] contains wrong data code or</description></item>
        ///         <item><description><paramref name="sequence"/> is too small</description></item>
        ///     </list>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadBinary32Header(ReadOnlySequence<byte> sequence, out uint length, out int readSize)
        {
            const int size = DataLengths.Binary32Header;
            if (sequence.First.Length >= size)
                return TryReadBinary32Header(sequence.First.Span, out length, out readSize);

            readSize = size;
            length = 0;
            Span<byte> buffer = stackalloc byte[size];
            return sequence.TryRead(buffer) && buffer[0] == DataCodes.Binary32 && BinaryPrimitives.TryReadUInt32BigEndian(buffer.Slice(1), out length);
        }

        /// <summary>
        /// Reads binary header.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes read from sequence</param>
        /// <returns>Binary length</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadBinaryHeader(ReadOnlySequence<byte> sequence, out int readSize)
        {
            var code = sequence.GetFirst();
            switch (code)
            {
                case DataCodes.Binary8:
                    return ReadBinary8Header(sequence, out readSize);
                case DataCodes.Binary16:
                    return ReadBinary16Header(sequence, out readSize);
                case DataCodes.Binary32:
                    var uint32Value = ReadBinary32Header(sequence, out readSize);
                    if (uint32Value <= int.MaxValue)
                    {
                        return (int) uint32Value;
                    }

                    return ThrowDataIsTooLarge(uint32Value);
                default:
                    readSize = 0;
                    return ThrowWrongCodeException(code, DataCodes.Binary8, DataCodes.Binary16, DataCodes.Binary32);
            }
        }

        /// <summary>
        /// Reads binary8 header.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="length">Binary length.</param>
        /// <param name="readSize">Count of bytes read from sequence.</param>
        /// <returns><c>true</c> if everything is ok, <c>false</c> if
        ///     <list type="bullet">
        ///         <item><description><paramref name="sequence"/>[0] contains wrong data code or</description></item>
        ///         <item><description><paramref name="sequence"/> is too small</description></item>
        ///     </list>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadBinaryHeader(ReadOnlySequence<byte> sequence, out uint length, out int readSize)
        {
            length = 0;

            if (TryReadBinary8Header(sequence, out var byteValue, out readSize))
            {
                length = byteValue;
                return true;
            }

            if (TryReadBinary16Header(sequence, out var uint16Value, out readSize))
            {
                length = uint16Value;
                return true;
            }

            return TryReadBinary32Header(sequence, out length, out readSize);
        }

        /// <summary>
        /// Read Binary8 into sequence, rented from <see cref="MemoryPool{T}.Shared"/>.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>.</param>
        /// <returns>Length of result is guaranteed to be equal of read length.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IMemoryOwner<byte> ReadBinary8(ReadOnlySequence<byte> sequence, out int readSize)
        {
            var resultLength = ReadBinary8Header(sequence, out readSize);
            return ReadBinaryBlob(sequence, ref readSize, resultLength);
        }

        /// <summary>
        /// Tries to read Binary8 into sequence, rented from <see cref="MemoryPool{T}.Shared"/>.
        /// </summary>
        /// <param name="sequence">Sequence to read from</param>
        /// <param name="result">Rented sequence from pool. If returned value is <c>false</c>, it will be null.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if not.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadBinary8(ReadOnlySequence<byte> sequence, out IMemoryOwner<byte> result, out int readSize)
        {
            result = null;

            if (TryReadBinary8Header(sequence, out var byteLength, out readSize))
            {
                return TryReadBinary(sequence, out result, byteLength, ref readSize);
            }

            return false;
        }

        /// <summary>
        /// Read Binary16 into sequence, rented from <see cref="MemoryPool{T}.Shared"/>.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>.</param>
        /// <returns>Length of result is guaranteed to be equal of read length.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IMemoryOwner<byte> ReadBinary16(ReadOnlySequence<byte> sequence, out int readSize)
        {
            var resultLength = ReadBinary8Header(sequence, out readSize);
            return ReadBinaryBlob(sequence, ref readSize, resultLength);
        }

        /// <summary>
        /// Tries to read Binary16 into sequence, rented from <see cref="MemoryPool{T}.Shared"/>.
        /// </summary>
        /// <param name="sequence">Sequence to read from</param>
        /// <param name="result">Rented sequence from pool. If returned value is <c>false</c>, it will be null.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if not.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadBinary16(ReadOnlySequence<byte> sequence, out IMemoryOwner<byte> result, out int readSize)
        {
            result = null;

            if (TryReadBinary16Header(sequence, out var ushortLength, out readSize))
            {
                return TryReadBinary(sequence, out result, ushortLength, ref readSize);
            }

            return false;
        }

        /// <summary>
        /// Read Binary32 into sequence, rented from <see cref="MemoryPool{T}.Shared"/>.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>.</param>
        /// <returns>Length of result is guaranteed to be equal of read length.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IMemoryOwner<byte> ReadBinary32(ReadOnlySequence<byte> sequence, out int readSize)
        {
            var length = ReadBinary32Header(sequence, out readSize);
            if (length > int.MaxValue) ThrowDataIsTooLarge(length);
            var resultLength = (int)length;
            return ReadBinaryBlob(sequence, ref readSize, resultLength);
        }

        /// <summary>
        /// Tries to read Binary32 into sequence, rented from <see cref="MemoryPool{T}.Shared"/>.
        /// </summary>
        /// <param name="sequence">Sequence to read from</param>
        /// <param name="result">Rented sequence from pool. If returned value is <c>false</c>, it will be null.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if not.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadBinary32(ReadOnlySequence<byte> sequence, out IMemoryOwner<byte> result, out int readSize)
        {
            result = null;

            if (TryReadBinary32Header(sequence, out var uintLength, out readSize) && uintLength <= int.MaxValue)
            {
                return TryReadBinary(sequence, out result, (int) uintLength, ref readSize);
            }

            return false;
        }

        /// <summary>
        /// Read binary into sequence, rented from <see cref="MemoryPool{T}.Shared"/>.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>.</param>
        /// <returns>Length of result is guaranteed to be equal of read length.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IMemoryOwner<byte> ReadBinary(ReadOnlySequence<byte> sequence, out int readSize)
        {
            var resultLength = ReadBinaryHeader(sequence, out readSize);
            return ReadBinaryBlob(sequence, ref readSize, resultLength);
        }

        /// <summary>
        /// Tries to read binary into sequence, rented from <see cref="MemoryPool{T}.Shared"/>.
        /// </summary>
        /// <param name="sequence">Sequence to read from</param>
        /// <param name="result">Rented sequence from pool. If returned value is <c>false</c>, it will be null.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if not.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadBinary(ReadOnlySequence<byte> sequence, out IMemoryOwner<byte> result, out int readSize)
        {
            return TryReadBinary8(sequence, out result, out readSize)
                || TryReadBinary16(sequence, out result, out readSize)
                || TryReadBinary32(sequence, out result, out readSize);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IMemoryOwner<byte> ReadBinaryBlob(ReadOnlySequence<byte> sequence, ref int readSize, int length)
        {
            var result = FixedLengthMemoryPool<byte>.Shared.Rent(length);
            sequence.Slice(readSize, length).CopyTo(result.Memory.Span);
            readSize += length;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryReadBinary(ReadOnlySequence<byte> sequence, out IMemoryOwner<byte> result, int resultLength, ref int readSize)
        {
            result = FixedLengthMemoryPool<byte>.Shared.Rent(resultLength);
            sequence.Slice(readSize, resultLength).CopyTo(result.Memory.Span);
            readSize += resultLength;
            return true;
        }
    }
}
