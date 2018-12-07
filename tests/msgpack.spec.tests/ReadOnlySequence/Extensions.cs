using System;
using System.Buffers;

namespace ProGaudi.MsgPack.Tests.ReadOnlySequence
{
    public static class Extensions
    {
        public static ReadOnlySequence<byte> ToSingleSegment(this byte[] buffer) => new ReadOnlySequence<byte>(buffer);

        public static ReadOnlySequence<byte> ToMultipleSegments(this byte[] buffer)
        {
            var length = buffer.Length;
            if (length == 1) return OneByteSequence();

            var memory = buffer.AsMemory();
            var last = new Segment(memory.Slice(1), null, length - 1);
            var lastEmpty = new Segment(ReadOnlyMemory<byte>.Empty, last, length - 1);
            var medium = new Segment(memory.Slice(1, length - 2), lastEmpty, 1);
            var firstEmpty = new Segment(ReadOnlyMemory<byte>.Empty, medium, 1);
            var first = new Segment(memory.Slice(1), firstEmpty, 0);
            return new ReadOnlySequence<byte>(first, 0, last, length);

            ReadOnlySequence<byte> OneByteSequence()
            {
                var afterByte = new Segment(ReadOnlyMemory<byte>.Empty, null, 1);
                var @byte = new Segment(buffer, afterByte, 0);
                var beforeByte = new Segment(ReadOnlyMemory<byte>.Empty, @byte, 0);

                return new ReadOnlySequence<byte>(beforeByte, 0, afterByte, 0);
            }
        }

        private sealed class Segment : ReadOnlySequenceSegment<byte>
        {
            public Segment(ReadOnlyMemory<byte> memory, ReadOnlySequenceSegment<byte> next, long runningIndex)
            {
                Memory = memory;
                Next = next;
                RunningIndex = runningIndex;
            }
        }
    }
}
