using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;

using static ProGaudi.MsgPack.Light.DataCodes;

namespace ProGaudi.MsgPack.Light
{
    /// <summary>
    /// Methods for working with binary blobs
    /// </summary>
    public static partial class MsgPackBinary
    {
        /// <summary>
        /// Writes Binary8 header into <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to write. Ensure that is at least 2 bytes long.</param>
        /// <param name="length">Length of binary blob.</param>
        /// <returns>Count of bytes, written to <paramref name="buffer"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteBinary8Header(Span<byte> buffer, byte length)
        {
            buffer[0] = Binary8;
            buffer[1] = length;
            return 2;
        }

        /// <summary>
        /// Tries to write Binary8 header into <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to write.</param>
        /// <param name="length">Length of binary blob.</param>
        /// <param name="wroteSize">Count of bytes, written to <paramref name="buffer"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="buffer"/> is too small.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteBinary8Header(Span<byte> buffer, byte length, out int wroteSize)
        {
            wroteSize = 2;
            if (buffer.Length < wroteSize) return false;
            buffer[0] = Binary8;
            buffer[1] = length;
            return true;
        }

        /// <summary>
        /// Writes Binary16 header into <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to write. Ensure that is at least 3 bytes long.</param>
        /// <param name="length">Length of binary blob.</param>
        /// <returns>Count of bytes, written to <paramref name="buffer"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteBinary16Header(Span<byte> buffer, ushort length)
        {
            buffer[0] = Binary16;
            BinaryPrimitives.WriteUInt16BigEndian(buffer.Slice(1), length);
            return 3;
        }

        /// <summary>
        /// Tries to write Binary16 header into <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to write.</param>
        /// <param name="length">Length of binary blob.</param>
        /// <param name="wroteSize">Count of bytes, written to <paramref name="buffer"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="buffer"/> is too small.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteBinary16Header(Span<byte> buffer, ushort length, out int wroteSize)
        {
            wroteSize = 3;
            if (buffer.Length < wroteSize) return false;
            buffer[0] = Binary16;
            return BinaryPrimitives.TryWriteUInt16BigEndian(buffer.Slice(1), length);
        }

        /// <summary>
        /// Writes Binary32 header into <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to write. Ensure that is at least 5 bytes long.</param>
        /// <param name="length">Length of binary blob.</param>
        /// <returns>Count of bytes, written to <paramref name="buffer"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteBinary32Header(Span<byte> buffer, uint length)
        {
            buffer[0] = Binary32;
            BinaryPrimitives.WriteUInt32BigEndian(buffer.Slice(1), length);
            return 5;
        }

        /// <summary>
        /// Tries to write Binary32 header into <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to write.</param>
        /// <param name="length">Length of binary blob.</param>
        /// <param name="wroteSize">Count of bytes, written to <paramref name="buffer"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="buffer"/> is too small.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteBinary32Header(Span<byte> buffer, uint length, out int wroteSize)
        {
            wroteSize = 5;
            if (buffer.Length < wroteSize) return false;
            buffer[0] = Binary32;
            return BinaryPrimitives.TryWriteUInt32BigEndian(buffer.Slice(1), length);
        }

        /// <summary>
        /// Reads binary8 header.
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="readSize">Count of bytes read from buffer</param>
        /// <returns>Binary length</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ReadBinary8Header(ReadOnlySpan<byte> buffer, out int readSize)
        {
            readSize = 2;
            if (buffer[0] == Binary8)
                return buffer[1];

            throw WrongCodeException(buffer[0], Binary8);
        }

        /// <summary>
        /// Reads binary8 header.
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="length">Binary length.</param>
        /// <param name="readSize">Count of bytes read from buffer.</param>
        /// <returns><c>true</c> if everything is ok, <c>false</c> if
        ///     <list type="bullet">
        ///         <item><description><paramref name="buffer"/>[0] contains wrong data code or</description></item>
        ///         <item><description><paramref name="buffer"/> is too small</description></item>
        ///     </list>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadBinary8Header(ReadOnlySpan<byte> buffer, out byte length, out int readSize)
        {
            readSize = 2;
            length = 0;
            if (buffer.Length < readSize) return false;
            length = buffer[1];
            return buffer[0] == Binary8;
        }

        /// <summary>
        /// Reads binary16 header.
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="readSize">Count of bytes read from buffer</param>
        /// <returns>Binary length</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ReadBinary16Header(ReadOnlySpan<byte> buffer, out int readSize)
        {
            readSize = 3;
            if (buffer[0] == Binary16)
                return BinaryPrimitives.ReadUInt16BigEndian(buffer.Slice(1));

            throw WrongCodeException(buffer[0], Binary16);
        }

