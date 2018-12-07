using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;

using ProGaudi.Buffers;

namespace ProGaudi.MsgPack
{
    /// <summary>
    /// Methods for working with extensions
    /// </summary>
    public static partial class MsgPackSpec
    {
        /// <summary>
        /// Reads extension header. Length of extension should be 1 byte.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes read from <paramref name="sequence"/>.</param>
        /// <returns>Extension code type.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte ReadFixExtension1Header(ReadOnlySequence<byte> sequence, out int readSize) => ReadFixExtensionHeader(sequence, DataCodes.FixExtension1, out readSize);

        /// <summary>
        /// Reads extension. Length of extension should be 1 byte.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes read from <paramref name="sequence"/>.</param>
        /// <returns>Extension code type and extension.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (sbyte type, byte extension) ReadFixExtension1(ReadOnlySequence<byte> sequence, out int readSize)
        {
            const int length = DataLengths.FixExtensionHeader + sizeof(byte);
            if (sequence.First.Length >= length)
                return ReadFixExtension1(sequence.First.Span, out readSize);

            readSize = length;
            Span<byte> buffer = stackalloc byte[length];
            if (!sequence.TryRead(buffer))
                throw GetReadOnlySequenceIsTooShortException(length, sequence.Length);

            return (ReadFixExtension1Header(buffer, out _), buffer[2]);
        }

        /// <summary>
        /// Reads extension header. Length of extension should be 2 bytes.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes read from <paramref name="sequence"/>.</param>
        /// <returns>Extension code type,</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte ReadFixExtension2Header(ReadOnlySequence<byte> sequence, out int readSize) => ReadFixExtensionHeader(sequence, DataCodes.FixExtension2, out readSize);

        /// <summary>
        /// Reads extension. Length of extension should be 2 bytes.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes read from <paramref name="sequence"/>.</param>
        /// <returns>Extension code type and extension.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (sbyte type, IMemoryOwner<byte> extension) ReadFixExtension2(ReadOnlySequence<byte> sequence, out int readSize)
        {
            var type = ReadFixExtension2Header(sequence, out readSize);
            return (type, ReadExtension(sequence, 2, ref readSize));
        }

        /// <summary>
        /// Reads extension header. Length of extension should be 4 bytes.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes read from <paramref name="sequence"/>.</param>
        /// <returns>Extension code type.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte ReadFixExtension4Header(ReadOnlySequence<byte> sequence, out int readSize) => ReadFixExtensionHeader(sequence, DataCodes.FixExtension4, out readSize);

        /// <summary>
        /// Reads extension. Length of extension should be 4 bytes.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes read from <paramref name="sequence"/>.</param>
        /// <returns>Extension code type and extension.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (sbyte type, IMemoryOwner<byte> extension) ReadFixExtension4(ReadOnlySequence<byte> sequence, out int readSize)
        {
            var type = ReadFixExtension4Header(sequence, out readSize);
            return (type, ReadExtension(sequence, 4, ref readSize));
        }

        /// <summary>
        /// Reads extension header. Length of extension should be 8 bytes.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes read from <paramref name="sequence"/>.</param>
        /// <returns>Extension code type.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte ReadFixExtension8Header(ReadOnlySequence<byte> sequence, out int readSize) => ReadFixExtensionHeader(sequence, DataCodes.FixExtension8, out readSize);

        /// <summary>
        /// Reads extension. Length of extension should be 8 bytes.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes read from <paramref name="sequence"/>.</param>
        /// <returns>Extension code type and extension.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (sbyte type, IMemoryOwner<byte> extension) ReadFixExtension8(ReadOnlySequence<byte> sequence, out int readSize)
        {
            var type = ReadFixExtension8Header(sequence, out readSize);
            return (type, ReadExtension(sequence, 8, ref readSize));
        }

