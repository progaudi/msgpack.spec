using System;
using System.Buffers;
using Shouldly;
using Xunit;

namespace ProGaudi.MsgPack.Tests.ReadOnlySequence.MultipleSegments
{
    public class GetIntLength
    {
        [Theory]
        [InlineData(new byte[0])]
        [InlineData(new byte[] {1})]
        [InlineData(new byte[] {byte.MaxValue, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10})]
        public void GetIntLengthByte(byte[] data)
        {
            data.ToMultipleSegments().GetIntLength().ShouldBe(data.Length);
        }

        [Theory]
        [InlineData(new int[0])]
        [InlineData(new[] {1})]
        [InlineData(new[] {int.MaxValue, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10})]
        public void GetIntLengthNonByte(int[] data)
        {
            data.ToMultipleSegments().GetIntLength().ShouldBe(data.Length);
        }

        [Fact]
        public void LongLength()
        {
            using (var megabyte = MemoryPool<byte>.Shared.Rent(1024 * 1024))
            {
                var memory = megabyte.Memory;
                var desiredLength = (1 + (int.MaxValue + 1L) / memory.Length) * memory.Length;
                desiredLength.ShouldBeGreaterThan(int.MaxValue);

                var sequence = CreateReadOnlySequence(memory, desiredLength);
                sequence.Length.ShouldBe(desiredLength);
                var e = Should.Throw<InvalidOperationException>(() => sequence.GetIntLength());
                e.Message.ShouldBe($@"You can't create arrays or string longer than int.MaxValue in .net. Packet length was: {desiredLength}.
See https://blogs.msdn.microsoft.com/joshwil/2005/08/10/bigarrayt-getting-around-the-2gb-array-size-limit/");
            }

            ReadOnlySequence<byte> CreateReadOnlySequence(Memory<byte> memory, long desiredLength)
            {
                ReadOnlySequenceSegment<byte> end = null;
                ReadOnlySequenceSegment<byte> current = null;
                while (desiredLength > 0)
                {
                    desiredLength -= memory.Length;
                    var next = new LongSequenceSegment<byte>(memory, desiredLength, current);
                    current = next;
                    end = end ?? next;
                }

                var sequence = new ReadOnlySequence<byte>(current, 0, end, end.Memory.Length);
                return sequence;
            }
        }

        private sealed class LongSequenceSegment<T> : ReadOnlySequenceSegment<T>
        {
            public LongSequenceSegment(in Memory<T> memory, long runningIndex, ReadOnlySequenceSegment<T> next)
            {
                Memory = memory;
                RunningIndex = runningIndex;
                Next = next;
            }
        }
    }
}
