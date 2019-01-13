using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Text;
using ProGaudi.Buffers;
using static ProGaudi.MsgPack.DataCodes;

namespace ProGaudi.MsgPack
{
    /// <summary>
    /// Methods for working with strings
    /// </summary>
    public static partial class MsgPackSpec
    {
        /// <summary>
        /// Reads FixString header from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <returns>Returns length of string.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ReadFixStringHeader(ReadOnlySequence<byte> sequence, out int readSize)
        {
            const int length = DataLengths.FixStringHeader;

            if (sequence.First.Length >= length)
                return ReadFixStringHeader(sequence.First.Span, out readSize);

            Span<byte> buffer = stackalloc byte[length];
            return sequence.TryFillSpan(buffer)
                ? ReadFixStringHeader(buffer, out readSize)
                : throw GetReadOnlySequenceIsTooShortException(length, sequence.Length);
        }

        /// <summary>
        /// Tries to read FixString header from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="value">Length of string. If return value is <c>false</c>, value is unspecified.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="sequence"/> is too small.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadFixStringHeader(ReadOnlySequence<byte> sequence, out byte value, out int readSize)
        {
            const int length = DataLengths.FixStringHeader;

            if (sequence.First.Length >= length)
                return TryReadFixStringHeader(sequence.First.Span, out value, out readSize);

            value = default;
            readSize = default;

            Span<byte> buffer = stackalloc byte[length];
            return sequence.TryFillSpan(buffer) && TryReadFixStringHeader(buffer, out value, out readSize);
        }

        /// <summary>
        /// Reads <see cref="String8"/> header from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <returns>Returns length of <see cref="String8"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ReadString8Header(ReadOnlySequence<byte> sequence, out int readSize)
        {
            const int length = DataLengths.String8Header;

            if (sequence.First.Length >= length)
                return ReadString8Header(sequence.First.Span, out readSize);

            Span<byte> buffer = stackalloc byte[length];
            return sequence.TryFillSpan(buffer)
                ? ReadString8Header(buffer, out readSize)
                : throw GetReadOnlySequenceIsTooShortException(length, sequence.Length);
        }

        /// <summary>
        /// Tries to read <see cref="String8"/> header from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="value">Length of string. If return value is <c>false</c>, value is unspecified.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="sequence"/> is too small.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadString8Header(ReadOnlySequence<byte> sequence, out byte value, out int readSize)
        {
            const int length = DataLengths.String8Header;

            if (sequence.First.Length >= length)
                return TryReadString8Header(sequence.First.Span, out value, out readSize);

            value = default;
            readSize = default;

            Span<byte> buffer = stackalloc byte[length];
            return sequence.TryFillSpan(buffer) && TryReadString8Header(buffer, out value, out readSize);
        }

        /// <summary>
        /// Reads <see cref="String16"/> header from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <returns>Returns length of <see cref="String16"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ReadString16Header(ReadOnlySequence<byte> sequence, out int readSize)
        {
            const int length = DataLengths.String16Header;

            if (sequence.First.Length >= length)
                return ReadString16Header(sequence.First.Span, out readSize);

            Span<byte> buffer = stackalloc byte[length];
            return sequence.TryFillSpan(buffer)
                ? ReadString16Header(buffer, out readSize)
                : throw GetReadOnlySequenceIsTooShortException(length, sequence.Length);
        }

        /// <summary>
        /// Tries to read <see cref="String16"/> header from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="value">Length of string. If return value is <c>false</c>, value is unspecified.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="sequence"/> is too small.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadString16Header(ReadOnlySequence<byte> sequence, out ushort value, out int readSize)
        {
            const int length = DataLengths.String16Header;

            if (sequence.First.Length >= length)
                return TryReadString16Header(sequence.First.Span, out value, out readSize);

            value = default;
            readSize = default;

            Span<byte> buffer = stackalloc byte[length];
            return sequence.TryFillSpan(buffer) && TryReadString16Header(buffer, out value, out readSize);
        }

        /// <summary>
        /// Reads <see cref="String32"/> header from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <returns>Returns length of <see cref="String32"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ReadString32Header(ReadOnlySequence<byte> sequence, out int readSize)
        {
            const int length = DataLengths.String32Header;

            if (sequence.First.Length >= length)
                return ReadString32Header(sequence.First.Span, out readSize);

            Span<byte> buffer = stackalloc byte[length];
            return sequence.TryFillSpan(buffer)
                ? ReadString32Header(buffer, out readSize)
                : throw GetReadOnlySequenceIsTooShortException(length, sequence.Length);
        }