        /// <summary>
        /// Reads extension header. Length of extension should be 16 bytes.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes read from <paramref name="sequence"/>.</param>
        /// <returns>Extension code type.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte ReadFixExtension16Header(ReadOnlySequence<byte> sequence, out int readSize) => ReadFixExtensionHeader(sequence, DataCodes.FixExtension16, out readSize);

        /// <summary>
        /// Reads extension. Length of extension should be 16 bytes.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes read from <paramref name="sequence"/>.</param>
        /// <returns>Extension code type and extension.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (sbyte type, IMemoryOwner<byte> extension) ReadFixExtension16(ReadOnlySequence<byte> sequence, out int readSize)
        {
            var type = ReadFixExtension16Header(sequence, out readSize);
            return (type, ReadExtension(sequence, 16, ref readSize));
        }

        /// <summary>
        /// Reads extension header. Length of extension should be 8 bits uint.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes read from <paramref name="sequence"/>.</param>
        /// <returns>Extension code type and length</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (sbyte type, byte length) ReadExtension8Header(ReadOnlySequence<byte> sequence, out int readSize)
        {
            const int length = DataLengths.Extension8Header;
            if (sequence.First.Length >= length)
                return ReadExtension8Header(sequence.First.Span, out readSize);

            readSize = length;
            Span<byte> buffer = stackalloc byte[length];
            if (!sequence.TryRead(buffer))
                throw GetReadOnlySequenceIsTooShortException(length, sequence.Length);

            var type = unchecked((sbyte)buffer[2]);
            if (buffer[0] != DataCodes.Extension8) ThrowWrongCodeException(buffer[0], DataCodes.Extension8);
            return (type, buffer[1]);
        }

        /// <summary>
        /// Reads extension. Length of extension should be 8 bits uint.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes read from <paramref name="sequence"/>.</param>
        /// <returns>Extension code type and extension.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (sbyte type, IMemoryOwner<byte> extension) ReadExtension8(ReadOnlySequence<byte> sequence, out int readSize)
        {
            var (type, size) = ReadExtension8Header(sequence, out readSize);
            return (type, ReadExtension(sequence, size, ref readSize));
        }

        /// <summary>
        /// Reads extension header. Length of extension should be 16 bits uint.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes read from <paramref name="sequence"/>.</param>
        /// <returns>Extension code type and length</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (sbyte type, ushort length) ReadExtension16Header(ReadOnlySequence<byte> sequence, out int readSize)
        {
            const int length = DataLengths.Extension16Header;
            if (sequence.First.Length >= length)
                return ReadExtension16Header(sequence.First.Span, out readSize);

            readSize = length;
            Span<byte> buffer = stackalloc byte[length];
            if (!sequence.TryRead(buffer))
                throw GetReadOnlySequenceIsTooShortException(length, sequence.Length);

            var type = unchecked((sbyte)buffer[3]);
            if (buffer[0] != DataCodes.Extension16) ThrowWrongCodeException(buffer[0], DataCodes.Extension16);
            return (type, BinaryPrimitives.ReadUInt16BigEndian(buffer.Slice(1)));
        }

        /// <summary>
        /// Reads extension. Length of extension should be 16 bits uint.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes read from <paramref name="sequence"/>.</param>
        /// <returns>Extension code type and extension.</returns>
        public static (sbyte type, IMemoryOwner<byte> extension) ReadExtension16(ReadOnlySequence<byte> sequence, out int readSize)
        {
            var (type, size) = ReadExtension16Header(sequence, out readSize);
            return (type, ReadExtension(sequence, size, ref readSize));
        }

