using System.Buffers;
using Shouldly;
using Xunit;

namespace ProGaudi.MsgPack.Tests.ReadOnlySequence
{
    public sealed class Array
    {
        [Fact]
        public void SingleSegmentTestNonFixedArray()
        {
            var tests = new[]
            {
                1, 2, 3, 4, 5,
                1, 2, 3, 4, 5,
                1, 2, 3, 4, 5,
                1, 2, 3, 4, 5,
            };

            var bytes = new byte[]
            {
                0xdc,
                0x00,
                0x14,

                0x01, 0x02, 0x03, 0x04, 0x05,
                0x01, 0x02, 0x03, 0x04, 0x05,
                0x01, 0x02, 0x03, 0x04, 0x05,
                0x01, 0x02, 0x03, 0x04, 0x05,
            }.ToSingleSegment();

            var length = MsgPackSpec.ReadArrayHeader(bytes, out var readSize);
            length.ShouldBe(tests.Length);

            var actual = new int[length];
            for (var i = 0; i < length; i++)
            {
                actual[i] = MsgPackSpec.ReadInt32(bytes.Slice(readSize), out var temp);
                readSize += temp;
            }

            actual.ShouldBe(tests);
        }


        [Fact]
        public void MultipleSegmentsTestNonFixedArray()
        {
            var tests = new[]
            {
                1, 2, 3, 4, 5,
                1, 2, 3, 4, 5,
                1, 2, 3, 4, 5,
                1, 2, 3, 4, 5,
            };

            var bytes = new byte[]
            {
                0xdc,
                0x00,
                0x14,

                0x01, 0x02, 0x03, 0x04, 0x05,
                0x01, 0x02, 0x03, 0x04, 0x05,
                0x01, 0x02, 0x03, 0x04, 0x05,
                0x01, 0x02, 0x03, 0x04, 0x05,
            }.ToMultipleSegments();

            var length = MsgPackSpec.ReadArrayHeader(bytes, out var readSize);
            length.ShouldBe(tests.Length);

            var actual = new int[length];
            for (var i = 0; i < length; i++)
            {
                actual[i] = MsgPackSpec.ReadInt32(bytes.Slice(readSize), out var temp);
                readSize += temp;
            }

            actual.ShouldBe(tests);
        }
    }
}
