using Shouldly;
using Xunit;

namespace ProGaudi.MsgPack.Tests.ReadOnlySequence.MultipleSegments
{
    public sealed class Integers
    {
        [Theory]
        [InlineData(0, new byte[] { 0x00 })]
        [InlineData(1, new byte[] { 1 })]
        [InlineData(-1, new byte[] { 0xff })]
        [InlineData(sbyte.MinValue, new byte[] { 208, 128 })]
        [InlineData(sbyte.MaxValue, new byte[] { 127 })]
        [InlineData(short.MinValue, new byte[] { 209, 128, 0 })]
        [InlineData(short.MaxValue, new byte[] { 209, 127, 0xff })]
        [InlineData(int.MinValue, new byte[] { 210, 128, 0, 0, 0 })]
        [InlineData(int.MaxValue, new byte[] { 210, 127, 0xff, 0xff, 0xff })]
        [InlineData(long.MaxValue, new byte[] { 211, 127, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff })]
        [InlineData(long.MinValue, new byte[] { 211, 128, 0, 0, 0, 0, 0, 0, 0 })]
        [InlineData(long.MaxValue, new byte[] { 207, 127, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff })]
        public void ReadSignedLong(long number, byte[] data)
        {
            MsgPackSpec.ReadInt64(data.ToMultipleSegments(), out var readSize).ShouldBe(number);
            readSize.ShouldBe(data.Length);
        }

        [Theory]
        [InlineData(0, new byte[] { 0x00 })]
        [InlineData(1, new byte[] { 1 })]
        [InlineData(-1, new byte[] { 0xff })]
        [InlineData(sbyte.MinValue, new byte[] { 208, 128 })]
        [InlineData(sbyte.MaxValue, new byte[] { 127 })]
        [InlineData(short.MinValue, new byte[] { 209, 128, 0 })]
        [InlineData(short.MaxValue, new byte[] { 209, 127, 0xff })]
        [InlineData(int.MinValue, new byte[] { 210, 128, 0, 0, 0 })]
        [InlineData(int.MaxValue, new byte[] { 210, 127, 0xff, 0xff, 0xff })]
        [InlineData(50505, new byte[] { 205, 197, 73 })]
        public void ReadSignedInt(int number, byte[] data)
        {
            MsgPackSpec.ReadInt32(data.ToMultipleSegments(), out var readSize).ShouldBe(number);
            readSize.ShouldBe(data.Length);
        }

        [Theory]
        [InlineData(0, new byte[] { 0x00 })]
        [InlineData(1, new byte[] { 1 })]
        [InlineData(-1, new byte[] { 0xff })]
        [InlineData(sbyte.MinValue, new byte[] { 208, 128 })]
        [InlineData(sbyte.MaxValue, new byte[] { 127 })]
        [InlineData(short.MinValue, new byte[] { 209, 128, 0 })]
        [InlineData(short.MaxValue, new byte[] { 209, 127, 0xff })]
        public void ReadSignedShort(short number, byte[] data)
        {
            MsgPackSpec.ReadInt16(data.ToMultipleSegments(), out var readSize).ShouldBe(number);
            readSize.ShouldBe(data.Length);
        }

        [Theory]
        [InlineData(0, new byte[] { 0x00 })]
        [InlineData(1, new byte[] { 1 })]
        [InlineData(-1, new byte[] { 0xff })]
        [InlineData(sbyte.MinValue, new byte[] { 208, 128 })]
        [InlineData(sbyte.MaxValue, new byte[] { 127 })]
        public void ReadSignedByte(sbyte number, byte[] data)
        {
            MsgPackSpec.ReadInt8(data.ToMultipleSegments(), out var readSize).ShouldBe(number);
            readSize.ShouldBe(data.Length);
        }

        [Theory]
        [InlineData(0, new byte[] { 0x00 })]
        [InlineData(1, new byte[] { 1 })]
        [InlineData(byte.MaxValue, new byte[] { 0xcc, 0xff })]
        [InlineData(ushort.MaxValue, new byte[] { 0xcd, 0xff, 0xff })]
        [InlineData(uint.MaxValue, new byte[] { 0xce, 0xff, 0xff, 0xff, 0xff })]
        [InlineData(ulong.MaxValue, new byte[] { 0xcf, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff })]
        public void ReadUnsignedLong(ulong number, byte[] data)
        {
            MsgPackSpec.ReadUInt64(data.ToMultipleSegments(), out var readSize).ShouldBe(number);
            readSize.ShouldBe(data.Length);
        }

