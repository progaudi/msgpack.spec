using System;
using System.Buffers;

namespace ProGaudi.MsgPack
{
    /// <summary>
    /// Methods for working with tokens
    /// </summary>
    public static partial class MsgPackSpec
    {
        /// <summary>
        /// Copies token from buffer.
        /// </summary>
        /// <returns>Returns memory with length equal to read data.</returns>
        public static IMemoryOwner<byte> CopyToken(ReadOnlySpan<byte> buffer)
        {
            var token = ReadToken(buffer);
            var result = _pool.Rent(token.Length);
            token.CopyTo(result.Memory.Span);
            return result;
        }

        /// <summary>
        /// Reads token from buffer. Just ignore value for skipping it.
        /// </summary>
        public static ReadOnlySpan<byte> ReadToken(ReadOnlySpan<byte> buffer)
        {
            if (buffer.IsEmpty)
            {
                throw new ArgumentOutOfRangeException(nameof(buffer), "EOF: Buffer is empty.");
            }

            var code = buffer[0];
            if (code <= DataCodes.FixPositiveMax) return buffer.Slice(0, 1);
            if (DataCodes.FixMapMin <= code && code <= DataCodes.FixMapMax)
                return ReadMap(buffer, ReadFixMapHeader(buffer, out var readSize), readSize);
            if (DataCodes.FixArrayMin <= code && code <= DataCodes.FixArrayMax)
                return ReadArray(buffer, ReadFixArrayHeader(buffer, out var readSize), readSize);
            if (DataCodes.FixStringMin <= code && code <= DataCodes.FixStringMax)
                return ReadString(buffer, ReadFixStringHeader(buffer, out var readSize), readSize);

            if (DataCodes.FixNegativeMin <= code) return buffer.Slice(0, 1);
            switch (code)
            {
                case DataCodes.Nil:
                case DataCodes.True:
                case DataCodes.False:
                    return buffer.Slice(0, 1);

                case DataCodes.Binary8:
                    return buffer.Slice(0, ReadBinary8Header(buffer, out var readSize) + readSize);
                case DataCodes.Binary16:
                    return buffer.Slice(0, ReadBinary16Header(buffer, out readSize) + readSize);
                case DataCodes.Binary32:
                    var binaryLength = ReadBinary32Header(buffer, out readSize);
                    if (int.MaxValue - readSize < binaryLength)
                        throw DataIsTooLarge(binaryLength);
                    return buffer.Slice(0, (int)(binaryLength + readSize));

                case DataCodes.Extension8:
                    return buffer.Slice(0, ReadExtension8Header(buffer, out readSize).length + readSize);
                case DataCodes.Extension16:
                    return buffer.Slice(0, ReadExtension16Header(buffer, out readSize).length + readSize);
                case DataCodes.Extension32:
                    var (_, extensionLength) = ReadExtension32Header(buffer, out readSize);
                    if (int.MaxValue - readSize < extensionLength)
                        throw DataIsTooLarge(extensionLength);
                    return buffer.Slice(0, (int)(extensionLength + readSize));

                case DataCodes.Float32:
                    return buffer.Slice(0, 5);
                case DataCodes.Float64:
                    return buffer.Slice(0, 9);

                case DataCodes.UInt8:
                    return buffer.Slice(0, 1);
                case DataCodes.UInt16:
                    return buffer.Slice(0, 3);
                case DataCodes.UInt32:
                    return buffer.Slice(0, 5);
                case DataCodes.UInt64:
                    return buffer.Slice(0, 9);
                case DataCodes.Int8:
                    return buffer.Slice(0, 1);
                case DataCodes.Int16:
                    return buffer.Slice(0, 3);
                case DataCodes.Int32:
                    return buffer.Slice(0, 5);
                case DataCodes.Int64:
                    return buffer.Slice(0, 9);

                case DataCodes.FixExtension1:
                    return buffer.Slice(0, 3);
                case DataCodes.FixExtension2:
                    return buffer.Slice(0, 4);
                case DataCodes.FixExtension4:
                    return buffer.Slice(0, 6);
                case DataCodes.FixExtension8:
                    return buffer.Slice(0, 8);
                case DataCodes.FixExtension16:
                    return buffer.Slice(0, 18);

                case DataCodes.String8:
                    return ReadString(buffer, ReadString8Header(buffer, out readSize), readSize);
                case DataCodes.String16:
                    return ReadString(buffer, ReadString16Header(buffer, out readSize), readSize);
                case DataCodes.String32:
                    var stringLength = ReadString32Header(buffer, out readSize);
                    return ReadString(buffer, stringLength, readSize);

                case DataCodes.Array16:
                    return ReadArray(buffer, ReadArray16Header(buffer, out readSize), readSize);
                case DataCodes.Array32:
                    return ReadArray(buffer, ReadArray32Header(buffer, out readSize), readSize);

                case DataCodes.Map16:
                    return ReadMap(buffer, ReadMap16Header(buffer, out readSize), readSize);
                case DataCodes.Map32:
                    return ReadMap(buffer, ReadMap32Header(buffer, out readSize), readSize);

                // case "NeverUsed" be here to have happy compilator
                default:
                    throw new ArgumentOutOfRangeException(nameof(buffer), "Data code is 0xc1 and it is invalid data code.");
            }

            ReadOnlySpan<byte> ReadString(in ReadOnlySpan<byte> b, uint length, int readSize)
            {
                if (int.MaxValue - readSize < length) throw DataIsTooLarge(length);
                return b.Slice(0, (int)(length + readSize));
            }

            ReadOnlySpan<byte> ReadMap(in ReadOnlySpan<byte> b, uint length, int readSize)
            {
                if (int.MaxValue - readSize < length) throw DataIsTooLarge(length);
                for (var i = 0u; i < length; i++)
                {
                    readSize += ReadToken(b.Slice(readSize)).Length;
                    readSize += ReadToken(b.Slice(readSize)).Length;
                }

                return b.Slice(0, readSize);
            }

            ReadOnlySpan<byte> ReadArray(in ReadOnlySpan<byte> b, uint length, int readSize)
            {
                if (int.MaxValue - readSize < length) throw DataIsTooLarge(length);
                for (var i = 0u; i < length; i++)
                {
                    readSize += ReadToken(b.Slice(readSize)).Length;
                }

                return b.Slice(0, readSize);
            }
        }
    }
}
