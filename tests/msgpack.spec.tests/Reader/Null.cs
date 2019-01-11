using System;
using Shouldly;
using Xunit;

namespace ProGaudi.MsgPack.Tests.Reader
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

        [Fact]
        public void TryReadNull()
        {
            var buffer = new [] { DataCodes.Nil };
            MsgPackSpec.TryReadNil(buffer, out var readSize).ShouldBeTrue();
            readSize.ShouldBe(buffer.Length);
        }

        [Fact]
        public void BadReadNull()
        {
            var buffer = new [] { DataCodes.True };
            var e = Should.Throw<InvalidOperationException>(() => MsgPackSpec.ReadNil(buffer, out _));
            e.Message.ShouldBe("Wrong data code: 0xc3. Expected: 0xc0.");
        }

        [Fact]
        public void BadTryReadNull()
        {
            var buffer = new [] { DataCodes.Int8 };
            MsgPackSpec.TryReadNil(buffer, out var readSize).ShouldBeFalse();
            readSize.ShouldBe(buffer.Length);
        }
    }
}
