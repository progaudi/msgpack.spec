using Shouldly;
using Xunit;

namespace ProGaudi.MsgPack.Tests.ReadOnlySequence
{
    public sealed class Null
    {
        [Fact]
        public void SingleSegmentReadNull()
        {
            var buffer = new [] { DataCodes.Nil };
            MsgPackSpec.ReadNil(buffer.ToSingleSegment(), out var readSize);
            readSize.ShouldBe(buffer.Length);
        }

        [Fact]
        public void MultipleSegmentsReadNull()
        {
            var buffer = new [] { DataCodes.Nil };
            MsgPackSpec.ReadNil(buffer.ToMultipleSegments(), out var readSize);
            readSize.ShouldBe(buffer.Length);
        }
    }
}
