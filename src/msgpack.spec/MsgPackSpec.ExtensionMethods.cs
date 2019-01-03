using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace ProGaudi.MsgPack
{
    public static partial class MsgPackSpec
    {
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
            if (destination.Length == 0) return true;
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



        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetIntLength<T>(this ReadOnlySequence<T> ros)
        {
            var length = ros.Length;
            if (length > int.MaxValue)
                return ThrowDataIsTooLarge(length);

            return (int)length;
        }
    }
}
