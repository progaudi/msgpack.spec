using Shouldly;

using Xunit;

namespace ProGaudi.MsgPack.Light.Tests.Writer
{
    public class Null
    {
        [Fact]
        public void ReadNull()
        {
            var buffer = new[] {DataCodes.NeverUsed};
            MsgPackBinary.WriteNil(buffer).ShouldBe(1);
            buffer[0].ShouldBe(DataCodes.Nil);
        }
    }
}
