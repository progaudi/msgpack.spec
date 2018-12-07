using Shouldly;
using Xunit;

namespace ProGaudi.MsgPack.Tests.ReadOnlySequence
{
    public sealed class Boolean
    {
        [Theory]
        [InlineData(true, new[] { DataCodes.True })]
        [InlineData(false, new[] { DataCodes.False })]
        public void SingleSegment(bool value, byte[] data)
        {
            MsgPackSpec.ReadBoolean(data.ToSingleSegment(), out var readSize).ShouldBe(value);
            readSize.ShouldBe(data.Length);
        }

        [Theory]
        [InlineData(true, new[] { DataCodes.True })]
        [InlineData(false, new[] { DataCodes.False })]
        public void MultipleSegments(bool value, byte[] data)
        {
            MsgPackSpec.ReadBoolean(data.ToMultipleSegments(), out var readSize).ShouldBe(value);
            readSize.ShouldBe(data.Length);
        }
    }
}
