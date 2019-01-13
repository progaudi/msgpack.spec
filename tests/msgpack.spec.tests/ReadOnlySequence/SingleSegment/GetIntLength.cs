using Shouldly;
using Xunit;

namespace ProGaudi.MsgPack.Tests.ReadOnlySequence.SingleSegment
{
    public class GetIntLength
    {
        [Theory]
        [InlineData(new byte[0])]
        [InlineData(new byte[] {1})]
        [InlineData(new byte[] {byte.MaxValue, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10})]
        public void GetIntLengthByte(byte[] data)
        {
            data.ToSingleSegment().GetIntLength().ShouldBe(data.Length);
        }

        [Theory]
        [InlineData(new int[0])]
        [InlineData(new[] {1})]
        [InlineData(new[] {int.MaxValue, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10})]
        public void GetIntLengthNonByte(int[] data)
        {
            data.ToSingleSegment().GetIntLength().ShouldBe(data.Length);
        }
    }
}
