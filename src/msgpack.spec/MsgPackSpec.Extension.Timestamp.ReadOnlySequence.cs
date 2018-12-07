using System;
using System.Buffers;
using static System.Buffers.Binary.BinaryPrimitives;

namespace ProGaudi.MsgPack
{
    /// <summary>
    /// Methods for working with timestamp extensions.
    /// </summary>
    public static partial class MsgPackSpec
    {
        /// <summary>
        /// Reads unsigned 32bit seconds since unix epoch.
        /// </summary>
        /// <paramref name="sequence">Sequence to read from.</paramref>
        /// <paramref name="readSize">Count of bytes, read from <paramref name="sequence"/>.</paramref>
        /// <returns>Timestamp</returns>
        public static Timestamp ReadTimestamp32(ReadOnlySequence<byte> sequence, out int readSize)
        {
            const int length = DataLengths.TimeStamp32;
            if (sequence.First.Length >= length)
                return ReadTimestamp32(sequence.First.Span, out readSize);

            readSize = length;
            Span<byte> buffer = stackalloc byte[length];
            if (!sequence.TryRead(buffer))
                throw GetReadOnlySequenceIsTooShortException(length, sequence.Length);

            var extension = ReadFixExtension4Header(buffer, out var headerSize);
            if (extension != ExtensionTypes.Timestamp) ThrowWrongExtensionTypeException(extension, ExtensionTypes.Timestamp);
            return new Timestamp(ReadUInt32BigEndian(buffer.Slice(headerSize)));
        }

        /// <summary>
        /// Reads unsigned 64bit seconds since unix epoch.
        /// </summary>
        /// <paramref name="sequence">Sequence to read from.</paramref>
        /// <paramref name="readSize">Count of bytes, read from <paramref name="sequence"/>.</paramref>
        /// <returns>Timestamp</returns>
        public static Timestamp ReadTimestamp64(ReadOnlySequence<byte> sequence, out int readSize)
        {
            const int length = DataLengths.TimeStamp64;
            if (sequence.First.Length >= length)
                return ReadTimestamp64(sequence.First.Span, out readSize);

            readSize = length;
            Span<byte> buffer = stackalloc byte[length];
            if (!sequence.TryRead(buffer))
                throw GetReadOnlySequenceIsTooShortException(length, sequence.Length);

            var extension = ReadFixExtension8Header(buffer, out var headerSize);
            if (extension != ExtensionTypes.Timestamp) ThrowWrongExtensionTypeException(extension, ExtensionTypes.Timestamp);
            return new Timestamp(ReadUInt64BigEndian(buffer.Slice(headerSize)));
        }

        /// <summary>
        /// Reads 96bit timestamp.
        /// </summary>
        /// <paramref name="sequence">Sequence to read from.</paramref>
        /// <paramref name="readSize">Count of bytes, read from <paramref name="sequence"/>.</paramref>
        /// <returns>Timestamp</returns>
        public static Timestamp ReadTimestamp96(ReadOnlySequence<byte> sequence, out int readSize)
        {
            const int length = DataLengths.TimeStamp96;
            if (sequence.First.Length >= length)
                return ReadTimestamp96(sequence.First.Span, out readSize);

            readSize = length;
            Span<byte> buffer = stackalloc byte[length];
            if (!sequence.TryRead(buffer))
                throw GetReadOnlySequenceIsTooShortException(length, sequence.Length);

            var (extension, exLength) = ReadExtension8Header(buffer, out var headerSize);
            if (extension != ExtensionTypes.Timestamp) ThrowWrongExtensionTypeException(extension, ExtensionTypes.Timestamp);
            if (exLength != 12) ThrowWrongExtensionLengthException(exLength, 12);
            return new Timestamp(ReadInt64BigEndian(buffer.Slice(headerSize + 4)), ReadUInt32BigEndian(buffer.Slice(headerSize)));
        }

