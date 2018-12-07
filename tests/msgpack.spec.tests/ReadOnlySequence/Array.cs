using System.Buffers;
using Shouldly;
using Xunit;

namespace ProGaudi.MsgPack.Tests.ReadOnlySequence
{
    public sealed class Array
    {
        [Fact]
        public void WriteTestNonFixedArray()
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
                var wroteSize = MsgPackSpec.WriteArrayHeader(buffer.Memory.Span, tests.Length);
                foreach (var test in tests)
                {
                    wroteSize += MsgPackSpec.WriteInt32(buffer.Memory.Span.Slice(wroteSize), test);
                }

                wroteSize.ShouldBe(bytes.Length);
                buffer.Memory.Span.Slice(0, wroteSize).ToArray().ShouldBe(bytes);
            }
        }
    }
}
