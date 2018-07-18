using System;
using System.Linq;
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
            $"Wrong data code: {code:x2}. Expected: {min:x2} <= code <= {max:x2}."
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static InvalidOperationException WrongCodeException(byte code, byte expected) => new InvalidOperationException(
            $"Wrong data code: {code:x2}. Expected: {expected:x2}."
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Exception WrongCodeException(byte code, byte a, byte b) => new InvalidOperationException(
            $"Wrong data code: {code:x2}. Expected: {a:x2} or {b:x2}."
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Exception WrongCodeException(byte code, byte a, byte b, byte c) => new InvalidOperationException(
            $"Wrong data code: {code:x2}. Expected: {a:x2}, {b:x2} or {c:x2}."
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Exception WrongCodeException(byte code, params byte[] expected) => new InvalidOperationException(
            $"Wrong data code: {code:x2}. Expected: {string.Join(", ", expected.Select(x => x.ToString("x2")))}."
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
            $"Wrong array data code: {code:x2}. Expected: {FixArrayMin:x2} <= code <= {FixArrayMax:x2} or {Array16:x2} or {Array32:x2}."
        );

    }
}
