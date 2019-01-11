using System.Buffers;
using Shouldly;
using Xunit;

namespace ProGaudi.MsgPack.Tests.Writer
{
    public sealed class Array
    {
        //[Fact]
        //public void SimpleArray()
        //{
        //    var tests = new[]
        //    {
        //        "a",
        //        "b",
        //        "c",
        //        "d",
        //        "e"
        //    };

        //    var bytes = new byte[]
        //    {
        //        149,
        //        161, 97,
        //        161, 98,
        //        161, 99,
        //        161, 100,
        //        161, 101
        //    };

        //    using (var buffer = MemoryPool<string>.Shared.Rent(tests.Length))
        //    {
        //        var temp = bytes.AsSpan();
        //        var length = MsgPackSpec.ReadArrayHeader(temp, out var readSize);
        //        length.ShouldBe(tests.Length);

        //        for (var i = 0; i < length; i++)
        //        {
        //            buffer.Memory.Span[i] = MsgPackSpec.ReadString(temp.Slice(readSize), out readSize);
        //        }

        //        buffer.Memory.ToArray().ShouldBe(tests);
        //    }
        //}

        [Fact]
        public void TestNonFixedArray()
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
            };

            using (var buffer = MemoryPool<byte>.Shared.Rent(bytes.Length))
            {
                var span = buffer.Memory.Span;
                var wroteSize = MsgPackSpec.WriteArrayHeader(span, tests.Length);
                foreach (var test in tests)
                {
                    wroteSize += MsgPackSpec.WriteInt32(span.Slice(wroteSize), test);
                }

                wroteSize.ShouldBe(bytes.Length);
                span.Slice(0, wroteSize).ToArray().ShouldBe(bytes);
            }
        }
    }
}
