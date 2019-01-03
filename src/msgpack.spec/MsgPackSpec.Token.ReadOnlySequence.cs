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
        public static IMemoryOwner<byte> CopyToken(ReadOnlySequence<byte> sequence)
        {
            var token = ReadToken(sequence);
            var result = FixedLengthMemoryPool<byte>.Shared.Rent(token.GetIntLength());
            token.CopyTo(result.Memory.Span);
            return result;
        }

        /// <summary>
        /// Reads token from buffer. Just ignore value for skipping it.
        /// </summary>
        public static ReadOnlySequence<byte> ReadToken(ReadOnlySequence<byte> sequence)
        {
            if (sequence.IsEmpty)
            {
                throw new ArgumentOutOfRangeException(nameof(sequence), "EOF: Sequence is empty.");
            }

            var ros = sequence;
            var elementsToRead = 1L;
            var tokenLength = 0;
            do
            {
                var subTokenLength = GetSubTokenLength();
                tokenLength += subTokenLength;
                ros = ros.Slice(subTokenLength);

                int GetSubTokenLength()
                {
                    var offset = 0;
                    var code = ros.GetFirst();
                    if (code <= DataCodes.FixPositiveMax)
                    {
                        elementsToRead--;
                        offset++;
                        return offset;
                    }

                    if (DataCodes.FixMapMin <= code && code <= DataCodes.FixMapMax)
                    {
                        var length = ReadFixMapHeader(ros.Slice(offset), out var readSize);
                        elementsToRead += 2 * length - 1;
                        offset += readSize;
                        return offset;
                    }

                    if (DataCodes.FixArrayMin <= code && code <= DataCodes.FixArrayMax)
                    {
                        var length = ReadFixArrayHeader(ros.Slice(offset), out var readSize);
                        elementsToRead += length - 1;
                        offset += readSize;
                        return offset;
                    }

                    if (DataCodes.FixStringMin <= code && code <= DataCodes.FixStringMax)
                    {
                        uint length = ReadFixStringHeader(ros.Slice(offset), out var readSize);
                        elementsToRead--;
                        offset += (int) (length + readSize);
                        return offset;
                    }

                    if (DataCodes.FixNegativeMin <= code)
                    {
                        elementsToRead--;
                        offset++;
                        return offset;
                    }

                    switch (code)
                    {
                        case DataCodes.Nil:
                        case DataCodes.True:
                        case DataCodes.False:
                            elementsToRead--;
                            offset++;
                            return offset;

                        case DataCodes.Binary8:
                            uint length = ReadBinary8Header(ros.Slice(offset), out var readSize);
                            elementsToRead--;
                            offset += (int) (length + readSize);
                            return offset;

                        case DataCodes.Binary16:
                            length = ReadBinary16Header(ros.Slice(offset), out readSize);
                            elementsToRead--;
                            offset += (int) (length + readSize);
                            return offset;

                        case DataCodes.Binary32:
                            length = ReadBinary32Header(ros.Slice(offset), out readSize);
                            elementsToRead--;
                            offset += (int) (length + readSize);
                            return offset;

                        case DataCodes.Extension8:
                            length = ReadExtension8Header(ros.Slice(offset), out readSize).length;
                            elementsToRead--;
                            offset += (int) (length + readSize);
                            return offset;

                        case DataCodes.Extension16:
                            length = ReadExtension16Header(ros.Slice(offset), out readSize).length;
                            elementsToRead--;
                            offset += (int) (length + readSize);
                            return offset;

                        case DataCodes.Extension32:
                            length = ReadExtension32Header(ros.Slice(offset), out readSize).length;
                            elementsToRead--;
                            offset += (int) (length + readSize);
                            return offset;

                        case DataCodes.Float32:
                            elementsToRead--;
                            offset += DataLengths.Float32;
                            return offset;

                        case DataCodes.Float64:
                            elementsToRead--;
                            offset += DataLengths.Float64;
                            return offset;

                        case DataCodes.UInt8:
                            elementsToRead--;
                            offset += DataLengths.UInt8;
                            return offset;

                        case DataCodes.UInt16:
                            elementsToRead--;
                            offset += DataLengths.UInt16;
                            return offset;

                        case DataCodes.UInt32:
                            elementsToRead--;
                            offset += DataLengths.UInt32;
                            return offset;

                        case DataCodes.UInt64:
                            elementsToRead--;
                            offset += DataLengths.UInt64;
                            return offset;

                        case DataCodes.Int8:
                            elementsToRead--;
                            offset += DataLengths.Int8;
                            return offset;

                        case DataCodes.Int16:
                            elementsToRead--;
                            offset += DataLengths.Int16;
                            return offset;

                        case DataCodes.Int32:
                            elementsToRead--;
                            offset += DataLengths.Int32;
                            return offset;

                        case DataCodes.Int64:
                            elementsToRead--;
                            offset += DataLengths.Int64;
                            return offset;

                        case DataCodes.FixExtension1:
                            elementsToRead--;
                            offset += DataLengths.FixExtensionHeader + 1;
                            return offset;

                        case DataCodes.FixExtension2:
                            elementsToRead--;
                            offset += DataLengths.FixExtensionHeader + 2;
                            return offset;

                        case DataCodes.FixExtension4:
                            elementsToRead--;
                            offset += DataLengths.FixExtensionHeader + 4;
                            return offset;

                        case DataCodes.FixExtension8:
                            elementsToRead--;
                            offset += DataLengths.FixExtensionHeader + 8;
                            return offset;

                        case DataCodes.FixExtension16:
                            elementsToRead--;
                            offset += DataLengths.FixExtensionHeader + 16;
                            return offset;

                        case DataCodes.String8:
                            length = ReadString8Header(ros.Slice(offset), out readSize);
                            elementsToRead--;
                            offset += (int) (length + readSize);
                            return offset;

                        case DataCodes.String16:
                            length = ReadString16Header(ros.Slice(offset), out readSize);
                            elementsToRead--;
                            offset += (int) (length + readSize);
                            return offset;

                        case DataCodes.String32:
                            length = ReadString32Header(ros.Slice(offset), out readSize);
                            elementsToRead--;
                            offset += (int) (length + readSize);
                            return offset;

                        case DataCodes.Array16:
                            length = ReadArray16Header(ros.Slice(offset), out readSize);
                            elementsToRead += length - 1;
                            offset += readSize;
                            return offset;

                        case DataCodes.Array32:
                            length = ReadArray32Header(ros.Slice(offset), out readSize);
                            elementsToRead += length - 1;
                            offset += readSize;
                            return offset;

                        case DataCodes.Map16:
                            length = ReadMap16Header(ros.Slice(offset), out readSize);
                            elementsToRead += 2 * length - 1;
                            offset += readSize;
                            return offset;

                        case DataCodes.Map32:
                            length = ReadMap32Header(ros.Slice(offset), out readSize);
                            elementsToRead += 2 * length - 1;
                            offset += readSize;
                            return offset;

                        // case "NeverUsed" be here to have happy compiler
                        default:
                            throw new ArgumentOutOfRangeException(nameof(sequence), $"Data code at {nameof(sequence)}[{tokenLength + offset}] is 0xc1 and it is invalid data code.");
                    }
                }
            } while (elementsToRead > 0);

            return sequence.Slice(0, tokenLength);
        }
    }
}