        /// <summary>
        /// Tries to read <see cref="String32"/> header from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="value">Length of string. If return value is <c>false</c>, value is unspecified.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="sequence"/> is too small.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadString32Header(ReadOnlySequence<byte> sequence, out uint value, out int readSize)
        {
            const int length = DataLengths.String32Header;

            if (sequence.First.Length >= length)
                return TryReadString32Header(sequence.First.Span, out value, out readSize);

            value = default;
            readSize = default;

            Span<byte> buffer = stackalloc byte[length];
            return sequence.TryFillSpan(buffer) && TryReadString32Header(buffer, out value, out readSize);
        }

        /// <summary>
        /// Reads string header from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <returns>Returns length of string.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadStringHeader(ReadOnlySequence<byte> sequence, out int readSize)
        {
            switch (sequence.GetFirst())
            {
                case String8:
                    return ReadString8Header(sequence, out readSize);
                case String16:
                    return ReadString16Header(sequence, out readSize);
                case String32:
                    var uint32Value = ReadString32Header(sequence, out readSize);
                    if (uint32Value > int.MaxValue) ThrowDataIsTooLarge(uint32Value);
                    return (int) uint32Value;
            }

            return ReadFixStringHeader(sequence, out readSize);
        }

        /// <summary>
        /// Tries to read string header from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="value">Length of string.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="sequence"/> is too small.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadStringHeader(ReadOnlySequence<byte> sequence, out int value, out int readSize)
        {
            if (TryReadFixStringHeader(sequence, out var byteValue, out readSize))
            {
                value = byteValue;
                return true;
            }

            if (TryReadString8Header(sequence, out byteValue, out readSize))
            {
                value = byteValue;
                return true;
            }

            if (TryReadString16Header(sequence, out var uint16Value, out readSize))
            {
                value = uint16Value;
                return true;
            }

            if (TryReadString32Header(sequence, out var uint32Value, out readSize))
            {
                // .net array size limitation
                // https://blogs.msdn.microsoft.com/joshwil/2005/08/10/bigarrayt-getting-around-the-2gb-array-size-limit/
                if (uint32Value <= int.MaxValue)
                {
                    value = (int)uint32Value;
                    return true;
                }
            }

            value = 0;
            return false;
        }

        /// <summary>
        /// Reads FixString from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <param name="decoder">Decoder. If not passed, utf8 without bom will be used.</param>
        /// <returns>Returns string.</returns>
        public static string ReadFixString(ReadOnlySequence<byte> sequence, out int readSize, Decoder decoder = null)
        {
            var length = ReadFixStringHeader(sequence, out readSize);
            readSize += length;
            return ReadStringImpl(sequence.Slice(readSize, length), decoder);
        }

        /// <summary>
        /// Tries to read FixString from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="value">Length of string.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <param name="decoder">Decoder. If not passed, utf8 without bom will be used.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="sequence"/> is too small.</returns>
        public static bool TryReadFixString(ReadOnlySequence<byte> sequence, out string value, out int readSize, Decoder decoder = null)
        {
            value = null;

            return TryReadFixStringHeader(sequence, out var length, out readSize)
                && TryReadStringImpl(sequence.Slice(readSize, length), out value, ref readSize, decoder);
        }

        /// <summary>
        /// Reads <see cref="String8"/> from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <param name="decoder">Decoder. If not passed, utf8 without bom will be used.</param>
        /// <returns>Returns string.</returns>
        public static string ReadString8(ReadOnlySequence<byte> sequence, out int readSize, Decoder decoder = null)
        {
            var length = ReadString8Header(sequence, out readSize);
            readSize += length;
            return ReadStringImpl(sequence.Slice(readSize, length), decoder);
        }

        /// <summary>
        /// Tries to read <see cref="String8"/> from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="value">Length of string.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <param name="decoder">Decoder. If not passed, utf8 without bom will be used.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="sequence"/> is too small.</returns>
        public static bool TryReadString8(ReadOnlySequence<byte> sequence, out string value, out int readSize, Decoder decoder = null)
        {
            value = null;

            return TryReadString8Header(sequence, out var length, out readSize)
                && TryReadStringImpl(sequence.Slice(readSize, length), out value, ref readSize, decoder);
        }

        /// <summary>
        /// Reads <see cref="String16"/> from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <param name="decoder">Decoder. If not passed, utf8 without bom will be used.</param>
        /// <returns>Returns string.</returns>
        public static string ReadString16(ReadOnlySequence<byte> sequence, out int readSize, Decoder decoder = null)
        {
            var length = ReadString16Header(sequence, out readSize);
            readSize += length;
            return ReadStringImpl(sequence.Slice(readSize, length), decoder);
        }

