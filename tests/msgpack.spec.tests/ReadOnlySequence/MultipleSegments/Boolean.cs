using Shouldly;
using Xunit;

namespace ProGaudi.MsgPack.Tests.ReadOnlySequence.MultipleSegments
{
    public sealed class Boolean
    {
        [Theory]
        [InlineData(true, new[] { DataCodes.True })]
        [InlineData(false, new[] { DataCodes.False })]
        public void Read(bool value, byte[] data)
        {
            MsgPackSpec.ReadBoolean(data.ToMultipleSegments(), out var readSize).ShouldBe(value);
            readSize.ShouldBe(data.Length);
        }

        [Theory]
        [InlineData(true, new[] { DataCodes.True })]
        [InlineData(false, new[] { DataCodes.False })]
        public void TryRead(bool value, byte[] data)
        {
            MsgPackSpec.TryReadBoolean(data.ToMultipleSegments(), out var result, out var readSize).ShouldBeTrue();
            result.ShouldBe(value);
            readSize.ShouldBe(data.Length);
        }
    }
}
