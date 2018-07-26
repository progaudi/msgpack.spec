using System;
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
        private static readonly FixedMemoryArrayPool _pool = new FixedMemoryArrayPool();

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

                // case "NeverUsed" be here to have happy compilator
                default:
                    return DataFamily.NeverUsed;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Exception WrongRangeCodeException(byte code, byte min, byte max) => new InvalidOperationException(
            $"Wrong data code: 0x{code:x2}. Expected: 0x{min:x2} <= code <= 0x{max:x2}."
        );

        private static Exception WrongRangeCodeException(sbyte code, sbyte min, sbyte max) => new InvalidOperationException(
            $"Wrong data code: 0x{code:x2}. Expected: 0x{min:x2} <= code <= 0x{max:x2}."
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Exception WrongCodeException(byte code, byte expected) => new InvalidOperationException(
            $"Wrong data code: 0x{code:x2}. Expected: 0x{expected:x2}."
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Exception WrongCodeException(byte code, byte a, byte b) => new InvalidOperationException(
            $"Wrong data code: 0x{code:x2}. Expected: 0x{a:x2} or 0x{b:x2}."
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Exception WrongCodeException(byte code, byte a, byte b, byte c) => new InvalidOperationException(
            $"Wrong data code: 0x{code:x2}. Expected: 0x{a:x2}, 0x{b:x2} or 0x{c:x2}."
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Exception WrongCodeException(byte code, params byte[] expected) => new InvalidOperationException(
            $"Wrong data code: 0x{code:x2}. Expected: {string.Join(", ", expected.Select(x => $"0x{x:x2}"))}."
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Exception LengthShouldBeNonNegative(int length) => new InvalidOperationException(
            $"Length should be nonnegative. Was {length}."
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Exception DataIsTooLarge(uint length) => new InvalidOperationException(
            $@"You can't create arrays or string longer than int.MaxValue in .net. Packet length was: {length}.
See https://blogs.msdn.microsoft.com/joshwil/2005/08/10/bigarrayt-getting-around-the-2gb-array-size-limit/"
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Exception DataIsTooLarge(int dataLength, int maxLength, string dataName, byte dataCode) => new InvalidOperationException(
            $"Data is {dataLength} bytes and it is too large for {dataName}(0x{dataCode:x2}). Maximum is {maxLength}."
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Exception DataIsTooLarge(int dataLength, int maxLength, string dataName, byte minCode, byte maxCode) => new InvalidOperationException(
            $"Data is {dataLength} bytes and it is too large for {dataName}(0x{minCode:x2} - 0x{maxCode:x2}). Maximum is {maxLength}."
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Exception WrongArrayHeader(byte code) => new InvalidOperationException(
            $"Wrong array data code: 0x{code:x2}. Expected: 0x{FixArrayMin:x2} <= code <= 0x{FixArrayMax:x2} or 0x{Array16:x2} or 0x{Array32:x2}."
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Exception WrongMapHeader(byte code) => new InvalidOperationException(
            $"Wrong map data code: 0x{code:x2}. Expected: 0x{FixMapMin:x2} <= code <= 0x{FixMapMax:x2} or 0x{Map16:x2} or 0x{Map32:x2}."
        );

        private static Exception CantReadEmptyBufferException() => new InvalidOperationException(
            "We need at least 1 byte to read from buffer!"
        );

        private static Exception ValueIsTooLargeException<T1, T2>(T1 value, T2 maxValue) => new InvalidOperationException(
            $"Value is {value}, maximum is {maxValue}"
        );

        private static Exception WrongIntCodeException(byte code, params byte[] expected) => new InvalidOperationException(
            $"Wrong int data code: 0x{code:x2}. Expected: 0x{FixNegativeMinSByte:x2} <= x <= 0x{FixPositiveMax:x2} or x ⊂ ({string.Join(", ", expected.Select(x => $"0x{x:x2}"))})."
        );

        private static Exception WrongUIntCodeException(byte code, params byte[] expected) => new InvalidOperationException(
            $"Wrong uint data code: 0x{code:x2}. Expected: 0 <= x <= 0x{FixPositiveMax:x2} or x ⊂ ({string.Join(", ", expected.Select(x => $"0x{x:x2}"))})."
        );

        private static Exception UnsignedIntException(long value) => new InvalidOperationException(
            $"Value {value} should be greater or equal to zero."
        );

        private static Exception WrongExtensionTypeException(sbyte extension, sbyte expected) => new InvalidOperationException(
            $"Wrong extension type: {extension}. Expected: {expected}."
        );
    }
}
