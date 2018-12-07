using System;
using System.Buffers;
using System.Linq;
using System.Runtime.CompilerServices;

using static ProGaudi.MsgPack.DataCodes;

namespace ProGaudi.MsgPack
{
    /// <summary>
    /// Encode/Decode Utility of MessagePack Spec.
    /// https://github.com/msgpack/msgpack/blob/master/spec.md
    /// </summary>
    public static partial class MsgPackSpec
    {
        /// <summary>
        /// Provides mapping of first byte of <paramref name="buffer"/> high level <see cref="DataFamily"/>. Will be useful for writing converters for complex types.
        /// </summary>
        public static DataFamily GetDataFamily(ReadOnlySpan<byte> buffer) => GetDataFamily(buffer[0]);

        /// <summary>
        /// Provides mapping to high level <see cref="DataFamily"/>. Will be useful for writing converters for complex types.
        /// </summary>
        public static DataFamily GetDataFamily(byte code)
        {
            if (code <= FixPositiveMax) return DataFamily.Integer;
            if (FixMapMin <= code && code <= FixMapMax) return DataFamily.Map;
            if (FixArrayMin <= code && code <= FixArrayMax) return DataFamily.Array;
            if (FixStringMin <= code && code <= FixStringMax) return DataFamily.String;
            if (FixNegativeMin <= code) return DataFamily.Integer;
            switch (code)
            {
                case Nil: return DataFamily.Nil;
                case True:
                case False:
                    return DataFamily.Boolean;

                case Binary8:
                case Binary16:
                case Binary32:
                    return DataFamily.Binary;

                case Extension8:
                case Extension16:
                case Extension32:
                    return DataFamily.Extension;

                case Float32:
                case Float64:
                    return DataFamily.Float;

                case UInt8:
                case DataCodes.UInt16:
                case DataCodes.UInt32:
                case DataCodes.UInt64:
                case Int8:
                case DataCodes.Int16:
                case DataCodes.Int32:
                case DataCodes.Int64:
                    return DataFamily.Integer;

                case FixExtension1:
                case FixExtension2:
                case FixExtension4:
                case FixExtension8:
                case FixExtension16:
                    return DataFamily.Extension;

                case String8:
                case String16:
                case String32:
                    return DataFamily.String;

                case Array16:
                case Array32:
                    return DataFamily.Array;

                case Map16:
                case Map32:
                    return DataFamily.Map;

                // case "NeverUsed" be here to have happy compiler
                default:
                    return DataFamily.NeverUsed;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T GetFirst<T>(this ReadOnlySequence<T> ros)
        {
            if (ros.IsSingleSegment) return ros.First.Span[0];
            if (!ros.IsEmpty)
            {
                foreach (var memory in ros)
                {
                    if (!memory.IsEmpty)
                        return memory.Span[0];
                }
            }

            throw GetReadOnlySequenceIsTooShortException(1, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryRead<T>(this ReadOnlySequence<T> ros, Span<T> destination)
        {
            if (destination.IsEmpty) return true;
            var index = 0;
            foreach (var memory in ros)
            {
                for (var i = 0; i < memory.Length; i++)
                {
                    destination[index++] = memory.Span[i];
                    if (index == destination.Length)
                        return true;
                }
            }

            return false;
        }

        private static Exception GetReadOnlySequenceIsTooShortException(int expected, long sequenceLength) => new ArgumentOutOfRangeException(
            nameof(sequenceLength),
            sequenceLength,
            $"ReadOnlySequence is too short. Expected: {expected}"
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte ThrowWrongRangeCodeException(byte code, byte min, byte max) => throw new InvalidOperationException(
            $"Wrong data code: 0x{code:x2}. Expected: 0x{min:x2} <= code <= 0x{max:x2}."
        );

        private static byte ThrowWrongRangeCodeException(sbyte code, sbyte min, sbyte max) => throw new InvalidOperationException(
            $"Wrong data code: 0x{code:x2}. Expected: 0x{min:x2} <= code <= 0x{max:x2}."
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte ThrowWrongCodeException(byte code, byte expected) => throw new InvalidOperationException(
            $"Wrong data code: 0x{code:x2}. Expected: 0x{expected:x2}."
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte ThrowWrongCodeException(byte code, byte a, byte b) => throw new InvalidOperationException(
            $"Wrong data code: 0x{code:x2}. Expected: 0x{a:x2} or 0x{b:x2}."
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte ThrowWrongCodeException(byte code, byte a, byte b, byte c) => throw new InvalidOperationException(
            $"Wrong data code: 0x{code:x2}. Expected: 0x{a:x2}, 0x{b:x2} or 0x{c:x2}."
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte ThrowWrongCodeException(byte code, params byte[] expected) => throw new InvalidOperationException(
            $"Wrong data code: 0x{code:x2}. Expected: {string.Join(", ", expected.Select(x => $"0x{x:x2}"))}."
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte ThrowLengthShouldBeNonNegative(int length) => throw new InvalidOperationException(
            $"Length should be nonnegative. Was {length}."
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte ThrowDataIsTooLarge(long length) => throw new InvalidOperationException(
            $@"You can't create arrays or string longer than int.MaxValue in .net. Packet length was: {length}.
See https://blogs.msdn.microsoft.com/joshwil/2005/08/10/bigarrayt-getting-around-the-2gb-array-size-limit/"
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte ThrowDataIsTooLarge(int dataLength, int maxLength, string dataName, byte dataCode) => throw new InvalidOperationException(
            $"Data is {dataLength} bytes and it is too large for {dataName}(0x{dataCode:x2}). Maximum is {maxLength}."
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte ThrowDataIsTooLarge(int dataLength, int maxLength, string dataName, byte minCode, byte maxCode) => throw new InvalidOperationException(
            $"Data is {dataLength} bytes and it is too large for {dataName}(0x{minCode:x2} - 0x{maxCode:x2}). Maximum is {maxLength}."
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte ThrowWrongArrayHeader(byte code) => throw new InvalidOperationException(
            $"Wrong array data code: 0x{code:x2}. Expected: 0x{FixArrayMin:x2} <= code <= 0x{FixArrayMax:x2} or 0x{Array16:x2} or 0x{Array32:x2}."
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte ThrowWrongMapHeader(byte code) => throw new InvalidOperationException(
            $"Wrong map data code: 0x{code:x2}. Expected: 0x{FixMapMin:x2} <= code <= 0x{FixMapMax:x2} or 0x{Map16:x2} or 0x{Map32:x2}."
        );

        private static byte ThrowCantReadEmptyBufferException() => throw new InvalidOperationException(
            "We need at least 1 byte to read from buffer!"
        );

        private static byte ThrowValueIsTooLargeException<T1, T2>(T1 value, T2 maxValue) => throw new InvalidOperationException(
            $"Value is {value}, maximum is {maxValue}"
        );

        private static byte ThrowWrongIntCodeException(byte code, params byte[] expected) => throw new InvalidOperationException(
            $"Wrong int data code: 0x{code:x2}. Expected: 0x{FixNegativeMinSByte:x2} <= x <= 0x{FixPositiveMax:x2} or x ⊂ ({string.Join(", ", expected.Select(x => $"0x{x:x2}"))})."
        );

        private static byte ThrowWrongUIntCodeException(byte code, params byte[] expected) => throw new InvalidOperationException(
            $"Wrong uint data code: 0x{code:x2}. Expected: 0 <= x <= 0x{FixPositiveMax:x2} or x ⊂ ({string.Join(", ", expected.Select(x => $"0x{x:x2}"))})."
        );

        private static byte ThrowUnsignedIntException(long value) => throw new InvalidOperationException(
            $"Value {value} should be greater or equal to zero."
        );

        private static byte ThrowWrongExtensionTypeException(sbyte extension, sbyte expected) => throw new InvalidOperationException(
            $"Wrong extension type: 0x{extension:x2}. Expected: 0x{expected:x2}."
        );

        private static byte ThrowWrongExtensionLengthException(byte length, int expected) => throw new InvalidOperationException(
            $"Wrong extension length: {length}. Expected: {expected}."
        );
    }
}
