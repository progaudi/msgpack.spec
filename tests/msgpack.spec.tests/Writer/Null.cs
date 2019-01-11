using Shouldly;
using Xunit;

namespace ProGaudi.MsgPack.Tests.Writer
{
    public sealed class Null
    {
        [Fact]
        public void ReadNull()
        {
            var buffer = new[] {DataCodes.NeverUsed};
            MsgPackSpec.WriteNil(buffer).ShouldBe(1);
            buffer[0].ShouldBe(DataCodes.Nil);
        }
    }
}
