using Shouldly;
using Xunit;

namespace ProGaudi.MsgPack.Tests.Reader
{
    public sealed class Boolean
    {
        [Theory]
        [InlineData(true, new[] { DataCodes.True })]
        [InlineData(false, new[] { DataCodes.False })]
        public void Test(bool value, byte[] data)
        {
            MsgPackSpec.ReadBoolean(data, out var readSize).ShouldBe(value);
            readSize.ShouldBe(data.Length);
        }
    }
}
