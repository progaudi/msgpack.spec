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
        public static int WriteNegativeFixInt(in Span<byte> buffer, sbyte value)
        {
            if (value < DataCodes.FixNegativeMinSByte || value > DataCodes.FixNegativeMaxSByte) throw new InvalidOperationException(nameof(value));

            EnsureCapacity(buffer, 1);
            buffer[0] = unchecked((byte) value);
            return 1;
        }
    }
}
