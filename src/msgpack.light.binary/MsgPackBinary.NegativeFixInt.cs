using System;
using System.Runtime.CompilerServices;

namespace ProGaudi.MsgPack.Light
{
    /// <summary>
    /// Methods for working with NegativeFixInt
    /// </summary>
    public static partial class MsgPackBinary
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteNegativeFixInt(in Span<byte> buffer, sbyte value) => TryWriteNegativeFixInt(buffer, value, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteNegativeFixInt(in Span<byte> buffer, sbyte value, out int wroteSize)
        {
            wroteSize = 1;
            if (value < DataCodes.FixNegativeMinSByte || value > DataCodes.FixNegativeMaxSByte) return false;

            buffer[0] = unchecked((byte)value);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte ReadNegativeFixInt(in ReadOnlySpan<byte> buffer, out int readSize) => TryReadNegativeFixInt(buffer, out var result, out readSize)
            ? result
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadNegativeFixInt(in ReadOnlySpan<byte> buffer, out sbyte value, out int readSize)
        {
            readSize = 1;
            value = unchecked((sbyte)buffer[0]);
            return DataCodes.FixNegativeMinSByte <= value && value <= DataCodes.FixNegativeMaxSByte;
        }
    }
}