        /// <summary>
        /// Reads timestamp.
        /// </summary>
        /// <paramref name="sequence">Sequence to read from.</paramref>
        /// <paramref name="readSize">Count of bytes, read from <paramref name="sequence"/>.</paramref>
        /// <returns>Timestamp</returns>
        public static Timestamp ReadTimestamp(ReadOnlySequence<byte> sequence, out int readSize)
        {
            var code = sequence.GetFirst();
            switch (code)
            {
                case DataCodes.FixExtension4:
                    return ReadTimestamp32(sequence, out readSize);
                case DataCodes.FixExtension8:
                    return ReadTimestamp64(sequence, out readSize);
                case DataCodes.Extension8:
                    return ReadTimestamp96(sequence, out readSize);
                default:
                    ThrowWrongCodeException(
                        code,
                        DataCodes.FixExtension4,
                        DataCodes.FixExtension8,
                        DataCodes.Extension8);
                    readSize = 0;
                    return default;
            }
        }

        /// <summary>
        /// Reads unsigned 32bit seconds since unix epoch.
        /// </summary>
        /// <paramref name="sequence">Sequence to read from.</paramref>
        /// <paramref name="timestamp">Timestamp</paramref>
        /// <paramref name="readSize">Count of bytes, read from <paramref name="sequence"/>.</paramref>
        /// <returns><c>true</c> if everything is ok, <c>false</c> if:
        /// <list type="bullet">
        ///     <item><description><paramref name="sequence"/> is too small or</description></item>
        ///     <item><description><paramref name="sequence"/>[0] is not <see cref="DataCodes.FixExtension4"/> or</description></item>
        ///     <item><description>extension type is not <see cref="ExtensionTypes.Timestamp"/> or</description></item>
        ///     <item><description>we could not read uint seconds from sequence.</description></item>
        /// </list>
        /// </returns>
        public static bool TryReadTimestamp32(ReadOnlySequence<byte> sequence, out Timestamp timestamp, out int readSize)
        {
            const int length = DataLengths.TimeStamp32;
            if (sequence.First.Length >= length)
                return TryReadTimestamp32(sequence.First.Span, out timestamp, out readSize);

            readSize = length;
            timestamp = Timestamp.Zero;
            Span<byte> buffer = stackalloc byte[length];
            if (!sequence.TryRead(buffer)) return false;

            if (!TryReadFixExtension4Header(buffer, out var extension, out var headerSize)) return false;
            if (extension != ExtensionTypes.Timestamp) return false;
            if (TryReadUInt32BigEndian(buffer.Slice(headerSize), out var seconds)) return false;
            timestamp = new Timestamp(seconds);
            return true;
        }

        /// <summary>
        /// Reads unsigned 64bit seconds since unix epoch.
        /// </summary>
        /// <paramref name="sequence">Sequence to read from.</paramref>
        /// <paramref name="timestamp">Timestamp</paramref>
        /// <paramref name="readSize">Count of bytes, read from <paramref name="sequence"/>.</paramref>
        /// <returns><c>true</c> if everything is ok, <c>false</c> if:
        /// <list type="bullet">
        ///     <item><description><paramref name="sequence"/> is too small or</description></item>
        ///     <item><description><paramref name="sequence"/>[0] is not <see cref="DataCodes.FixExtension8"/> or</description></item>
        ///     <item><description>extension type is not <see cref="ExtensionTypes.Timestamp"/> or</description></item>
        ///     <item><description>we could not read ulong seconds from sequence.</description></item>
        /// </list></returns>
        public static bool TryReadTimestamp64(ReadOnlySequence<byte> sequence, out Timestamp timestamp, out int readSize)
        {
            const int length = DataLengths.TimeStamp64;
            if (sequence.First.Length >= length)
                return TryReadTimestamp32(sequence.First.Span, out timestamp, out readSize);

            readSize = length;
            timestamp = Timestamp.Zero;
            Span<byte> buffer = stackalloc byte[length];
            if (!sequence.TryRead(buffer)) return false;

            if (!TryReadFixExtension8Header(buffer, out var extension, out var headerSize)) return false;
            if (extension != ExtensionTypes.Timestamp) return false;
            if (TryReadUInt64BigEndian(buffer.Slice(headerSize), out var seconds)) return false;
            timestamp = new Timestamp(seconds);
            return true;
        }