        /// <summary>
        /// Reads extension header. Length of extension should be 32 bits uint.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes read from <paramref name="sequence"/>.</param>
        /// <returns>Extension code type and length</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (sbyte type, uint length) ReadExtension32Header(ReadOnlySequence<byte> sequence, out int readSize)
        {
            const int length = DataLengths.Extension32Header;
            if (sequence.First.Length >= length)
                return ReadExtension32Header(sequence.First.Span, out readSize);

            readSize = length;
            Span<byte> buffer = stackalloc byte[length];
            if (!sequence.TryRead(buffer))
                throw GetReadOnlySequenceIsTooShortException(length, sequence.Length);

            var type = unchecked((sbyte)buffer[5]);
            if (buffer[0] != DataCodes.Extension32) ThrowWrongCodeException(buffer[0], DataCodes.Extension32);
            return (type, BinaryPrimitives.ReadUInt32BigEndian(buffer.Slice(1)));
        }

        /// <summary>
        /// Reads extension. Length of extension should be 32 bits uint.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes read from <paramref name="sequence"/>.</param>
        /// <returns>Extension code type and extension.</returns>
        public static (sbyte type, IMemoryOwner<byte> extension) ReadExtension32(ReadOnlySequence<byte> sequence, out int readSize)
        {
            var (type, uintSize) = ReadExtension32Header(sequence, out readSize);
            if (uintSize > int.MaxValue) ThrowDataIsTooLarge(uintSize);
            return (type, ReadExtension(sequence, (int) uintSize, ref readSize));
        }

        /// <summary>
        /// Reads extension header.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes read from <paramref name="sequence"/>.</param>
        /// <returns>Extension code type and length</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (sbyte type, uint length) ReadExtensionHeader(ReadOnlySequence<byte> sequence, out int readSize)
        {
            var code = sequence.GetFirst();
            switch (code)
            {
                case DataCodes.FixExtension1:
                    return (ReadFixExtension1Header(sequence, out readSize), 1);

                case DataCodes.FixExtension2:
                    return (ReadFixExtension2Header(sequence, out readSize), 2);

                case DataCodes.FixExtension4:
                    return (ReadFixExtension2Header(sequence, out readSize), 4);

                case DataCodes.FixExtension8:
                    return (ReadFixExtension2Header(sequence, out readSize), 8);

                case DataCodes.FixExtension16:
                    return (ReadFixExtension2Header(sequence, out readSize), 16);

                case DataCodes.Extension8:
                    return ReadExtension8Header(sequence, out readSize);

                case DataCodes.Extension16:
                    return ReadExtension16Header(sequence, out readSize);

                case DataCodes.Extension32:
                    return ReadExtension32Header(sequence, out readSize);

                default:
                    readSize = 0;
                    ThrowWrongCodeException(
                        code,
                        DataCodes.FixExtension1,
                        DataCodes.FixExtension2,
                        DataCodes.FixExtension4,
                        DataCodes.FixExtension8,
                        DataCodes.FixExtension16,
                        DataCodes.Extension8,
                        DataCodes.Extension16,
                        DataCodes.Extension32);
                    return default;
            }
        }

        /// <summary>
        /// Reads extension.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes read from <paramref name="sequence"/>.</param>
        /// <returns>Extension code type and extension.</returns>
        public static (sbyte type, IMemoryOwner<byte> extension) ReadExtension(ReadOnlySequence<byte> sequence, out int readSize)
        {
            var (type, uintSize) = ReadExtensionHeader(sequence, out readSize);
            if (uintSize > int.MaxValue) ThrowDataIsTooLarge(uintSize);
            return (type, ReadExtension(sequence, (int) uintSize, ref readSize));
        }

        /// <summary>
        /// Tries to read header of 1-byte extension.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="type">Type of extension.</param>
        /// <param name="readSize">Count of bytes read from <paramref name="sequence"/>.</param>
        /// <returns><c>true</c> if everything is ok or <c>false</c> if <paramref name="sequence"/> is too short or first byte isn't <see cref="DataCodes.FixExtension1"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadFixExtension1Header(ReadOnlySequence<byte> sequence, out sbyte type, out int readSize) => TryReadExtensionHeader(sequence, DataCodes.FixExtension1, out type, out readSize);

