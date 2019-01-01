using System;
using Shouldly;
using Xunit;

namespace ProGaudi.MsgPack.Tests.Reader
{
    public sealed class ReadToken
    {
        [Theory]
        [InlineData(new byte[0],
            0)]
        [InlineData(new byte[]
            {
                161, 97,
                161, 98,
                161, 99,
                161, 100,
                161, 101
            },
            5)]
        [InlineData(new byte[]
            {
                149,
                161, 97,
                161, 98,
                161, 99,
                161, 100,
                161, 101
            },
            1)]
        [InlineData(new byte[]
             {
                151,
                0,
                205, 197, 73,
                202, 255, 192, 0, 0,
                202, 127, 127, 255, 255,
                147,
                    195,
                    194,
                    195,
                192,
                129,
                    164, 66, 97, 108, 108, 166, 83, 111, 99, 99, 101, 114
            },
            1)]
        [InlineData(new byte[]
             {
                0,
                205, 197, 73,
                202, 255, 192, 0, 0,
                202, 127, 127, 255, 255,
                147,
                    195,
                    194,
                    195,
                192,
                129,
                    164, 66, 97, 108, 108, 166, 83, 111, 99, 99, 101, 114
            },
            7)]
        [InlineData(new byte[]
              {
                0xdc,
                0x00,
                0x14,

                0x01, 0x02, 0x03, 0x04, 0x05,
                0x01, 0x02, 0x03, 0x04, 0x05,
                0x01, 0x02, 0x03, 0x04, 0x05,
                0x01, 0x02, 0x03, 0x04, 0x05,
            },
            1)]
        [InlineData(new byte[]
              {
                0x01, 0x02, 0x03, 0x04, 0x05,
                0x01, 0x02, 0x03, 0x04, 0x05,
                0x01, 0x02, 0x03, 0x04, 0x05,
                0x01, 0x02, 0x03, 0x04, 0x05,
            },
            20)]
        [InlineData(new byte[]
              {
            197, 1, 44,

            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,

            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,

            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0
        },
            1)]
        [InlineData(new byte[]
              {
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,

            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,

            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0
        },
            300)]
        [InlineData(new byte[]
              {
                138,
                166, 97, 114, 114, 97, 121, 49,
                    147,
                    173, 97, 114, 114, 97, 121, 49, 95, 118, 97, 108, 117, 101, 49,
                    173, 97, 114, 114, 97, 121, 49, 95, 118, 97, 108, 117, 101, 50,
                    173, 97, 114, 114, 97, 121, 49, 95, 118, 97, 108, 117, 101, 51,
                165, 98, 111, 111, 108, 49, 195,
                167, 100, 111, 117, 98, 108, 101, 49, 203, 64, 73, 64, 0, 0, 0, 0, 0,
                167, 100, 111, 117, 98, 108, 101, 50, 203, 64, 46, 102, 102, 102, 102, 102, 102,
                164, 105, 110, 116, 49, 205, 197, 73,
                164, 105, 110, 116, 50, 50,
                202, 64, 72, 245, 195, 203, 64, 9, 30, 184, 81, 235, 133, 31,
                42, 42,
                129, 1, 2, 192,
                146, 1, 2, 192
            },
            1)]
        [InlineData(new byte[]
              {
                166, 97, 114, 114, 97, 121, 49,
                    147,
                    173, 97, 114, 114, 97, 121, 49, 95, 118, 97, 108, 117, 101, 49,
                    173, 97, 114, 114, 97, 121, 49, 95, 118, 97, 108, 117, 101, 50,
                    173, 97, 114, 114, 97, 121, 49, 95, 118, 97, 108, 117, 101, 51,
                165, 98, 111, 111, 108, 49, 195,
                167, 100, 111, 117, 98, 108, 101, 49, 203, 64, 73, 64, 0, 0, 0, 0, 0,
                167, 100, 111, 117, 98, 108, 101, 50, 203, 64, 46, 102, 102, 102, 102, 102, 102,
                164, 105, 110, 116, 49, 205, 197, 73,
                164, 105, 110, 116, 50, 50,
                202, 64, 72, 245, 195, 203, 64, 9, 30, 184, 81, 235, 133, 31,
                42, 42,
                129, 1, 2, 192,
                146, 1, 2, 192
            },
            20)]
        [InlineData(new byte[]
              {
                0xde,
                0x00,
                0x14,

                0x01, 0xa1, 0x61,
                0x02, 0xa1, 0x62,
                0x03, 0xa1, 0x63,
                0x04, 0xa1, 0x64,
                0x05, 0xa1, 0x65,

                0x0b, 0xa1, 0x61,
                0x0c, 0xa1, 0x62,
                0x0d, 0xa1, 0x63,
                0x0e, 0xa1, 0x64,
                0x0f, 0xa1, 0x65,

                0x15, 0xa1, 0x61,
                0x16, 0xa1, 0x62,
                0x17, 0xa1, 0x63,
                0x18, 0xa1, 0x64,
                0x19, 0xa1, 0x65,

                0x1f, 0xa1, 0x61,
                0x20, 0xa1, 0x62,
                0x21, 0xa1, 0x63,
                0x22, 0xa1, 0x64,
                0x23, 0xa1, 0x65,
            },
            1)]
        [InlineData(new byte[]
              {
                0x01, 0xa1, 0x61,
                0x02, 0xa1, 0x62,
                0x03, 0xa1, 0x63,
                0x04, 0xa1, 0x64,
                0x05, 0xa1, 0x65,

                0x0b, 0xa1, 0x61,
                0x0c, 0xa1, 0x62,
                0x0d, 0xa1, 0x63,
                0x0e, 0xa1, 0x64,
                0x0f, 0xa1, 0x65,

                0x15, 0xa1, 0x61,
                0x16, 0xa1, 0x62,
                0x17, 0xa1, 0x63,
                0x18, 0xa1, 0x64,
                0x19, 0xa1, 0x65,

                0x1f, 0xa1, 0x61,
                0x20, 0xa1, 0x62,
                0x21, 0xa1, 0x63,
                0x22, 0xa1, 0x64,
                0x23, 0xa1, 0x65,
            },
            40)]
        [InlineData(new byte[] {
                    218, 1, 80, 48, 56, 54, 49, 50, 51, 54, 52, 49, 50, 51, 48, 54, 53, 55, 49, 50, 54, 51, 57, 103, 114,
                    49, 104, 50, 106, 51, 103, 114, 116, 107, 49, 104, 50, 51, 107, 103, 102, 114, 116, 49, 104, 106, 50,
                    103, 51, 102, 106, 114, 103, 102, 49, 106, 50, 104, 103, 48, 56, 54, 49, 50, 51, 54, 52, 49, 50, 51,
                    48, 54, 53, 55, 49, 50, 54, 51, 57, 103, 114, 49, 104, 50, 106, 51, 103, 114, 116, 107, 49, 104, 50,
                    51, 107, 103, 102, 114, 116, 49, 104, 106, 50, 103, 51, 102, 106, 114, 103, 102, 49, 106, 50, 104,
                    103, 48, 56, 54, 49, 50, 51, 54, 52, 49, 50, 51, 48, 54, 53, 55, 49, 50, 54, 51, 57, 103, 114, 49,
                    104, 50, 106, 51, 103, 114, 116, 107, 49, 104, 50, 51, 107, 103, 102, 114, 116, 49, 104, 106, 50,
                    103, 51, 102, 106, 114, 103, 102, 49, 106, 50, 104, 103, 48, 56, 54, 49, 50, 51, 54, 52, 49, 50, 51,
                    48, 54, 53, 55, 49, 50, 54, 51, 57, 103, 114, 49, 104, 50, 106, 51, 103, 114, 116, 107, 49, 104, 50,
                    51, 107, 103, 102, 114, 116, 49, 104, 106, 50, 103, 51, 102, 106, 114, 103, 102, 49, 106, 50, 104,
                    103, 48, 56, 54, 49, 50, 51, 54, 52, 49, 50, 51, 48, 54, 53, 55, 49, 50, 54, 51, 57, 103, 114, 49,
                    104, 50, 106, 51, 103, 114, 116, 107, 49, 104, 50, 51, 107, 103, 102, 114, 116, 49, 104, 106, 50,
                    103, 51, 102, 106, 114, 103, 102, 49, 106, 50, 104, 103, 48, 56, 54, 49, 50, 51, 54, 52, 49, 50, 51,
                    48, 54, 53, 55, 49, 50, 54, 51, 57, 103, 114, 49, 104, 50, 106, 51, 103, 114, 116, 107, 49, 104, 50,
                    51, 107, 103, 102, 114, 116, 49, 104, 106, 50, 103, 51, 102, 106, 114, 103, 102, 49, 106, 50, 104,
                    103
                },
            1)]
        public void NonEmptyRead(byte[] data, int objectsCount)
        {
            ReadOnlySpan<byte> buffer = data;
            for (var i = 0; i < objectsCount; i++)
            {
                buffer = buffer.Slice(MsgPackSpec.ReadToken(buffer).Length);
            }

            buffer.IsEmpty.ShouldBeTrue();
        }