        /// <summary>
        /// Tries to read <see cref="String16"/> from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="value">Length of string.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <param name="decoder">Decoder. If not passed, utf8 without bom will be used.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="sequence"/> is too small.</returns>
        public static bool TryReadString16(ReadOnlySequence<byte> sequence, out string value, out int readSize, Decoder decoder = null)
        {
            value = null;

            return TryReadString16Header(sequence, out var length, out readSize)
                   && TryReadStringImpl(sequence.Slice(readSize, length), out value, ref readSize, decoder);
        }

        /// <summary>
        /// Reads <see cref="String32"/> from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <param name="decoder">Decoder. If not passed, utf8 without bom will be used.</param>
        /// <returns>Returns string.</returns>
        public static string ReadString32(ReadOnlySequence<byte> sequence, out int readSize, Decoder decoder = null)
        {
            var length = ReadString32Header(sequence, out readSize);
            if (length > int.MaxValue) ThrowDataIsTooLarge(length);
            var intLength = (int)length;
            readSize += intLength;
            return ReadStringImpl(sequence.Slice(readSize, intLength), decoder);
        }

        /// <summary>
        /// Tries to read <see cref="String32"/> from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="value">Length of string.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <param name="decoder">Decoder. If not passed, utf8 without bom will be used.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="sequence"/> is too small.</returns>
        public static bool TryReadString32(ReadOnlySequence<byte> sequence, out string value, out int readSize, Decoder decoder = null)
        {
            value = null;

            return TryReadString32Header(sequence, out var length, out readSize)
                   && length <= int.MaxValue
                   && TryReadStringImpl(sequence.Slice(readSize, (int) length), out value, ref readSize, decoder);
        }

        /// <summary>
        /// Reads string from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <param name="decoder">Decoder. If not passed, utf8 without bom will be used.</param>
        /// <returns>Returns string.</returns>
        public static string ReadString(ReadOnlySequence<byte> sequence, out int readSize, Decoder decoder = null)
        {
            var length = ReadStringHeader(sequence, out var offset);
            readSize = offset + length;
            return ReadStringImpl(sequence.Slice(offset, length), decoder);
        }

        /// <summary>
        /// Tries to read string from <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">Sequence to read from.</param>
        /// <param name="value">Length of string.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="sequence"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <param name="decoder">Decoder. If not passed, utf8 without bom will be used.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="sequence"/> is too small.</returns>
        public static bool TryReadString(ReadOnlySequence<byte> sequence, out string value, out int readSize, Decoder decoder = null)
        {
            return TryReadString8(sequence, out value, out readSize, decoder)
                || TryReadFixString(sequence, out value, out readSize, decoder)
                || TryReadString16(sequence, out value, out readSize, decoder)
                || TryReadString32(sequence, out value, out readSize, decoder);
        }

        private static string ReadStringImpl(ReadOnlySequence<byte> sequence, Decoder decoder)
        {
            if (sequence.IsSingleSegment) return ReadStringImpl(sequence.First.Span, decoder);

            var safeDecoder = decoder.GetThreadStatic();
            using (var rentedBuffer = MemoryPool<char>.Shared.Rent(sequence.GetIntLength()))
            {
                var chars = rentedBuffer.Memory.Span;
                var length = 0;
                var leftOver = ReadOnlySpan<byte>.Empty;

                foreach (var memory in sequence)
                {
                    if (memory.IsEmpty) continue;

                    var bytes = memory.Span;
                    using (var rentedBytes = GetMemoryOwner(leftOver, bytes))
                    {
                        safeDecoder.Convert(
                            rentedBytes != null ? rentedBytes.Memory.Span : bytes,
                            chars,
                            false,
                            out var bytesUsed,
                            out var charsUsed,
                            out var completed);

                        chars = chars.Slice(charsUsed);
                        length += charsUsed;

                        leftOver = completed ? ReadOnlySpan<byte>.Empty : bytes.Slice(bytesUsed - leftOver.Length);
                    }
                }

                if (!leftOver.IsEmpty)
                {
                    throw GetInvalidStringException();
                }

                return rentedBuffer.Memory.Span.Slice(0, length).ToString();
            }

            IMemoryOwner<byte> GetMemoryOwner(ReadOnlySpan<byte> leftOver, ReadOnlySpan<byte> bytes)
            {
                if (leftOver.IsEmpty)
                    return null;

                var owner = FixedLengthMemoryPool<byte>.Shared.Rent(leftOver.Length + bytes.Length);
                var span = owner.Memory.Span;
                leftOver.CopyTo(span);
                bytes.CopyTo(span.Slice(leftOver.Length));
                return owner;
            }
        }

        private static bool TryReadStringImpl(ReadOnlySequence<byte> sequence, out string value, ref int readSize, Decoder decoder)
        {
            value = ReadStringImpl(sequence, decoder);
            readSize += sequence.GetIntLength();
            return true;
        }
    }
}
