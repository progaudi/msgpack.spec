using System;
using System.Runtime.CompilerServices;

using static ProGaudi.MsgPack.Light.DataCodes;

namespace ProGaudi.MsgPack.Light
{
    /// <summary>
    /// Encode/Decode Utility of MessagePack Spec.
    /// https://github.com/msgpack/msgpack/blob/master/spec.md
    /// </summary>
    public static partial class MsgPackBinary
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static InvalidOperationException WrongRangeCodeException(byte code, byte min, byte max) => new InvalidOperationException(
            $"Wrong data code: {code}. Expected: {min} <= code <= {max}."
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static InvalidOperationException WrongCode(byte code, byte expected) => new InvalidOperationException(
            $"Wrong data code: {code}. Expected: {expected}."
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Exception WrongCode(byte code, byte a, byte b) => new InvalidOperationException(
            $"Wrong data code: {code}. Expected: {a} or {b}."
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Exception WrongCode(byte code, byte a, byte b, byte c) => new InvalidOperationException(
            $"Wrong data code: {code}. Expected: {a}, {b} or {c}."
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Exception LengthShouldBeNonNegative(int length) => new InvalidOperationException(
            $"Length should be nonnegative. Was {length}."
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Exception TooLargeArray(uint length) => new InvalidOperationException(
            $@"You can't create arrays longer than int.MaxValue in .net. Packet length was: {length}.
See https://blogs.msdn.microsoft.com/joshwil/2005/08/10/bigarrayt-getting-around-the-2gb-array-size-limit/"
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Exception DataIsTooLarge(int dataLength, int maxLength, string dataName, byte dataCode) => new InvalidOperationException(
            $"Data is {dataLength} bytes and it is too large for {dataName}({dataCode:x2}). Maximum is {maxLength}."
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Exception DataIsTooLarge(int dataLength, int maxLength, string dataName, byte minCode, byte maxCode) => new InvalidOperationException(
            $"Data is {dataLength} bytes and it is too large for {dataName}({minCode:x2} - {maxCode:x2}). Maximum is {maxLength}."
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Exception WrongArrayHeader(byte code) => new InvalidOperationException(
            $"Wrong array data code: {code}. Expected: {FixArrayMin} <= code <= {FixArrayMax} or {Array16} or {Array32}."
        );

    }
}
