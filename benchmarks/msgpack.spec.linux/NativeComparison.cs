using System;
using System.Buffers;
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

        [Benchmark(Baseline = true)]
        public void MsgPackSpecArray()
        {
            var buffer = _buffer.AsSpan();
            var wroteSize = MsgPackSpec.WriteArray16Header(buffer, length);
            for (var i = 0u; i < length; i++)
                wroteSize += MsgPackSpec.WriteUInt32(buffer.Slice(wroteSize), baseInt - i);
        }

        [Benchmark]
        public void MsgPackSpecPointer()
        {
            fixed (byte* pointer = &_buffer.AsSpan().GetPinnableReference())
            {
                pointer[0] = DataCodes.Array16;
                Unsafe.WriteUnaligned(ref pointer[1], length);
                for (var i = 0u; i < length; i++)
                {
                    pointer[3 + 5 * i] = DataCodes.UInt32;
                    Unsafe.WriteUnaligned(ref pointer[3 + 5 * i + 1], baseInt - i);
                }
            }
        }

        [Benchmark]
        public void CArray() => CNative.SerializeArray();

        [Benchmark]
        public void CppArray() => CppNative.SerializeArray();

        private static class CNative
        {
            [DllImport("libcMsgPack.so", EntryPoint = "serializeIntArray", CallingConvention = CallingConvention.Cdecl)]
            public static extern void SerializeArray();
        }

        private static class CppNative
        {
            [DllImport("libcppMsgPack.so", EntryPoint = "serializeIntArray", CallingConvention = CallingConvention.Cdecl)]
            public static extern void SerializeArray();
        }
    }
}
