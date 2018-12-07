using System;
using System.Buffers;
using Shouldly;
using Xunit;

namespace ProGaudi.MsgPack.Tests.Writer
{
    public sealed class Boolean
    {
        [Theory]
        [InlineData(true, new[] { DataCodes.True })]
        [InlineData(false, new[] { DataCodes.False })]
        public void Test(bool value, byte[] data)
        {
            var buffer = new Span<byte>(ArrayPool<byte>.Shared.Rent(10));
            var length = MsgPackSpec.WriteBoolean(buffer, value);
            length.ShouldBe(data.Length);
            buffer.Slice(0, length).ToArray().ShouldBe(data);
        }
    }
}