        /// <summary>
        /// Tries to read 1-byte extension.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="type">Type of extension.</param>
        /// <param name="extension">Extension data.</param>
        /// <param name="readSize">Count of bytes read from <paramref name="sequence"/>.</param>
        /// <returns><c>true</c> if everything is ok or <c>false</c> if <paramref name="sequence"/> is too short or <see cref="TryReadFixExtension1Header"/> returned false.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadFixExtension1(ReadOnlySequence<byte> sequence, out sbyte type, out byte extension, out int readSize)
        {
            const int length = DataLengths.FixExtensionHeader + sizeof(byte);
            if (sequence.First.Length >= length)
                return TryReadFixExtension1(sequence.First.Span, out type, out extension, out readSize);

            readSize = length;
            Span<byte> buffer = stackalloc byte[length];
            if (sequence.TryRead(buffer) && TryReadFixExtension1Header(sequence, out type, out readSize))
            {
                extension = buffer[2];
                return true;
            }

            type = default;
            extension = default;
            return false;
        }

        /// <summary>
        /// Tries to read header of 2-bytes extension.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="type">Type of extension.</param>
        /// <param name="readSize">Count of bytes read from <paramref name="sequence"/>.</param>
        /// <returns><c>true</c> if everything is ok or <c>false</c> if <paramref name="sequence"/> is too short or first byte isn't <see cref="DataCodes.FixExtension2"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadFixExtension2Header(ReadOnlySequence<byte> sequence, out sbyte type, out int readSize) => TryReadExtensionHeader(sequence, DataCodes.FixExtension2, out type, out readSize);

        /// <summary>
        /// Tries to read 2-bytes extension.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="type">Type of extension.</param>
        /// <param name="extension">Extension data.</param>
        /// <param name="readSize">Count of bytes read from <paramref name="sequence"/>.</param>
        /// <returns><c>true</c> if everything is ok or <c>false</c> if <paramref name="sequence"/> is too short or <see cref="TryReadFixExtension2Header"/> returned false.</returns>
        public static bool TryReadFixExtension2(ReadOnlySequence<byte> sequence, out sbyte type, out IMemoryOwner<byte> extension, out int readSize)
        {
            extension = null;
            return TryReadFixExtension2Header(sequence, out type, out readSize) && TryReadExtension(sequence, 2, ref extension, ref readSize);
        }

        /// <summary>
        /// Tries to read header of 4-bytes extension.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="type">Type of extension.</param>
        /// <param name="readSize">Count of bytes read from <paramref name="sequence"/>.</param>
        /// <returns><c>true</c> if everything is ok or <c>false</c> if <paramref name="sequence"/> is too short or first byte isn't <see cref="DataCodes.FixExtension4"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadFixExtension4Header(ReadOnlySequence<byte> sequence, out sbyte type, out int readSize) => TryReadExtensionHeader(sequence, DataCodes.FixExtension4, out type, out readSize);

        /// <summary>
        /// Tries to read 4-bytes extension.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="type">Type of extension.</param>
        /// <param name="extension">Extension data.</param>
        /// <param name="readSize">Count of bytes read from <paramref name="sequence"/>.</param>
        /// <returns><c>true</c> if everything is ok or <c>false</c> if <paramref name="sequence"/> is too short or <see cref="TryReadFixExtension4Header"/> returned false.</returns>
        public static bool TryReadFixExtension4(ReadOnlySequence<byte> sequence, out sbyte type, out IMemoryOwner<byte> extension, out int readSize)
        {
            extension = null;
            return TryReadFixExtension4Header(sequence, out type, out readSize) && TryReadExtension(sequence, 4, ref extension, ref readSize);
        }

        /// <summary>
        /// Tries to read header of 8-bytes extension.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="type">Type of extension.</param>
        /// <param name="readSize">Count of bytes read from <paramref name="sequence"/>.</param>
        /// <returns><c>true</c> if everything is ok or <c>false</c> if <paramref name="sequence"/> is too short or first byte isn't <see cref="DataCodes.FixExtension8"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadFixExtension8Header(ReadOnlySequence<byte> sequence, out sbyte type, out int readSize) => TryReadExtensionHeader(sequence, DataCodes.FixExtension8, out type, out readSize);

