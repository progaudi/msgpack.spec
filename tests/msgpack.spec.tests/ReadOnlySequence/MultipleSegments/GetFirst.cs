using System;
using Shouldly;
using Xunit;

namespace ProGaudi.MsgPack.Tests.ReadOnlySequence.MultipleSegments
{
    public class PublicExtensions
    {
        [Theory]
        [InlineData(new byte[] {1})]
        [InlineData(new byte[] {byte.MaxValue, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10})]
        public void GetFirstByteNonEmpty(byte[] data)
        {
            data.ToMultipleSegments().GetFirst().ShouldBe(data[0]);
        }

        [Fact]
        public void GetFirstByteEmpty()
        {
            var e = Should.Throw<IndexOutOfRangeException>(() => System.Array.Empty<byte>().ToMultipleSegments().GetFirst());
            e.Message.ShouldBe("ReadOnlySequence is too short. Expected: 1, actual length: 0");
        }

        [Theory]
        [InlineData(new[] {1})]
        [InlineData(new[] {int.MaxValue, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10})]
        public void GetFirstNonByteNonEmpty(int[] data)
        {
            data.ToMultipleSegments().GetFirst().ShouldBe(data[0]);
        }

        [Fact]
        public void GetFirstNonByteEmpty()
        {
            var e = Should.Throw<IndexOutOfRangeException>(() => System.Array.Empty<byte>().ToMultipleSegments().GetFirst());
            e.Message.ShouldBe("ReadOnlySequence is too short. Expected: 1, actual length: 0");
        }
    }
}