        /// <summary>
        /// Reads binary16 header.
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="length">Binary length.</param>
        /// <param name="readSize">Count of bytes read from buffer.</param>
        /// <returns><c>true</c> if everything is ok, <c>false</c> if
        ///     <list type="bullet">
        ///         <item><description><paramref name="buffer"/>[0] contains wrong data code or</description></item>
        ///         <item><description><paramref name="buffer"/> is too small</description></item>
        ///     </list>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadBinary16Header(ReadOnlySpan<byte> buffer, out ushort length, out int readSize)
        {
            readSize = 3;
            length = 0;
            return buffer.Length >= readSize
                && buffer[0] == Binary16
                && BinaryPrimitives.TryReadUInt16BigEndian(buffer.Slice(1), out length);
        }

        /// <summary>
        /// Reads binary32 header.
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="readSize">Count of bytes read from buffer</param>
        /// <returns>Binary length</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ReadBinary32Header(ReadOnlySpan<byte> buffer, out int readSize)
        {
            readSize = 5;
            if (buffer[0] == Binary32)
                return BinaryPrimitives.ReadUInt32BigEndian(buffer.Slice(1));

            throw WrongCodeException(buffer[0], Binary32);
        }

        /// <summary>
        /// Reads binary32 header.
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="length">Binary length.</param>
        /// <param name="readSize">Count of bytes read from buffer.</param>
        /// <returns><c>true</c> if everything is ok, <c>false</c> if
        ///     <list type="bullet">
        ///         <item><description><paramref name="buffer"/>[0] contains wrong data code or</description></item>
        ///         <item><description><paramref name="buffer"/> is too small</description></item>
        ///     </list>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadBinary32Header(ReadOnlySpan<byte> buffer, out uint length, out int readSize)
        {
            readSize = 5;
            length = 0;
            return buffer.Length >= readSize
                && buffer[0] == Binary32
                && BinaryPrimitives.TryReadUInt32BigEndian(buffer.Slice(1), out length);
        }

        /// <summary>
        /// Writes smallest possible binary header into <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to write. Depending on <paramref name="length"/> you'll need 2, 3 or 5 bytes in buffer.</param>
        /// <param name="length">Length of binary blob.</param>
        /// <returns>Count of bytes, written to <paramref name="buffer"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteBinaryHeader(Span<byte> buffer, int length)
        {
            if (length < 0)
            {
                throw LengthShouldBeNonNegative(length);
            }

            if (length <= byte.MaxValue)
            {
                return WriteBinary8Header(buffer, (byte) length);
            }

            if (length <= ushort.MaxValue)
            {
                return WriteBinary16Header(buffer, (ushort) length);
            }

            return WriteBinary32Header(buffer, (uint) length);
        }

        /// <summary>
        /// Tries to write smallest possible binary header into <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to write.</param>
        /// <param name="length">Length of binary blob.</param>
        /// <param name="wroteSize">Count of bytes, written to <paramref name="buffer"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="buffer"/> is too small or <see cref="length"/> is below zero.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteBinaryHeader(Span<byte> buffer, int length, out int wroteSize)
        {
            if (length < 0)
            {
                wroteSize = 0;
                return false;
            }

            if (length <= byte.MaxValue)
            {
                return TryWriteBinary8Header(buffer, (byte) length, out wroteSize);
            }

            if (length <= ushort.MaxValue)
            {
                return TryWriteBinary16Header(buffer, (ushort) length, out wroteSize);
            }

            return TryWriteBinary32Header(buffer, (uint) length, out wroteSize);
        }

        /// <summary>
        /// Reads binary header.
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="readSize">Count of bytes read from buffer</param>
        /// <returns>Binary length</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadBinaryHeader(ReadOnlySpan<byte> buffer, out int readSize)
        {
            switch (buffer[0])
            {
                case Binary8:
                    return ReadBinary8Header(buffer, out readSize);
                case Binary16:
                    return ReadBinary16Header(buffer, out readSize);
                case Binary32:
                    var uint32Value = ReadBinary32Header(buffer, out readSize);
                    if (uint32Value <= int.MaxValue)
                    {
                        return (int) uint32Value;
                    }

                    throw TooLargeArray(uint32Value);
                default:
                    throw WrongCodeException(buffer[0], Binary8, Binary16, Binary32);
            }
        }

