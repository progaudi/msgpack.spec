using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using ProGaudi.MsgPack;

namespace msgpack.spec.linux
{
    [MemoryDiagnoser]
    [Q3Column]
    [MarkdownExporterAttribute.GitHub]
    public unsafe class NativeComparison
    {
        private const ushort length = 100;
        private const uint baseInt = 1 << 30;
        private readonly byte[] _buffer = ArrayPool<byte>.Shared.Rent(short.MaxValue);

        [Benchmark]
        public void MsgPackSpecArray()
        {
            var buffer = _buffer.AsSpan();
            var wroteSize = MsgPackSpec.WriteArray16Header(buffer, length);
            for (var i = 0u; i < length; i++)
                wroteSize += MsgPackSpec.WriteUInt32(buffer.Slice(wroteSize), baseInt - i);
        }

        [Benchmark]
        public void PointerBigEndian()
        {
            fixed (byte* pointer = &_buffer[0])
            {
                pointer[0] = DataCodes.Array16;
                Unsafe.WriteUnaligned(ref pointer[1], BinaryPrimitives.ReverseEndianness(length));
                for (var i = 0u; i < length; i++)
                {
                    pointer[3 + 5 * i] = DataCodes.UInt32;
                    Unsafe.WriteUnaligned(ref pointer[3 + 5 * i + 1], BinaryPrimitives.ReverseEndianness(baseInt));
                }
            }
        }

        [Benchmark]
        public void SpanBigEndian()
        {
            var pointer = _buffer.AsSpan();
            var l = BinaryPrimitives.ReverseEndianness(length);
            pointer[0] = DataCodes.Array16;
            MemoryMarshal.Write(pointer.Slice(1), ref l);
            var x = BinaryPrimitives.ReverseEndianness(baseInt);
            for (var i = 0; i < length; i++)
            {
                pointer[3 + 5 * i] = DataCodes.UInt32;
                MemoryMarshal.Write(pointer.Slice(3 + 5 * i + 1), ref x);
            }
        }

        [Benchmark]
        public void SpanBigEndianLength()
        {
            var pointer = _buffer.AsSpan();
            var l = BinaryPrimitives.ReverseEndianness(length);
            pointer[0] = DataCodes.Array16;
            MemoryMarshal.Write(pointer.Slice(1, 2), ref l);
            var x = BinaryPrimitives.ReverseEndianness(baseInt);
            for (var i = 0; i < length; i++)
            {
                pointer[3 + 5 * i] = DataCodes.UInt32;
                MemoryMarshal.Write(pointer.Slice(3 + 5 * i + 1), ref x);
            }
        }

        [Benchmark]
        public void SpanBigEndianBinaryPrimitive()
        {
            var pointer = _buffer.AsSpan();
            pointer[0] = DataCodes.Array16;
            BinaryPrimitives.WriteUInt16BigEndian(pointer.Slice(1), length);
            for (var i = 0; i < length; i++)
            {
                pointer[3 + 5 * i] = DataCodes.UInt32;
                BinaryPrimitives.WriteUInt32BigEndian(pointer.Slice(3 + 5 * i + 1), baseInt);
            }
        }

        [Benchmark(Baseline = true)]
        public void CArray() => CNative.SerializeArray();

        private static class CNative
        {
            [DllImport("libcMsgPack.so", EntryPoint = "serializeIntArray", CallingConvention = CallingConvention.Cdecl)]
            public static extern void SerializeArray();

            [DllImport("libcMsgPack.so", EntryPoint = "empty", CallingConvention = CallingConvention.Cdecl)]
            public static extern void Empty();
        }

        private static class CppNative
        {
            [DllImport("libcppMsgPack.so", EntryPoint = "serializeIntArray", CallingConvention = CallingConvention.Cdecl)]
            public static extern void SerializeArray();
        }
    }
}
