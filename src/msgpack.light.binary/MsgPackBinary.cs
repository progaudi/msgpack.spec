using System;
using System.Runtime.CompilerServices;

namespace ProGaudi.MsgPack.Light
{
    /// <summary>
    /// Encode/Decode Utility of MessagePack Spec.
    /// https://github.com/msgpack/msgpack/blob/master/spec.md
    /// </summary>
    public static partial class MsgPackBinary
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void EnsureCapacity(in Span<byte> buffer, in int count)
        {
            if (buffer.Length < count)
            {
                throw new ArgumentOutOfRangeException(nameof(buffer));
            }
        }
    }
}