        /// <summary>
        /// Reads binary8 header.
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="length">Binary length.</param>
        /// <param name="readSize">Count of bytes read from buffer.</param>
        /// <returns><c>true</c> if everything is ok, <c>false</c> if
        ///     <list type="bullet">
        ///         <item><description><paramref name="buffer"/>[0] contains wrong data code or</description></item>
        ///         <item><description><paramref name="buffer"/> is too small</description></item>
        ///     </list>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadBinaryHeader(ReadOnlySpan<byte> buffer, out uint length, out int readSize)
        {
            length = 0;

            if (TryReadBinary8Header(buffer, out var byteValue, out readSize))
            {
                length = byteValue;
                return true;
            }

            if (TryReadBinary16Header(buffer, out var uint16Value, out readSize))
            {
                length = uint16Value;
                return true;
            }

            return TryReadBinary32Header(buffer, out length, out readSize);
        }

        /// <summary>
        /// Writes Binary8 into <paramref name="buffer"/>. Overhead is 2 bytes.
        /// </summary>
        /// <param name="buffer">Buffer to write.</param>
        /// <param name="binary">Binary blob. Should be shorter or equal to <see cref="byte.MaxValue"/>.</param>
        /// <returns>Count of bytes, written to <paramref name="buffer"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteBinary8(Span<byte> buffer, ReadOnlySpan<byte> binary)
        {
            var length = binary.Length;
            if (length > byte.MaxValue)
            {
                throw DataIsTooLarge(binary.Length, byte.MaxValue, nameof(Binary8), Binary8);
            }

            var result = WriteBinary8Header(buffer, (byte)length);
            binary.CopyTo(buffer.Slice(result));
            return result + length;
        }