        /// <summary>
        /// Tries to read 8-bytes extension.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="type">Type of extension.</param>
        /// <param name="extension">Extension data.</param>
        /// <param name="readSize">Count of bytes read from <paramref name="sequence"/>.</param>
        /// <returns><c>true</c> if everything is ok or <c>false</c> if <paramref name="sequence"/> is too short or <see cref="TryReadFixExtension8Header"/> returned false.</returns>
        public static bool TryReadFixExtension8(ReadOnlySequence<byte> sequence, out sbyte type, out IMemoryOwner<byte> extension, out int readSize)
        {
            extension = null;
            return TryReadFixExtension8Header(sequence, out type, out readSize) && TryReadExtension(sequence, 8, ref extension, ref readSize);
        }

        /// <summary>
        /// Tries to read header of 16-bytes extension.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="type">Type of extension.</param>
        /// <param name="readSize">Count of bytes read from <paramref name="sequence"/>.</param>
        /// <returns><c>true</c> if everything is ok or <c>false</c> if <paramref name="sequence"/> is too short or first byte isn't <see cref="DataCodes.FixExtension16"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadFixExtension16Header(ReadOnlySequence<byte> sequence, out sbyte type, out int readSize) => TryReadExtensionHeader(sequence, DataCodes.FixExtension16, out type, out readSize);

        /// <summary>
        /// Tries to read 16-bytes extension.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="type">Type of extension.</param>
        /// <param name="extension">Extension data.</param>
        /// <param name="readSize">Count of bytes read from <paramref name="sequence"/>.</param>
        /// <returns><c>true</c> if everything is ok or <c>false</c> if <paramref name="sequence"/> is too short or <see cref="TryReadFixExtension16Header"/> returned false.</returns>
        public static bool TryReadFixExtension16(ReadOnlySequence<byte> sequence, out sbyte type, out IMemoryOwner<byte> extension, out int readSize)
        {
            extension = null;
            return TryReadFixExtension16Header(sequence, out type, out readSize) && TryReadExtension(sequence, 16, ref extension, ref readSize);
        }

        /// <summary>
        /// Tries to read header of 8 bit length extension.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="type">Type of extension.</param>
        /// <param name="length">Length of extension.</param>
        /// <param name="readSize">Count of bytes read from <paramref name="sequence"/>.</param>
        /// <returns><c>true</c> if everything is ok or <c>false</c> if <paramref name="sequence"/> is too short or first byte isn't <see cref="DataCodes.Extension8"/></returns>
        public static bool TryReadExtension8Header(ReadOnlySequence<byte> sequence, out sbyte type, out byte length, out int readSize)
        {
            const int size = DataLengths.Extension8Header + sizeof(sbyte);
            if (sequence.First.Length >= size)
                return TryReadExtension8Header(sequence.First.Span, out type, out length, out readSize);

            length = default;
            type = default;
            readSize = size;
            Span<byte> buffer = stackalloc byte[size];
            return sequence.TryRead(buffer) && TryReadExtension8Header(buffer, out type, out length, out readSize);
        }

        /// <summary>
        /// Tries to read 8 bit extension.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="type">Type of extension.</param>
        /// <param name="extension">Extension data.</param>
        /// <param name="readSize">Count of bytes read from <paramref name="sequence"/>.</param>
        /// <returns><c>true</c> if everything is ok or <c>false</c> if <paramref name="sequence"/> is too short or <see cref="TryReadExtension8Header"/> returned false.</returns>
        public static bool TryReadExtension8(ReadOnlySequence<byte> sequence, out sbyte type, out IMemoryOwner<byte> extension, out int readSize)
        {
            extension = null;
            return TryReadExtension8Header(sequence, out type, out var length, out readSize)
                && TryReadExtension(sequence, length, ref extension, ref readSize);
        }

