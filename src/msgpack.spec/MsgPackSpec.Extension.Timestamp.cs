using System;
using static System.Buffers.Binary.BinaryPrimitives;

namespace ProGaudi.MsgPack
{
    /// <summary>
    /// Methods for working with timestamp extensions.
    /// </summary>
    public static partial class MsgPackSpec
    {
        /// <summary>
        /// Writes unsigned 32bit seconds since unix epoch.
        /// </summary>
        /// <returns>Count of bytes, written to <paramref name="buffer"/>.</returns>
        public static int WriteTimestamp32(Span<byte> buffer, in Timestamp timestamp)
        {
            WriteUInt32BigEndian(buffer.Slice(2), timestamp.EpochSeconds);
            WriteFixExtension4Header(buffer, ExtensionTypes.Timestamp);
            return 6;
        }

        /// <summary>
        /// Writes the number of seconds and nanoseconds that have elapsed since unix epoch.
        /// </summary>
        /// <remarks>Nanoseconds is 30-bit unsigned int and seconds is in 34-bit unsigned int.</remarks>
        /// <returns>Count of bytes, written to <paramref name="buffer"/>.</returns>
        public static int WriteTimestamp64(Span<byte> buffer, in Timestamp timestamp)
        {
            WriteUInt64BigEndian(buffer.Slice(2), timestamp.Epoch64);
            WriteFixExtension8Header(buffer, ExtensionTypes.Timestamp);
            return 10;
        }

        /// <summary>
        /// Writes the number of seconds and nanoseconds that have elapsed since unix epoch.
        /// </summary>
        /// <remarks>Nanoseconds is 32-bit unsigned int and seconds is in 64-bit signed int.</remarks>
        /// <returns>Count of bytes, written to <paramref name="buffer"/>.</returns>
        public static int WriteTimestamp96(Span<byte> buffer, in Timestamp timestamp)
        {
            WriteInt64BigEndian(buffer.Slice(7), timestamp.Seconds);
            WriteUInt32BigEndian(buffer.Slice(3), timestamp.NanoSeconds);
            WriteExtension8Header(buffer, ExtensionTypes.Timestamp, 12);
            return 15;
        }

        /// <summary>
        /// Writes smallest representation of timestamp.
        /// </summary>
        /// <returns>Count of bytes, written to <paramref name="buffer"/>.</returns>
        public static int WriteTimestamp(Span<byte> buffer, in Timestamp timestamp)
        {
            if (timestamp.Seconds >> 34 != 0)
                return WriteTimestamp96(buffer, timestamp);

            var data64 = (timestamp.NanoSeconds << 34) | (ulong)timestamp.Seconds;

            return (data64 & 0xffffffff00000000UL) == 0
                ? WriteTimestamp32(buffer, timestamp)
                : WriteTimestamp64(buffer, timestamp);
        }

        /// <summary>
        /// Tries to writes unsigned 32bit seconds since unix epoch.
        /// </summary>
        /// <paramref name="buffer">Buffer to write to.</paramref>
        /// <paramref name="timestamp">Timestamp to write.</paramref>
        /// <paramref name="wroteSize">Count of bytes, written to <paramref name="buffer"/>.</paramref>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if:
        /// <list type="bullet">
        ///     <item><description><paramref name="buffer"/> is too small or</description></item>
        ///     <item><description><paramref name="timestamp"/> ⊄ [1970-01-01 00:00:00 UTC, 2106-02-07 06:28:16 UTC)</description></item>
        /// </list>
        /// </returns>
        public static bool TryWriteTimestamp32(Span<byte> buffer, in Timestamp timestamp, out int wroteSize)
        {
            wroteSize = 6;
            if (buffer.Length < wroteSize) return false;
            if (!(0 <= timestamp.Seconds && timestamp.Seconds <= uint.MaxValue)) return false;
            WriteUInt32BigEndian(buffer.Slice(2), timestamp.EpochSeconds);
            WriteFixExtension4Header(buffer, ExtensionTypes.Timestamp);
            return true;
        }

