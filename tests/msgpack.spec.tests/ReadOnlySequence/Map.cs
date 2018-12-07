using System;
using System.Collections.Generic;
using Shouldly;
using Xunit;

namespace ProGaudi.MsgPack.Tests.ReadOnlySequence
{
    public sealed class Map
    {
        //[Fact]
        //public void ComplexDictionary()
        //{
        //    var tests = new Dictionary<object, object>
        //    {
        //        {
        //            "array1",
        //            new object[]
        //            {
        //                "array1_value1",
        //                "array1_value2",
        //                "array1_value3"
        //            }
        //        },
        //        {"bool1", true},
        //        {"double1", 50.5},
        //        {"double2", 15.2},
        //        {"int1", 50505},
        //        {"int2", 50},
        //        {3.14f, 3.14},
        //        {42, 42},
        //        {new Dictionary<int, int> {{1, 2}}, null},
        //        {new[] {1, 2}, null}
        //    };

        //    var data = new byte[]
        //    {
        //        138,
        //        166, 97, 114, 114, 97, 121, 49,
        //            147,
        //            173, 97, 114, 114, 97, 121, 49, 95, 118, 97, 108, 117, 101, 49,
        //            173, 97, 114, 114, 97, 121, 49, 95, 118, 97, 108, 117, 101, 50,
        //            173, 97, 114, 114, 97, 121, 49, 95, 118, 97, 108, 117, 101, 51,
        //        165, 98, 111, 111, 108, 49, 195,
        //        167, 100, 111, 117, 98, 108, 101, 49, 203, 64, 73, 64, 0, 0, 0, 0, 0,
        //        167, 100, 111, 117, 98, 108, 101, 50, 203, 64, 46, 102, 102, 102, 102, 102, 102,
        //        164, 105, 110, 116, 49, 205, 197, 73,
        //        164, 105, 110, 116, 50, 50,
        //        202, 64, 72, 245, 195, 203, 64, 9, 30, 184, 81, 235, 133, 31,
        //        42, 42,
        //        129, 1, 2, 192,
        //        146, 1, 2, 192
        //    };

        //    var settings = new MsgPackContext();
        //    settings.RegisterConverter(new TestReflectionConverter());
        //    MsgPackSerializer.Serialize(tests, settings).ShouldBe(data);

        //    Helpers.CheckTokenDeserialization(data);
        //}


        [Fact]
        public void SingleSegmentSimpleInt()
        {
            var test = new Dictionary<int, int>
            {
                {1, 1},
                {2, 2},
                {3, 3},
                {4, 4},
                {5, 5}
            };

            var bytes = new byte[]
            {
                133,
                1, 1,
                2, 2,
                3, 3,
                4, 4,
                5, 5,
            };

            TestReadDictionary(bytes.ToSingleSegment(), test, MsgPackSpec.ReadInt32);
        }


        [Fact]
        public void SingleSegmentNonFixedDictionaryInt()
        {
            var bytes = new byte[]
            {
                0xde,
                0x00,
                0x14,

                0x01, 0x01,
                0x02, 0x02,
                0x03, 0x03,
                0x04, 0x04,
                0x05, 0x05,

                0x0b, 0x0b,
                0x0c, 0x0c,
                0x0d, 0x0d,
                0x0e, 0x0e,
                0x0f, 0x0f,

                0x15, 0x15,
                0x16, 0x16,
                0x17, 0x17,
                0x18, 0x18,
                0x19, 0x19,

                0x1f, 0x1f,
                0x20, 0x20,
                0x21, 0x21,
                0x22, 0x22,
                0x23, 0x23,
            };

            var test = new Dictionary<int, int>
            {
                {1, 1},
                {2, 2},
                {3, 3},
                {4, 4},
                {5, 5},

                {11, 11},
                {12, 12},
                {13, 13},
                {14, 14},
                {15, 15},

                {21, 21},
                {22, 22},
                {23, 23},
                {24, 24},
                {25, 25},

                {31, 31},
                {32, 32},
                {33, 33},
                {34, 34},
                {35, 35},
            };

            TestReadDictionary(bytes.ToSingleSegment(), test, MsgPackSpec.ReadInt32);
        }
        [Fact]
        public void MultipleSegmentsSimpleInt()
        {
            var test = new Dictionary<int, int>
            {
                {1, 1},
                {2, 2},
                {3, 3},
                {4, 4},
                {5, 5}
            };

            var bytes = new byte[]
            {
                133,
                1, 1,
                2, 2,
                3, 3,
                4, 4,
                5, 5,
            };

            TestReadDictionary(bytes.ToMultipleSegments(), test, MsgPackSpec.ReadInt32);
        }

        [Fact]
        public void MultipleSegmentsNonFixedDictionaryInt()
        {
            var bytes = new byte[]
            {
                0xde,
                0x00,
                0x14,

                0x01, 0x01,
                0x02, 0x02,
                0x03, 0x03,
                0x04, 0x04,
                0x05, 0x05,

                0x0b, 0x0b,
                0x0c, 0x0c,
                0x0d, 0x0d,
                0x0e, 0x0e,
                0x0f, 0x0f,

                0x15, 0x15,
                0x16, 0x16,
                0x17, 0x17,
                0x18, 0x18,
                0x19, 0x19,

                0x1f, 0x1f,
                0x20, 0x20,
                0x21, 0x21,
                0x22, 0x22,
                0x23, 0x23,
            };

            var test = new Dictionary<int, int>
            {
                {1, 1},
                {2, 2},
                {3, 3},
                {4, 4},
                {5, 5},

                {11, 11},
                {12, 12},
                {13, 13},
                {14, 14},
                {15, 15},

                {21, 21},
                {22, 22},
                {23, 23},
                {24, 24},
                {25, 25},

                {31, 31},
                {32, 32},
                {33, 33},
                {34, 34},
                {35, 35},
            };

            TestReadDictionary(bytes.ToMultipleSegments(), test, MsgPackSpec.ReadInt32);
        }

        private delegate T ValueExtractor<T>(ReadOnlySequence<byte> bytes, out int readSize);
        private static void TestReadDictionary<T>(ReadOnlySequence<byte> bytes, Dictionary<int, T> test, ValueExtractor<T> extractor)
        {
            var length = (int) MsgPackSpec.ReadMapHeader(bytes, out var readSize);
            length.ShouldBe(test.Count);
            var dictionary = new Dictionary<int, T>(length);
            for (var i = 0; i < length; i++)
            {
                bytes = bytes.Slice(readSize);
                var key = MsgPackSpec.ReadInt32(bytes, out readSize);
                bytes = bytes.Slice(readSize);
                dictionary[key] = extractor(bytes, out readSize);
            }

            dictionary.ShouldBe(test, true);
        }
    }
}