        /// <summary>
        /// Tries to read header of 16 bit length extension.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="type">Type of extension.</param>
        /// <param name="length">Length of extension.</param>
        /// <param name="readSize">Count of bytes read from <paramref name="sequence"/>.</param>
        /// <returns><c>true</c> if everything is ok or <c>false</c> if <paramref name="sequence"/> is too short or first byte isn't <see cref="DataCodes.Extension16"/></returns>
        public static bool TryReadExtension16Header(ReadOnlySequence<byte> sequence, out sbyte type, out ushort length, out int readSize)
        {
            const int size = DataLengths.Extension16Header + sizeof(ushort);
            if (sequence.First.Length >= size)
                return TryReadExtension16Header(sequence.First.Span, out type, out length, out readSize);

            length = default;
            type = default;
            readSize = size;
            Span<byte> buffer = stackalloc byte[size];
            return sequence.TryRead(buffer) && TryReadExtension16Header(buffer, out type, out length, out readSize);
        }

        /// <summary>
        /// Tries to read 16 bit extension.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="type">Type of extension.</param>
        /// <param name="extension">Extension data.</param>
        /// <param name="readSize">Count of bytes read from <paramref name="sequence"/>.</param>
        /// <returns><c>true</c> if everything is ok or <c>false</c> if <paramref name="sequence"/> is too short or <see cref="TryReadExtension16Header"/> returned false.</returns>
        public static bool TryReadExtension16(ReadOnlySequence<byte> sequence, out sbyte type, out IMemoryOwner<byte> extension, out int readSize)
        {
            extension = null;
            return TryReadExtension16Header(sequence, out type, out var length, out readSize)
                && TryReadExtension(sequence, length, ref extension, ref readSize);
        }

        /// <summary>
        /// Tries to read header of 16 bit length extension.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="type">Type of extension.</param>
        /// <param name="length">Length of extension.</param>
        /// <param name="readSize">Count of bytes read from <paramref name="sequence"/>.</param>
        /// <returns><c>true</c> if everything is ok or <c>false</c> if <paramref name="sequence"/> is too short or first byte isn't <see cref="DataCodes.Extension16"/></returns>
        public static bool TryReadExtension32Header(ReadOnlySequence<byte> sequence, out sbyte type, out uint length, out int readSize)
        {
            const int size = DataLengths.Extension32Header + sizeof(uint);
            if (sequence.First.Length >= size)
                return TryReadExtension32Header(sequence.First.Span, out type, out length, out readSize);

            length = default;
            type = default;
            readSize = size;
            Span<byte> buffer = stackalloc byte[size];
            return sequence.TryRead(buffer) && TryReadExtension32Header(buffer, out type, out length, out readSize);
        }

        /// <summary>
        /// Tries to read 32 bit extension.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="type">Type of extension.</param>
        /// <param name="extension">Extension data.</param>
        /// <param name="readSize">Count of bytes read from <paramref name="sequence"/>.</param>
        /// <returns><c>true</c> if everything is ok or <c>false</c> if <paramref name="sequence"/> is too short or <see cref="TryReadExtension32Header"/> returned false.</returns>
        public static bool TryReadExtension32(ReadOnlySequence<byte> sequence, out sbyte type, out IMemoryOwner<byte> extension, out int readSize)
        {
            extension = null;
            return TryReadExtension32Header(sequence, out type, out var length, out readSize)
                && length <= int.MaxValue
                && TryReadExtension(sequence, (int) length, ref extension, ref readSize);
        }