        /// <summary>
        /// Tries to the number of seconds and nanoseconds that have elapsed since unix epoch.
        /// </summary>
        /// <paramref name="buffer">Buffer to write to.</paramref>
        /// <paramref name="timestamp">Timestamp to write.</paramref>
        /// <paramref name="wroteSize">Count of bytes, written to <paramref name="buffer"/>.</paramref>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if:
        /// <list type="bullet">
        ///     <item><description><paramref name="buffer"/> is too small or</description></item>
        ///     <item><description><paramref name="timestamp"/> ⊄ [1970-01-01 00:00:00.000000000 UTC, 2514-05-30 01:53:04.000000000 UTC)</description></item>
        /// </list>
        /// </returns>
        public static bool TryWriteTimestamp64(Span<byte> buffer, in Timestamp timestamp, out int wroteSize)
        {
            wroteSize = 10;
            if (buffer.Length < wroteSize) return false;
            if (timestamp.Seconds >> 34 != 0) return false;
            WriteUInt64BigEndian(buffer.Slice(2), timestamp.Epoch64);
            WriteFixExtension8Header(buffer, ExtensionTypes.Timestamp);
            return true;
        }

        /// <summary>
        /// Tries to write the number of seconds and nanoseconds.
        /// </summary>
        /// <paramref name="buffer">Buffer to write to.</paramref>
        /// <paramref name="timestamp">Timestamp to write.</paramref>
        /// <paramref name="wroteSize">Count of bytes, written to <paramref name="buffer"/>.</paramref>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if:
        /// <list type="bullet">
        ///     <item><description><paramref name="buffer"/> is too small or</description></item>
        ///     <item><description><paramref name="timestamp"/> ⊄ [-584554047284-02-23 16:59:44 UTC, 584554051223-11-09 07:00:16.000000000 UTC)</description></item>
        /// </list>
        /// </returns>
        public static bool TryWriteTimestamp96(Span<byte> buffer, in Timestamp timestamp, out int wroteSize)
        {
            wroteSize = 15;
            if (buffer.Length < wroteSize) return false;
            WriteInt64BigEndian(buffer.Slice(7), timestamp.Seconds);
            WriteUInt32BigEndian(buffer.Slice(3), timestamp.NanoSeconds);
            WriteExtension8Header(buffer, ExtensionTypes.Timestamp, 12);
            return false;
        }

        /// <summary>
        /// Tries to write smallest possible representation of <paramref name="timestamp"/> into 
        /// </summary>
        /// <paramref name="buffer">Buffer to write to.</paramref>
        /// <paramref name="timestamp">Timestamp to write.</paramref>
        /// <paramref name="wroteSize">Count of bytes, written to <paramref name="buffer"/>.</paramref>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="buffer"/> is too small.</returns>
        public static bool TryWriteTimestamp(Span<byte> buffer, in Timestamp timestamp, out int wroteSize)
        {
            if (timestamp.Seconds >> 34 != 0)
                return TryWriteTimestamp96(buffer, timestamp, out wroteSize);

            var data64 = (timestamp.NanoSeconds << 34) | (ulong)timestamp.Seconds;

            return (data64 & 0xffffffff00000000UL) == 0
                ? TryWriteTimestamp32(buffer, timestamp, out wroteSize)
                : TryWriteTimestamp64(buffer, timestamp, out wroteSize);
        }

        /// <summary>
        /// Writes unsigned 32bit seconds since unix epoch.
        /// </summary>
        /// <returns>Count of bytes, written to <paramref name="buffer"/>.</returns>
        public static Timestamp ReadTimestamp32(ReadOnlySpan<byte> buffer, out int readSize)
        {
            readSize = 6;
            var extension = ReadFixExtension4Header(buffer, out _);
            if (extension != ExtensionTypes.Timestamp) throw WrongExtensionTypeException(extension, ExtensionTypes.Timestamp);
            return new Timestamp(ReadUInt32BigEndian(buffer.Slice(readSize)));
        }

        /// <summary>
        /// Writes unsigned 64bit seconds since unix epoch.
        /// </summary>
        /// <returns>Count of bytes, written to <paramref name="buffer"/>.</returns>
        public static Timestamp ReadTimestamp64(ReadOnlySpan<byte> buffer, out int readSize)
        {
            readSize = 10;
            var extension = ReadFixExtension8Header(buffer, out _);
            if (extension != ExtensionTypes.Timestamp) throw WrongExtensionTypeException(extension, ExtensionTypes.Timestamp);
            return new Timestamp(ReadUInt64BigEndian(buffer.Slice(readSize)));
        }
    }
}
