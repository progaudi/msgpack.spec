using System;
using Shouldly;
using Xunit;

namespace ProGaudi.MsgPack.Tests.ReadOnlySequence.MultipleSegments
{
    public class TryFillSpan
    {
        [Theory]
        [InlineData(new byte[] {1})]
        [InlineData(new byte[] {byte.MaxValue, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10})]
        public void TryFillSpanByteNonEmpty(byte[] data)
        {
            var destination = new byte[5];
            data.ToMultipleSegments().TryFillSpan(destination).ShouldBe(data.Length >= destination.Length);
            var copiedLength = Math.Min(data.Length, destination.Length);
            destination.AsSpan().Slice(0, copiedLength).ToArray().ShouldBe(data.AsSpan().Slice(0, copiedLength).ToArray());
        }

        [Fact]
        public void TryFillSpanByteEmpty()
        {
            var empty = System.Array.Empty<byte>();
            empty.ToMultipleSegments().TryFillSpan(empty).ShouldBeTrue();
        }

        [Theory]
        [InlineData(new[] {1})]
        [InlineData(new[] {int.MaxValue, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10})]
        public void TryFillSpanNonByteNonEmpty(int[] data)
        {
            var destination = new int[5];
            data.ToMultipleSegments().TryFillSpan(destination).ShouldBe(data.Length >= destination.Length);
            var copiedLength = Math.Min(data.Length, destination.Length);
            destination.AsSpan().Slice(0, copiedLength).ToArray().ShouldBe(data.AsSpan().Slice(0, copiedLength).ToArray());
        }

        [Fact]
        public void TryFillSpanNonByteEmpty()
        {
            var empty = System.Array.Empty<int>();
            empty.ToMultipleSegments().TryFillSpan(empty).ShouldBeTrue();
        }
    }
}
