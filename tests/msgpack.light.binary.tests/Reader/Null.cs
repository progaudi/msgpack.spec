using Shouldly;

using Xunit;

namespace ProGaudi.MsgPack.Light.Tests.Reader
{
    public class Null
    {
        [Fact]
        public void ReadNull()
        {
            var buffer = new [] { DataCodes.Nil };
            MsgPackBinary.ReadNil(buffer, out var readSize);
            readSize.ShouldBe(buffer.Length);
        }
    }
}