        /// <summary>
        /// Reads 96bit timestamp.
        /// </summary>
        /// <paramref name="sequence">Sequence to read from.</paramref>
        /// <paramref name="timestamp">Timestamp</paramref>
        /// <paramref name="readSize">Count of bytes, read from <paramref name="sequence"/>.</paramref>
        /// <returns><c>true</c> if everything is ok, <c>false</c> if:
        /// <list type="bullet">
        ///     <item><description><paramref name="sequence"/> is too small or</description></item>
        ///     <item><description><paramref name="sequence"/>[0] is not <see cref="DataCodes.Extension8"/> or</description></item>
        ///     <item><description>extension type is not <see cref="ExtensionTypes.Timestamp"/> or</description></item>
        ///     <item><description>we could not read long seconds and/or uint nanoseconds from sequence.</description></item>
        /// </list></returns>
        public static bool TryReadTimestamp96(ReadOnlySequence<byte> sequence, out Timestamp timestamp, out int readSize)
        {
            const int length = DataLengths.TimeStamp96;
            if (sequence.First.Length >= length)
                return TryReadTimestamp32(sequence.First.Span, out timestamp, out readSize);

            readSize = length;
            timestamp = Timestamp.Zero;
            Span<byte> buffer = stackalloc byte[length];
            if (!sequence.TryRead(buffer)) return false;

            if (!TryReadExtension8Header(sequence, out var extension, out var exLength, out var headerSize)) return false;
            if (extension != ExtensionTypes.Timestamp) return false;
            if (exLength != 12) return false;
            if (TryReadUInt32BigEndian(buffer.Slice(headerSize), out var nanoseconds)) return false;
            if (TryReadInt64BigEndian(buffer.Slice(headerSize + 4), out var seconds)) return false;
            timestamp = new Timestamp(seconds, nanoseconds);
            return true;
        }

        /// <summary>
        /// Reads timestamp.
        /// </summary>
        /// <paramref name="sequence">Sequence to read from.</paramref>
        /// <paramref name="timestamp">Timestamp</paramref>
        /// <paramref name="readSize">Count of bytes, read from <paramref name="sequence"/>.</paramref>
        /// <returns><c>true</c> if everything is ok, <c>false</c> if:
        /// <list type="bullet">
        ///     <item><description><paramref name="sequence"/> is too small or</description></item>
        ///     <item><description><paramref name="sequence"/>[0] is not <see cref="DataCodes.FixExtension4"/>, <see cref="DataCodes.FixExtension8"/> or <see cref="DataCodes.Extension8"/> or</description></item>
        ///     <item><description>extension type is not <see cref="ExtensionTypes.Timestamp"/> or</description></item>
        ///     <item><description>we could not read data from sequence.</description></item>
        /// </list></returns>
        public static bool TryReadTimestamp(ReadOnlySequence<byte> sequence, out Timestamp timestamp, out int readSize)
        {
            switch (sequence.GetFirst())
            {
                case DataCodes.FixExtension4:
                    return TryReadTimestamp32(sequence, out timestamp, out readSize);
                case DataCodes.FixExtension8:
                    return TryReadTimestamp64(sequence, out timestamp, out readSize);
                case DataCodes.Extension8:
                    return TryReadTimestamp96(sequence, out timestamp, out readSize);
                default:
                    readSize = 0;
                    timestamp = Timestamp.Zero;
                    return false;
            }
        }
    }
}