        /// <summary>
        /// Tries to read header of extension.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="type">Type of extension.</param>
        /// <param name="length">Length of extension.</param>
        /// <param name="readSize">Count of bytes read from <paramref name="sequence"/>.</param>
        /// <returns><c>true</c> if everything is ok or <c>false</c> if <paramref name="sequence"/> is too short or first byte isn't one of extension codes.</returns>
        public static bool TryReadExtensionHeader(ReadOnlySequence<byte> sequence, out sbyte type, out uint length, out int readSize)
        {
            if (sequence.IsEmpty)
            {
                type = 0;
                length = 0;
                readSize = 0;
                return false;
            }

            if (TryReadFixExtension1Header(sequence, out type, out readSize))
            {
                length = 1;
                return true;
            }

            if (TryReadFixExtension2Header(sequence, out type, out readSize))
            {
                length = 2;
                return true;
            }

            if (TryReadFixExtension4Header(sequence, out type, out readSize))
            {
                length = 4;
                return true;
            }

            if (TryReadFixExtension8Header(sequence, out type, out readSize))
            {
                length = 8;
                return true;
            }

            if (TryReadFixExtension16Header(sequence, out type, out readSize))
            {
                length = 16;
                return true;
            }

            if (TryReadExtension8Header(sequence, out type, out var byteLength, out readSize))
            {
                length = byteLength;
                return true;
            }

            if (TryReadExtension16Header(sequence, out type, out var shortLength, out readSize))
            {
                length = shortLength;
                return true;
            }

            return TryReadExtension32Header(sequence, out type, out length, out readSize);
        }

        /// <summary>
        /// Tries to read extension.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="type">Type of extension.</param>
        /// <param name="extension">Extension data.</param>
        /// <param name="readSize">Count of bytes read from <paramref name="sequence"/>.</param>
        /// <returns><c>true</c> if everything is ok or <c>false</c> if <paramref name="sequence"/> is too short or <see cref="TryReadExtensionHeader"/> returned false, or length is greater than <see cref="int.MaxValue"/>.</returns>
        public static bool TryReadExtension(ReadOnlySequence<byte> sequence, out sbyte type, out IMemoryOwner<byte> extension, out int readSize)
        {
            extension = null;
            return TryReadExtensionHeader(sequence, out type, out var length, out readSize)
                && length <= int.MaxValue
                && TryReadExtension(sequence, (int) length, ref extension, ref readSize);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryReadExtension(ReadOnlySequence<byte> sequence, int size, ref IMemoryOwner<byte> extension, ref int readSize)
        {
            if (sequence.Length - readSize < size) return false;

            extension = FixedLengthMemoryPool<byte>.Shared.Rent(size);
            sequence.Slice(readSize).CopyTo(extension.Memory.Span);
            readSize += size;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryReadExtensionHeader(ReadOnlySequence<byte> sequence, byte code, out sbyte type, out int readSize)
        {
            type = 0;
            const int length = DataLengths.FixExtensionHeader;
            readSize = length;
            Span<byte> buffer = stackalloc byte[length];
            if (sequence.TryRead(buffer)) return false;
            if (buffer[0] != code) return false;
            type = unchecked((sbyte)buffer[1]);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IMemoryOwner<byte> ReadExtension(
            ReadOnlySequence<byte> sequence,
            int size,
            ref int readSize)
        {
            var owner = FixedLengthMemoryPool<byte>.Shared.Rent(size);
            sequence.Slice(readSize, size).CopyTo(owner.Memory.Span);
            readSize += size;
            return owner;
        }

        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static sbyte ReadFixExtensionHeader(ReadOnlySequence<byte> sequence, byte code, out int readSize)
        {
            const int length = DataLengths.FixExtensionHeader;
            if (sequence.First.Length >= length)
                return ReadFixExtension1Header(sequence.First.Span, out readSize);

            readSize = length;
            Span<byte> buffer = stackalloc byte[length];
            if (!sequence.TryRead(buffer))
                throw GetReadOnlySequenceIsTooShortException(length, sequence.Length);

            var type = unchecked((sbyte)buffer[1]);
            if (buffer[0] != code) ThrowWrongCodeException(buffer[0], code);
            return type;
        }
    }
}
