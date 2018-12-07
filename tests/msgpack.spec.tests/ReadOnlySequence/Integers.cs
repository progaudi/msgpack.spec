using Shouldly;
using Xunit;

namespace ProGaudi.MsgPack.Tests.ReadOnlySequence
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
        public void SingleSegmentTestSignedLong(long number, byte[] data)
        {
            MsgPackSpec.ReadInt64(data.ToSingleSegment(), out var readSize).ShouldBe(number);
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
        public void SingleSegmentTestSignedInt(int number, byte[] data)
        {
            MsgPackSpec.ReadInt32(data.ToSingleSegment(), out var readSize).ShouldBe(number);
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
        public void SingleSegmentTestSignedShort(short number, byte[] data)
        {
            MsgPackSpec.ReadInt16(data.ToSingleSegment(), out var readSize).ShouldBe(number);
            readSize.ShouldBe(data.Length);
        }

        [Theory]
        [InlineData(0, new byte[] { 0x00 })]
        [InlineData(1, new byte[] { 1 })]
        [InlineData(-1, new byte[] { 0xff })]
        [InlineData(sbyte.MinValue, new byte[] { 208, 128 })]
        [InlineData(sbyte.MaxValue, new byte[] { 127 })]
        public void SingleSegmentTestSignedByte(sbyte number, byte[] data)
        {
            MsgPackSpec.ReadInt8(data.ToSingleSegment(), out var readSize).ShouldBe(number);
            readSize.ShouldBe(data.Length);
        }

        [Theory]
        [InlineData(0, new byte[] { 0x00 })]
        [InlineData(1, new byte[] { 1 })]
        [InlineData(byte.MaxValue, new byte[] { 0xcc, 0xff })]
        [InlineData(ushort.MaxValue, new byte[] { 0xcd, 0xff, 0xff })]
        [InlineData(uint.MaxValue, new byte[] { 0xce, 0xff, 0xff, 0xff, 0xff })]
        [InlineData(ulong.MaxValue, new byte[] { 0xcf, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff })]
        public void SingleSegmentTestUnsignedLong(ulong number, byte[] data)
        {
            MsgPackSpec.ReadUInt64(data.ToSingleSegment(), out var readSize).ShouldBe(number);
            readSize.ShouldBe(data.Length);
        }

        [Theory]
        [InlineData(0, new byte[] { 0x00 })]
        [InlineData(1, new byte[] { 1 })]
        [InlineData(byte.MaxValue, new byte[] { 0xcc, 0xff })]
        [InlineData(ushort.MaxValue, new byte[] { 0xcd, 0xff, 0xff })]
        [InlineData(uint.MaxValue, new byte[] { 0xce, 0xff, 0xff, 0xff, 0xff })]
        [InlineData(0x10000000, new byte[] { 0xce, 0x10, 0x00, 0x00, 0x00 })]
        public void SingleSegmentTestUnsignedInt(uint number, byte[] data)
        {
            MsgPackSpec.ReadUInt32(data.ToSingleSegment(), out var readSize).ShouldBe(number);
            readSize.ShouldBe(data.Length);
        }

        [Theory]
        [InlineData(0, new byte[] { 0x00 })]
        [InlineData(1, new byte[] { 1 })]
        [InlineData(byte.MaxValue, new byte[] { 0xcc, 0xff })]
        [InlineData(ushort.MaxValue, new byte[] { 0xcd, 0xff, 0xff })]
        public void SingleSegmentTestUnsignedShort(ushort number, byte[] data)
        {
            MsgPackSpec.ReadUInt16(data.ToSingleSegment(), out var readSize).ShouldBe(number);
            readSize.ShouldBe(data.Length);
        }

        [Theory]
        [InlineData(0, new byte[] { 0x00 })]
        [InlineData(1, new byte[] { 1 })]
        [InlineData(byte.MaxValue, new byte[] { 0xcc, 0xff })]
        public void SingleSegmentTestUnsignedByte(byte number, byte[] data)
        {
            MsgPackSpec.ReadUInt8(data.ToSingleSegment(), out var readSize).ShouldBe(number);
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
        public void MultipleSegmentsTestSignedLong(long number, byte[] data)
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
        public void MultipleSegmentsTestSignedInt(int number, byte[] data)
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
        public void MultipleSegmentsTestSignedShort(short number, byte[] data)
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
        public void MultipleSegmentsTestSignedByte(sbyte number, byte[] data)
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
        public void MultipleSegmentsTestUnsignedLong(ulong number, byte[] data)
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
        public void MultipleSegmentsTestUnsignedInt(uint number, byte[] data)
        {
            MsgPackSpec.ReadUInt32(data.ToMultipleSegments(), out var readSize).ShouldBe(number);
            readSize.ShouldBe(data.Length);
        }

        [Theory]
        [InlineData(0, new byte[] { 0x00 })]
        [InlineData(1, new byte[] { 1 })]
        [InlineData(byte.MaxValue, new byte[] { 0xcc, 0xff })]
        [InlineData(ushort.MaxValue, new byte[] { 0xcd, 0xff, 0xff })]
        public void MultipleSegmentsTestUnsignedShort(ushort number, byte[] data)
        {
            MsgPackSpec.ReadUInt16(data.ToMultipleSegments(), out var readSize).ShouldBe(number);
            readSize.ShouldBe(data.Length);
        }

        [Theory]
        [InlineData(0, new byte[] { 0x00 })]
        [InlineData(1, new byte[] { 1 })]
        [InlineData(byte.MaxValue, new byte[] { 0xcc, 0xff })]
        public void MultipleSegmentsTestUnsignedByte(byte number, byte[] data)
        {
            MsgPackSpec.ReadUInt8(data.ToMultipleSegments(), out var readSize).ShouldBe(number);
            readSize.ShouldBe(data.Length);
        }
    }
}
