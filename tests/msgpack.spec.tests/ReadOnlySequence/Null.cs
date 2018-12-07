using Shouldly;
using Xunit;

namespace ProGaudi.MsgPack.Tests.ReadOnlySequence
{
    public sealed class Null
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
