using Shouldly;
using Xunit;

namespace ProGaudi.MsgPack.Tests.ReadOnlySequence.MultipleSegments
{
    public sealed class Array
    {
//        [Fact]
//        public void SimpleArrayRead()
//        {
//            var tests = new[]
//            {
//                "a",
//                "b",
//                "c",
//                "d",
//                "e"
//            };
//
//            var bytes = new byte[]
//            {
//                149,
//                161, 97,
//                161, 98,
//                161, 99,
//                161, 100,
//                161, 101
//            }.ToMultipleSegments();
//
//            var length = MsgPackSpec.ReadArrayHeader(bytes, out var readSize);
//            length.ShouldBe(tests.Length);
//
//            var actual = new string[length];
//            for (var i = 0; i < length; i++)
//            {
//                actual[i] = MsgPackSpec.ReadString(bytes.Slice(readSize), out var temp);
//                readSize += temp;
//            }
//
//            actual.ShouldBe(tests);
//        }

        [Fact]
        public void TestNonFixedArrayRead()
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

//        [Fact]
//        public void SimpleArrayTryRead()
//        {
//            var tests = new[]
//            {
//                "a",
//                "b",
//                "c",
//                "d",
//                "e"
//            };
//
//            Span<byte> bytes = new byte[]
//            {
//                149,
//                161, 97,
//                161, 98,
//                161, 99,
//                161, 100,
//                161, 101
//            };
//
//            MsgPackSpec.TryReadArrayHeader(bytes, out var length, out var readSize).ShouldBeTrue();
//            length.ShouldBe(tests.Length);
//
//            var actual = new string[length];
//            for (var i = 0; i < length; i++)
//            {
//                actual[i] = MsgPackSpec.ReadString(bytes.Slice(readSize), out var temp);
//                readSize += temp;
//            }
//
//            actual.ShouldBe(tests);
//        }

        [Fact]
        public void TestNonFixedArrayTryRead()
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

            MsgPackSpec.TryReadArrayHeader(bytes, out var length, out var readSize).ShouldBeTrue();
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