        [Theory]
        [InlineData(0, new byte[] { 0x00 })]
        [InlineData(1, new byte[] { 1 })]
        [InlineData(byte.MaxValue, new byte[] { 0xcc, 0xff })]
        [InlineData(ushort.MaxValue, new byte[] { 0xcd, 0xff, 0xff })]
        [InlineData(uint.MaxValue, new byte[] { 0xce, 0xff, 0xff, 0xff, 0xff })]
        [InlineData(0x10000000, new byte[] { 0xce, 0x10, 0x00, 0x00, 0x00 })]
        public void ReadUnsignedInt(uint number, byte[] data)
        {
            MsgPackSpec.ReadUInt32(data.ToMultipleSegments(), out var readSize).ShouldBe(number);
            readSize.ShouldBe(data.Length);
        }

        [Theory]
        [InlineData(0, new byte[] { 0x00 })]
        [InlineData(1, new byte[] { 1 })]
        [InlineData(byte.MaxValue, new byte[] { 0xcc, 0xff })]
        [InlineData(ushort.MaxValue, new byte[] { 0xcd, 0xff, 0xff })]
        public void ReadUnsignedShort(ushort number, byte[] data)
        {
            MsgPackSpec.ReadUInt16(data.ToMultipleSegments(), out var readSize).ShouldBe(number);
            readSize.ShouldBe(data.Length);
        }

        [Theory]
        [InlineData(0, new byte[] { 0x00 })]
        [InlineData(1, new byte[] { 1 })]
        [InlineData(byte.MaxValue, new byte[] { 0xcc, 0xff })]
        public void ReadUnsignedByte(byte number, byte[] data)
        {
            MsgPackSpec.ReadUInt8(data.ToMultipleSegments(), out var readSize).ShouldBe(number);
            readSize.ShouldBe(data.Length);
        }
        [Theory]
        [InlineData(0, new byte[] { 0x00 })]
        [InlineData(1, new byte[] { 1 })]
        [InlineData(-1, new byte[] { 0xff })]
        [InlineData(sbyte.MinValue, new byte[] { 208, 128 })]
        [InlineData(sbyte.MaxValue, new byte[] { 127 })]
        [InlineData(short.MinValue, new byte[] { 209, 128, 0 })]
        [InlineData(short.MaxValue, new byte[] { 209, 127, 0xff })]
        [InlineData(int.MinValue, new byte[] { 210, 128, 0, 0, 0 })]
        [InlineData(int.MaxValue, new byte[] { 210, 127, 0xff, 0xff, 0xff })]
        [InlineData(long.MaxValue, new byte[] { 211, 127, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff })]
        [InlineData(long.MinValue, new byte[] { 211, 128, 0, 0, 0, 0, 0, 0, 0 })]
        [InlineData(long.MaxValue, new byte[] { 207, 127, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff })]
        public void TryReadSignedLong(long number, byte[] data)
        {
            MsgPackSpec.TryReadInt64(data.ToMultipleSegments(), out var value, out var readSize).ShouldBeTrue();
            value.ShouldBe(number);
            readSize.ShouldBe(data.Length);
        }

        [Theory]
        [InlineData(0, new byte[] { 0x00 })]
        [InlineData(1, new byte[] { 1 })]
        [InlineData(-1, new byte[] { 0xff })]
        [InlineData(sbyte.MinValue, new byte[] { 208, 128 })]
        [InlineData(sbyte.MaxValue, new byte[] { 127 })]
        [InlineData(short.MinValue, new byte[] { 209, 128, 0 })]
        [InlineData(short.MaxValue, new byte[] { 209, 127, 0xff })]
        [InlineData(int.MinValue, new byte[] { 210, 128, 0, 0, 0 })]
        [InlineData(int.MaxValue, new byte[] { 210, 127, 0xff, 0xff, 0xff })]
        [InlineData(50505, new byte[] { 205, 197, 73 })]
        public void SignedInt(int number, byte[] data)
        {
            MsgPackSpec.TryReadInt32(data.ToMultipleSegments(), out var value, out var readSize).ShouldBeTrue();
            value.ShouldBe(number);
            readSize.ShouldBe(data.Length);
        }