        /// <summary>
        /// Tries to write Binary8 into <paramref name="buffer"/>. Overhead is 2 bytes.
        /// </summary>
        /// <param name="buffer">Buffer to write.</param>
        /// <param name="binary">Binary blob. Should be shorter or equal to <see cref="byte.MaxValue"/>.</param>
        /// <param name="wroteSize">Count of bytes, written to <paramref name="buffer"/>.</param>
        /// <returns><c>true</c> if everything is ok, <c>false</c> if <paramref name="buffer"/> is too short or <paramref name="binary"/> is too long.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteBinary8(Span<byte> buffer, ReadOnlySpan<byte> binary, out int wroteSize)
        {
            var length = binary.Length;
            if (length > byte.MaxValue)
            {
                wroteSize = 0;
                return false;
            }

            if (TryWriteBinary8Header(buffer, (byte)length, out wroteSize))
            {
                if (binary.TryCopyTo(buffer.Slice(wroteSize)))
                {
                    wroteSize += length;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Writes Binary16 into <paramref name="buffer"/>. Overhead is 3 bytes.
        /// </summary>
        /// <param name="buffer">Buffer to write.</param>
        /// <param name="binary">Binary blob. Should be shorter or equal to <see cref="ushort.MaxValue"/>.</param>
        /// <returns>Count of bytes, written to <paramref name="buffer"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteBinary16(Span<byte> buffer, ReadOnlySpan<byte> binary)
        {
            var length = binary.Length;
            if (length > ushort.MaxValue)
            {
                throw DataIsTooLarge(binary.Length, ushort.MaxValue, nameof(Binary16), Binary16);
            }

            var result = WriteBinary16Header(buffer, (ushort) length);
            binary.CopyTo(buffer.Slice(result));
            return result + length;
        }

        /// <summary>
        /// Tries to write Binary16 into <paramref name="buffer"/>. Overhead is 3 bytes.
        /// </summary>
        /// <param name="buffer">Buffer to write.</param>
        /// <param name="binary">Binary blob. Should be shorter or equal to <see cref="ushort.MaxValue"/>.</param>
        /// <param name="wroteSize">Count of bytes, written to <paramref name="buffer"/>.</param>
        /// <returns><c>true</c> if everything is ok, <c>false</c> if <paramref name="buffer"/> is too short or <paramref name="binary"/> is too long.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteBinary16(Span<byte> buffer, ReadOnlySpan<byte> binary, out int wroteSize)
        {
            var length = binary.Length;
            if (length > ushort.MaxValue)
            {
                wroteSize = 0;
                return false;
            }

            if (TryWriteBinary16Header(buffer, (ushort) length, out wroteSize))
            {
                if (binary.TryCopyTo(buffer.Slice(wroteSize)))
                {
                    wroteSize += length;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Writes Binary32 into <paramref name="buffer"/>. Overhead is 5 bytes.
        /// </summary>
        /// <param name="buffer">Buffer to write.</param>
        /// <param name="binary">Binary blob.</param>
        /// <returns>Count of bytes, written to <paramref name="buffer"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteBinary32(Span<byte> buffer, ReadOnlySpan<byte> binary)
        {
            var result = WriteBinary32Header(buffer, (uint) binary.Length);
            binary.CopyTo(buffer.Slice(result));
            return result + binary.Length;
        }

        /// <summary>
        /// Tries to write Binary32 into <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to write. Overhead is 5 bytes.</param>
        /// <param name="binary">Binary blob.</param>
        /// <param name="wroteSize">Count of bytes, written to <paramref name="buffer"/>.</param>
        /// <returns><c>true</c> if everything is ok, <c>false</c> if <paramref name="buffer"/> is too short or <paramref name="binary"/> is too long.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteBinary32(Span<byte> buffer, ReadOnlySpan<byte> binary, out int wroteSize)
        {
            var length = binary.Length;
            if (TryWriteBinary32Header(buffer, (uint) length, out wroteSize))
            {
                if (binary.TryCopyTo(buffer.Slice(wroteSize)))
                {
                    wroteSize += length;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Writes binary with smallest possible overhead into <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to write. Overhead is 5 bytes.</param>
        /// <param name="binary">Binary blob.</param>
        /// <returns><c>true</c> if everything is ok, <c>false</c> if <paramref name="buffer"/> is too short or <paramref name="binary"/> is too long.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteBinary(Span<byte> buffer, ReadOnlySpan<byte> binary)
        {
            if (binary.Length <= byte.MaxValue) return WriteBinary8(buffer, binary);
            if (binary.Length <= ushort.MaxValue) return WriteBinary16(buffer, binary);
            return WriteBinary32(buffer, binary);
        }

        /// <summary>
        /// Tries to write binary with smallest possible overhead into <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to write.</param>
        /// <param name="binary">Binary blob.</param>
        /// <param name="wroteSize">Count of bytes, written to <paramref name="buffer"/>.</param>
        /// <returns><c>true</c> if everything is ok, <c>false</c> if <paramref name="buffer"/> is too short or <paramref name="binary"/> is too long.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteBinary(Span<byte> buffer, ReadOnlySpan<byte> binary, out int wroteSize)
        {
            return TryWriteBinary8(buffer, binary, out wroteSize)
                || TryWriteBinary16(buffer, binary, out wroteSize)
                || TryWriteBinary32(buffer, binary, out wroteSize);
        }

        /// <summary>
        /// Read Binary8 into buffer, rented from <see cref="MemoryPool{T}.Shared"/>.
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="resultLength">Length of returned buffer.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="buffer"/>.</param>
        /// <returns>Buffer that can be larger, than <paramref name="resultLength"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IMemoryOwner<byte> ReadBinary8(ReadOnlySpan<byte> buffer, out int resultLength, out int readSize)
        {
            resultLength = ReadBinary8Header(buffer, out readSize);
            return ReadBinaryBlob(buffer, ref readSize, resultLength);
        }

        /// <summary>
        /// Tries to read Binary8 into buffer, rented from <see cref="MemoryPool{T}.Shared"/>.
        /// </summary>
        /// <param name="buffer">Buffer to read from</param>
        /// <param name="result">Rented buffer from pool. If returned value is <c>false</c>, it will be null.</param>
        /// <param name="resultLength">Length of binary, read into <paramref name="result"/>.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="buffer"/>.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if not.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadBinary8(ReadOnlySpan<byte> buffer, out IMemoryOwner<byte> result, out int resultLength, out int readSize)
        {
            result = null;
            resultLength = 0;

            if (TryReadBinary8Header(buffer, out var byteLength, out readSize))
            {
                resultLength = byteLength;
                return TryReadBinary(buffer, out result, resultLength, ref readSize);
            }

            return false;
        }

        /// <summary>
        /// Read Binary16 into buffer, rented from <see cref="MemoryPool{T}.Shared"/>.
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="resultLength">Length of returned buffer.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="buffer"/>.</param>
        /// <returns>Buffer that can be larger, than <paramref name="resultLength"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IMemoryOwner<byte> ReadBinary16(ReadOnlySpan<byte> buffer, out int resultLength, out int readSize)
        {
            resultLength = ReadBinary8Header(buffer, out readSize);
            return ReadBinaryBlob(buffer, ref readSize, resultLength);
        }

        /// <summary>
        /// Tries to read Binary16 into buffer, rented from <see cref="MemoryPool{T}.Shared"/>.
        /// </summary>
        /// <param name="buffer">Buffer to read from</param>
        /// <param name="result">Rented buffer from pool. If returned value is <c>false</c>, it will be null.</param>
        /// <param name="resultLength">Length of binary, read into <paramref name="result"/>.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="buffer"/>.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if not.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadBinary16(ReadOnlySpan<byte> buffer, out IMemoryOwner<byte> result, out int resultLength, out int readSize)
        {
            result = null;
            resultLength = 0;

            if (TryReadBinary16Header(buffer, out var ushortLength, out readSize))
            {
                resultLength = ushortLength;
                return TryReadBinary(buffer, out result, resultLength, ref readSize);
            }

            return false;
        }

        /// <summary>
        /// Read Binary32 into buffer, rented from <see cref="MemoryPool{T}.Shared"/>.
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="resultLength">Length of returned buffer.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="buffer"/>.</param>
        /// <returns>Buffer that can be larger, than <paramref name="resultLength"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IMemoryOwner<byte> ReadBinary32(ReadOnlySpan<byte> buffer, out int resultLength, out int readSize)
        {
            var length = ReadBinary32Header(buffer, out readSize);
            if (length > int.MaxValue) throw TooLargeArray(length);
            resultLength = (int)length;
            return ReadBinaryBlob(buffer, ref readSize, resultLength);
        }

        /// <summary>
        /// Tries to read Binary32 into buffer, rented from <see cref="MemoryPool{T}.Shared"/>.
        /// </summary>
        /// <param name="buffer">Buffer to read from</param>
        /// <param name="result">Rented buffer from pool. If returned value is <c>false</c>, it will be null.</param>
        /// <param name="resultLength">Length of binary, read into <paramref name="result"/>.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="buffer"/>.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if not.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadBinary32(ReadOnlySpan<byte> buffer, out IMemoryOwner<byte> result, out int resultLength, out int readSize)
        {
            result = null;
            resultLength = 0;

            if (TryReadBinary32Header(buffer, out var uintLength, out readSize) && uintLength <= int.MaxValue)
            {
                resultLength = (int) uintLength;
                return TryReadBinary(buffer, out result, resultLength, ref readSize);
            }

            return false;
        }

        /// <summary>
        /// Read binary into buffer, rented from <see cref="MemoryPool{T}.Shared"/>.
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="resultLength">Length of returned buffer.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="buffer"/>.</param>
        /// <returns>Buffer that can be larger, than <paramref name="resultLength"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IMemoryOwner<byte> ReadBinary(ReadOnlySpan<byte> buffer, out int resultLength, out int readSize)
        {
            resultLength = ReadBinary8Header(buffer, out readSize);
            return ReadBinaryBlob(buffer, ref readSize, resultLength);
        }

        /// <summary>
        /// Tries to read binary into buffer, rented from <see cref="MemoryPool{T}.Shared"/>.
        /// </summary>
        /// <param name="buffer">Buffer to read from</param>
        /// <param name="result">Rented buffer from pool. If returned value is <c>false</c>, it will be null.</param>
        /// <param name="resultLength">Length of binary, read into <paramref name="result"/>.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="buffer"/>.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if not.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadBinary(ReadOnlySpan<byte> buffer, out IMemoryOwner<byte> result, out int resultLength, out int readSize)
        {
            return TryReadBinary8(buffer, out result, out resultLength, out readSize)
                || TryReadBinary16(buffer, out result, out resultLength, out readSize)
                || TryReadBinary32(buffer, out result, out resultLength, out readSize);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IMemoryOwner<byte> ReadBinaryBlob(ReadOnlySpan<byte> buffer, ref int readSize, int length)
        {
            var result = MemoryPool<byte>.Shared.Rent(length);
            buffer.Slice(readSize, length).CopyTo(result.Memory.Span);
            readSize += length;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryReadBinary(ReadOnlySpan<byte> buffer, out IMemoryOwner<byte> result, int resultLength, ref int readSize)
        {
            result = MemoryPool<byte>.Shared.Rent(resultLength);
            if (buffer.Slice(readSize, resultLength).TryCopyTo(result.Memory.Span))
            {
                readSize += resultLength;
                return true;
            }

            result.Dispose();
            result = null;
            return false;
        }
    }
}