        [Fact]
        public void EmptyRead()
        {
            var e = Should.Throw<ArgumentOutOfRangeException>(() => MsgPackSpec.ReadToken(System.Array.Empty<byte>()));
            e.Message.ShouldBe($"EOF: Buffer is empty.{Environment.NewLine}Parameter name: buffer");
            e.ParamName.ShouldBe("buffer");
        }

        [Fact]
        public void NeverUsedRead()
        {
            var e = Should.Throw<ArgumentOutOfRangeException>(() => MsgPackSpec.ReadToken(new [] {DataCodes.NeverUsed}));
            e.Message.ShouldBe($"Data code at buffer[0] is 0xc1 and it is invalid data code.{Environment.NewLine}Parameter name: buffer");
            e.ParamName.ShouldBe("buffer");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1_000)]
        [InlineData(10_000)]
        [InlineData(100_000)]
        [InlineData(1_000_000)]
#if !DEBUG
        [InlineData(10_000_000)]
        [InlineData(100_000_000)]
        [InlineData(1_000_000_000)]
#endif
        public void DeepStack(int depth)
        {
            var buffer = new byte[depth];
            var span = buffer.AsSpan();
            while (span.Length > 1)
                span = span.Slice(MsgPackSpec.WriteFixArrayHeader(span, 1));

            MsgPackSpec.WritePositiveFixInt(span, 1);

            var token = MsgPackSpec.ReadToken(buffer);
            token.Length.ShouldBe(buffer.Length);
        }
    }
}