        [Theory]
        [InlineData(0, new byte[] { 0x00 })]
        [InlineData(1, new byte[] { 1 })]
        [InlineData(-1, new byte[] { 0xff })]
        [InlineData(sbyte.MinValue, new byte[] { 208, 128 })]
        [InlineData(sbyte.MaxValue, new byte[] { 127 })]
        [InlineData(short.MinValue, new byte[] { 209, 128, 0 })]
        [InlineData(short.MaxValue, new byte[] { 209, 127, 0xff })]
        public void SignedShort(short number, byte[] data)
        {
            MsgPackSpec.TryReadInt16(data.ToMultipleSegments(), out var value, out var readSize).ShouldBeTrue();
            value.ShouldBe(number);
            readSize.ShouldBe(data.Length);
        }

        [Theory]
        [InlineData(0, new byte[] { 0x00 })]
        [InlineData(1, new byte[] { 1 })]
        [InlineData(-1, new byte[] { 0xff })]
        [InlineData(sbyte.MinValue, new byte[] { 208, 128 })]
        [InlineData(sbyte.MaxValue, new byte[] { 127 })]
        public void SignedByte(sbyte number, byte[] data)
        {
            MsgPackSpec.TryReadInt8(data.ToMultipleSegments(), out var value, out var readSize).ShouldBeTrue();
            value.ShouldBe(number);
            readSize.ShouldBe(data.Length);
        }

        [Theory]
        [InlineData(0, new byte[] { 0x00 })]
        [InlineData(1, new byte[] { 1 })]
        [InlineData(byte.MaxValue, new byte[] { 0xcc, 0xff })]
        [InlineData(ushort.MaxValue, new byte[] { 0xcd, 0xff, 0xff })]
        [InlineData(uint.MaxValue, new byte[] { 0xce, 0xff, 0xff, 0xff, 0xff })]
        [InlineData(ulong.MaxValue, new byte[] { 0xcf, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff })]
        public void UnsignedLong(ulong number, byte[] data)
        {
            MsgPackSpec.TryReadUInt64(data.ToMultipleSegments(), out var value, out var readSize).ShouldBeTrue();
            value.ShouldBe(number);
            readSize.ShouldBe(data.Length);
        }

        [Theory]
        [InlineData(0, new byte[] { 0x00 })]
        [InlineData(1, new byte[] { 1 })]
        [InlineData(byte.MaxValue, new byte[] { 0xcc, 0xff })]
        [InlineData(ushort.MaxValue, new byte[] { 0xcd, 0xff, 0xff })]
        [InlineData(uint.MaxValue, new byte[] { 0xce, 0xff, 0xff, 0xff, 0xff })]
        [InlineData(0x10000000, new byte[] { 0xce, 0x10, 0x00, 0x00, 0x00 })]
        public void UnsignedInt(uint number, byte[] data)
        {
            MsgPackSpec.TryReadUInt32(data.ToMultipleSegments(), out var value, out var readSize).ShouldBeTrue();
            value.ShouldBe(number);
            readSize.ShouldBe(data.Length);
        }

        [Theory]
        [InlineData(0, new byte[] { 0x00 })]
        [InlineData(1, new byte[] { 1 })]
        [InlineData(byte.MaxValue, new byte[] { 0xcc, 0xff })]
        [InlineData(ushort.MaxValue, new byte[] { 0xcd, 0xff, 0xff })]
        public void UnsignedShort(ushort number, byte[] data)
        {
            MsgPackSpec.TryReadUInt16(data.ToMultipleSegments(), out var value, out var readSize).ShouldBeTrue();
            value.ShouldBe(number);
            readSize.ShouldBe(data.Length);
        }

        [Theory]
        [InlineData(0, new byte[] { 0x00 })]
        [InlineData(1, new byte[] { 1 })]
        [InlineData(byte.MaxValue, new byte[] { 0xcc, 0xff })]
        public void UnsignedByte(byte number, byte[] data)
        {
            MsgPackSpec.TryReadUInt8(data.ToMultipleSegments(), out var value, out var readSize).ShouldBeTrue();
            value.ShouldBe(number);
            readSize.ShouldBe(data.Length);
        }
    }
}
