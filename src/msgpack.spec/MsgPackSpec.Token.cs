using System;
using System.Buffers;

using ProGaudi.Buffers;

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
            var result = FixedLengthMemoryPool<byte>.Shared.Rent(token.Length);
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

            var offset = 0;
            var elementsToRead = 1L;
            do
            {
                var code = buffer[offset];
                if (code <= DataCodes.FixPositiveMax)
                {
                    elementsToRead--;
                    offset++;
                    continue;
                }

                if (DataCodes.FixMapMin <= code && code <= DataCodes.FixMapMax)
                {
                    var length = ReadFixMapHeader(buffer.Slice(offset), out var readSize);
                    elementsToRead += 2 * length - 1;
                    offset += readSize;
                    continue;
                }

                if (DataCodes.FixArrayMin <= code && code <= DataCodes.FixArrayMax)
                {
                    var length = ReadFixArrayHeader(buffer.Slice(offset), out var readSize);
                    elementsToRead += length - 1;
                    offset += readSize;
                    continue;
                }

                if (DataCodes.FixStringMin <= code && code <= DataCodes.FixStringMax)
                {
                    uint length = ReadFixStringHeader(buffer.Slice(offset), out var readSize);
                    elementsToRead--;
                    offset += (int)(length + readSize);
                    continue;
                }

                if (DataCodes.FixNegativeMin <= code)
                {
                    elementsToRead--;
                    offset++;
                    continue;
                }

                switch (code)
                {
                    case DataCodes.Nil:
                    case DataCodes.True:
                    case DataCodes.False:
                        elementsToRead--;
                        offset++;
                        continue;

                    case DataCodes.Binary8:
                        uint length = ReadBinary8Header(buffer.Slice(offset), out var readSize);
                        elementsToRead--;
                        offset += (int)(length + readSize);
                        continue;

                    case DataCodes.Binary16:
                        length = ReadBinary16Header(buffer.Slice(offset), out readSize);
                        elementsToRead--;
                        offset += (int)(length + readSize);
                        continue;

                    case DataCodes.Binary32:
                        length = ReadBinary32Header(buffer.Slice(offset), out readSize);
                        elementsToRead--;
                        offset += (int)(length + readSize);
                        continue;

                    case DataCodes.Extension8:
                        length = ReadExtension8Header(buffer.Slice(offset), out readSize).length;
                        elementsToRead--;
                        offset += (int)(length + readSize);
                        continue;

                    case DataCodes.Extension16:
                        length = ReadExtension16Header(buffer.Slice(offset), out readSize).length;
                        elementsToRead--;
                        offset += (int)(length + readSize);
                        continue;

                    case DataCodes.Extension32:
                        length = ReadExtension32Header(buffer.Slice(offset), out readSize).length;
                        elementsToRead--;
                        offset += (int)(length + readSize);
                        continue;

                    case DataCodes.Float32:
                        elementsToRead--;
                        offset += DataLengths.Float32;
                        continue;

                    case DataCodes.Float64:
                        elementsToRead--;
                        offset += DataLengths.Float64;
                        continue;

                    case DataCodes.UInt8:
                        elementsToRead--;
                        offset += DataLengths.UInt8;
                        continue;

                    case DataCodes.UInt16:
                        elementsToRead--;
                        offset += DataLengths.UInt16;
                        continue;

                    case DataCodes.UInt32:
                        elementsToRead--;
                        offset += DataLengths.UInt32;
                        continue;

                    case DataCodes.UInt64:
                        elementsToRead--;
                        offset += DataLengths.UInt64;
                        continue;

                    case DataCodes.Int8:
                        elementsToRead--;
                        offset += DataLengths.Int8;
                        continue;

                    case DataCodes.Int16:
                        elementsToRead--;
                        offset += DataLengths.Int16;
                        continue;

                    case DataCodes.Int32:
                        elementsToRead--;
                        offset += DataLengths.Int32;
                        continue;

                    case DataCodes.Int64:
                        elementsToRead--;
                        offset += DataLengths.Int64;
                        continue;

                    case DataCodes.FixExtension1:
                        elementsToRead--;
                        offset += DataLengths.FixExtensionHeader + 1;
                        continue;

                    case DataCodes.FixExtension2:
                        elementsToRead--;
                        offset += DataLengths.FixExtensionHeader + 2;
                        continue;

                    case DataCodes.FixExtension4:
                        elementsToRead--;
                        offset += DataLengths.FixExtensionHeader + 4;
                        continue;

                    case DataCodes.FixExtension8:
                        elementsToRead--;
                        offset += DataLengths.FixExtensionHeader + 8;
                        continue;

                    case DataCodes.FixExtension16:
                        elementsToRead--;
                        offset += DataLengths.FixExtensionHeader + 16;
                        continue;

                    case DataCodes.String8:
                        length = ReadString8Header(buffer.Slice(offset), out readSize);
                        elementsToRead--;
                        offset += (int)(length + readSize);
                        continue;

                    case DataCodes.String16:
                        length = ReadString16Header(buffer.Slice(offset), out readSize);
                        elementsToRead--;
                        offset += (int)(length + readSize);
                        continue;

                    case DataCodes.String32:
                        length = ReadString32Header(buffer.Slice(offset), out readSize);
                        elementsToRead--;
                        offset += (int)(length + readSize);
                        continue;

                    case DataCodes.Array16:
                        length = ReadArray16Header(buffer.Slice(offset), out readSize);
                        elementsToRead += length - 1;
                        offset += readSize;
                        continue;

                    case DataCodes.Array32:
                        length = ReadArray32Header(buffer.Slice(offset), out readSize);
                        elementsToRead += length - 1;
                        offset += readSize;
                        continue;

                    case DataCodes.Map16:
                        length = ReadMap16Header(buffer.Slice(offset), out readSize);
                        elementsToRead += 2*length - 1;
                        offset += readSize;
                        continue;

                    case DataCodes.Map32:
                        length = ReadMap32Header(buffer.Slice(offset), out readSize);
                        elementsToRead += 2*length - 1;
                        offset += readSize;
                        continue;

                    // case "NeverUsed" be here to have happy compiler
                    default:
                        throw new ArgumentOutOfRangeException(nameof(buffer), $"Data code at {nameof(buffer)}[{offset}] is 0xc1 and it is invalid data code.");
                }
            } while (elementsToRead > 0);

            return buffer.Slice(0, offset);
        }
    }
}
