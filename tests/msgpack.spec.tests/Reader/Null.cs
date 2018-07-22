using Shouldly;
using Xunit;

namespace ProGaudi.MsgPack.Tests.Reader
{
    public class Null
    {
        [Fact]
        public void ReadNull()
        {
            var buffer = new [] { DataCodes.Nil };
            MsgPackSpec.ReadNil(buffer, out var readSize);
            readSize.ShouldBe(buffer.Length);
        }
    }
}
